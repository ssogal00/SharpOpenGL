using CompiledMaterial.GBufferMacro1;
using Core;
using Core.Primitive;
using GLTF;
using OpenTK.Graphics.OpenGL;
using Engine;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CameraTransform = CompiledMaterial.GBufferPNTT.CameraTransform;
using ModelTransform = CompiledMaterial.GBufferPNTT.ModelTransform;

namespace Engine
{
    public class GLTFStaticMeshObject : GameObject
    {
        private static readonly Dictionary<string, int> mVertexAttributePriority = new Dictionary<string, int>
        {
            {"POSITION", 0},
            {"NORMAL", 1},
            {"TEXCOORD_0", 2},
            {"TEXCOORD_1", 3},
            {"TANGENT", 4}
        };

        private static int GetSemanticPriority(string semanticName)
        {
            if (semanticName.StartsWith("POSITION", true, null))
            {
                return 0;
            }

            else if (semanticName.StartsWith("NORMAL", true, null))
            {
                return 1;
            }

            else if (semanticName.StartsWith("TEXCOORD_0", true, null))
            {
                return 2;
            }

            else if (semanticName.StartsWith("TANGENT", true, null))
            {
                return 3;
            }

            return 0;
        }

        private static bool CanCreateTangent(List<VertexAttributeSemantic> semanticList)
        {
            bool bContainsTexCoord = false;
            bool bContainsNormal = false;
            bool bContainsTangent = false;
            bool bContainsPosition = false;

            foreach (var semantic in semanticList)
            {
                if (semantic.name.StartsWith("TEXCOORD", true, null))
                {
                    bContainsTexCoord = true;
                }

                if (semantic.name.StartsWith("NORMAL", true, null))
                {
                    bContainsNormal = true;
                }

                if (semantic.name.StartsWith("TANGENT", true, null))
                {
                    bContainsTangent = true;
                }

                if (semantic.name.StartsWith("POSITION", true, null))
                {
                    bContainsPosition = true;
                }
            }

            return bContainsTexCoord && bContainsNormal && (!bContainsTangent) && bContainsPosition;
        }
        

        public GLTFStaticMeshObject(GLTFMeshAsset asset)
        {
            mGLTFAsset = asset;
            mEncodedPBRInfo = EncodePBRInfo();
            RenderingThread.Get().ExecuteImmediatelyIfRenderingThread(() =>
            {
                PrepareRenderingData();
            });
        }

        public override void Render()
        {
            if (bReadyToDraw)
            {
                var mtl = ShaderManager.Get().GetMaterial("GBufferMacro1");

                mtl.Bind();

                mTransformInfo.Proj = CameraManager.Get().CurrentCameraProj;
                mTransformInfo.View = CameraManager.Get().CurrentCameraView;
                mtl.SetUniformBufferValue<CameraTransform>(@"CameraTransform", mTransformInfo);

                mModelTransformInfo.Model = this.LocalMatrix;
                mtl.SetUniformBufferValue<ModelTransform>(@"ModelTransform", mModelTransformInfo);
                
                mMaterialProperty.MetallicExist = true;
                mMaterialProperty.NormalExist = true;
                mMaterialProperty.MaskExist = false;
                mMaterialProperty.MetallicRoughnessOneTexture = true;
                mMaterialProperty.RoghnessExist = true;

                mtl.SetUniformBufferValue<MaterialProperty>(@"MaterialProperty", mMaterialProperty);

                // base 
                if (this.mGLTFAsset.Material.TextureMap.ContainsKey(PBRTextureType.BaseColor))
                {
                    var path = this.mGLTFAsset.Material.TextureMap[PBRTextureType.BaseColor];
                    mtl.SetTexture("DiffuseTex", TextureManager.Get().GetTexture2D(path));
                }

                // normal
                if (this.mGLTFAsset.Material.TextureMap.ContainsKey(PBRTextureType.Normal))
                {
                    var path = this.mGLTFAsset.Material.TextureMap[PBRTextureType.Normal];
                    mtl.SetTexture("NormalTex", TextureManager.Get().GetTexture2D(path));
                }

                // metallic roughness
                if (this.mGLTFAsset.Material.TextureMap.ContainsKey(PBRTextureType.MetallicRoughness))
                {
                    var path = this.mGLTFAsset.Material.TextureMap[PBRTextureType.MetallicRoughness];
                    mtl.SetTexture("MetallicTex", TextureManager.Get().GetTexture2D(path));
                    mtl.SetTexture("RoughnessTex", TextureManager.Get().GetTexture2D(path));
                }

                mDrawable.DrawIndexed();

                mtl.Unbind();
            }
        }

        // bit position 0 => metallicExist;
        // bit position 1 => roghnessExist;
        // bit position 2 => maskExist;
        // bit position 3 => normalExist;
        // bit position 4 => occlusionExist;
        // bit position 5 => opacityExist;
        // bit position 6 => emissiveExist;
        // bit position 7 => ?

        // gltf stores occlusion(R), roughness(G), metallic(B)
        // bit position 8,9 => metallic swizzle index
        // bit position 10,11 => roughness swizzle index
        // bit position 12,13 => occlusion swizzle index

        private uint EncodePBRInfo()
        {
            uint encoded = 0;
            if (mGLTFAsset.Material.TextureMap.ContainsKey(PBRTextureType.MetallicRoughness))
            {
                encoded |= (1);
                encoded |= (1 << 2);

                // metallic index
                encoded |= (1 << 8);
                encoded |= (1 << 9);

                // roughness index
                encoded |= (0 << 10);
                encoded |= (1 << 11);
            }

            if (mGLTFAsset.Material.TextureMap.ContainsKey(PBRTextureType.Occlusion))
            {
                encoded |= (1 << 4);
                // occlusion index
                encoded |= (0 << 12);
                encoded |= (0 << 13);
            }

            if (mGLTFAsset.Material.TextureMap.ContainsKey(PBRTextureType.Normal))
            {
                encoded |= (1 << 3);
            }
            
            return encoded;
        }



        protected override void PrepareRenderingData()
        {
            Debug.Assert(RenderingThread.IsInRenderingThread());

            mDrawable = new GenericMeshDrawable();
            mDrawable.Bind();

            var attrList = mGLTFAsset.VertexAttributeMap
                .OrderBy(x => GetSemanticPriority(x.Key))
                .Select(x => x.Value)
                .ToArray();

            for (int i = 0; i < attrList.Count(); ++i)
            {
                if (attrList[i].attributeType == GLTF.V2.AttributeType.VEC3)
                {
                    var data = mGLTFAsset.Vector3VertexAttributes[attrList[i]].ToArray();
                    mDrawable.SetVertexBufferData(i, data);
                    mGLVertexAttributeTypeList.Add(ActiveAttribType.FloatVec3);
                }
                else if (attrList[i].attributeType == GLTF.V2.AttributeType.VEC2)
                {
                    var data = mGLTFAsset.Vector2VertexAttributes[attrList[i]].ToArray();
                    mDrawable.SetVertexBufferData(i, data);
                    mGLVertexAttributeTypeList.Add(ActiveAttribType.FloatVec2);
                }
                else if (attrList[i].attributeType == GLTF.V2.AttributeType.VEC4)
                {
                    var data = mGLTFAsset.Vector4VertexAttributes[attrList[i]].ToArray();
                    mDrawable.SetVertexBufferData(i, data);
                    mGLVertexAttributeTypeList.Add(ActiveAttribType.FloatVec4);
                }
            }

            if (mGLTFAsset.UIntIndices.Count > 0)
            {
                var arr = mGLTFAsset.UIntIndices.ToArray();
                mDrawable.SetIndexBufferData(arr);
                mIndexCount = (uint)arr.Length;
            }
            else if (mGLTFAsset.UShortIndices.Count > 0)
            {
                var arr = mGLTFAsset.UShortIndices.ToArray();
                mDrawable.SetIndexBufferData(arr);
                mIndexCount = (uint)arr.Length;
            }

            LoadTextures();

            mDrawable.Unbind();

            bReadyToDraw = true;
        }

        private void LoadTextures()
        {
            foreach (var kvp in this.mGLTFAsset.Material.TextureMap)
            {
                if (File.Exists(kvp.Value))
                {
                    TextureManager.Get().CacheTexture2D(kvp.Value);
                }
            }
        }

        private GenericMeshDrawable mDrawable = null;

        protected GLTFMeshAsset mGLTFAsset;

        protected uint mIndexCount = 0;

        protected List<ActiveAttribType> mGLVertexAttributeTypeList = new List<ActiveAttribType>();

        protected CameraTransform mTransformInfo = new CameraTransform();

        protected ModelTransform mModelTransformInfo = new ModelTransform();

        protected MaterialProperty mMaterialProperty = new MaterialProperty();

        protected uint mEncodedPBRInfo = 0;
    }
}
