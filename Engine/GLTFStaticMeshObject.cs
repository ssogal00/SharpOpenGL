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
using System.Windows.Xps.Serialization;
using OpenTK.Mathematics;
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

            else if (semanticName.StartsWith("TEXCOORD", true, null))
            {
                return 2;
            }

            else if (semanticName.StartsWith("TANGENT", true, null))
            {
                return 3;
            }

            return 0;
        }


        public GLTFStaticMeshObject(GLTFMeshAsset asset)
        {
            mGLTFAsset = asset;

            MaterialName = "GBufferMacro1";

            mVertexAttributeMap = mGLTFAsset.VertexAttributeMap;
            mVector2VertexAttributes = mGLTFAsset.Vector2VertexAttributes;
            mVector3VertexAttributes = mGLTFAsset.Vector3VertexAttributes;
            mVector4VertexAttributes = mGLTFAsset.Vector4VertexAttributes;
            mUIntIndices = mGLTFAsset.UIntIndices;
            mUShortIndices = mGLTFAsset.UShortIndices;
        }

        public override void Render()
        {
            if (bReadyToDraw)
            {
                var mtl = ShaderManager.Instance.GetMaterial("GBufferMacro1");

                mtl.Bind();

                mTransformInfo.Proj = CameraManager.Instance.CurrentCameraProj;
                mTransformInfo.View = CameraManager.Instance.CurrentCameraView;
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
                    mtl.SetTexture("DiffuseTex", TextureManager.Instance.GetTexture2D(path));
                }

                // normal
                if (this.mGLTFAsset.Material.TextureMap.ContainsKey(PBRTextureType.Normal))
                {
                    var path = this.mGLTFAsset.Material.TextureMap[PBRTextureType.Normal];
                    mtl.SetTexture("NormalTex", TextureManager.Instance.GetTexture2D(path));
                }

                // metallic roughness
                if (this.mGLTFAsset.Material.TextureMap.ContainsKey(PBRTextureType.MetallicRoughness))
                {
                    var path = this.mGLTFAsset.Material.TextureMap[PBRTextureType.MetallicRoughness];
                    mtl.SetTexture("MetallicTex", TextureManager.Instance.GetTexture2D(path));
                    mtl.SetTexture("RoughnessTex", TextureManager.Instance.GetTexture2D(path));
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
                if (attrList[i].AttributeType == GLTF.V2.AttributeType.VEC3)
                {
                    var data = mGLTFAsset.Vector3VertexAttributes[attrList[i]].ToArray();
                    mDrawable.SetVertexBufferData(i, data);
                    mGLVertexAttributeTypeList.Add(ActiveAttribType.FloatVec3);
                }
                else if (attrList[i].AttributeType == GLTF.V2.AttributeType.VEC2)
                {
                    var data = mGLTFAsset.Vector2VertexAttributes[attrList[i]].ToArray();
                    mDrawable.SetVertexBufferData(i, data);
                    mGLVertexAttributeTypeList.Add(ActiveAttribType.FloatVec2);
                }
                else if (attrList[i].AttributeType == GLTF.V2.AttributeType.VEC4)
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
                    TextureManager.Instance.CacheTexture2D(kvp.Value);
                }
            }
        }

        public override IEnumerable<(string, Matrix4)> GetMatrix4Params()
        {
            yield return ("View", CameraManager.Instance.CurrentCameraView);
            yield return ("Proj", CameraManager.Instance.CurrentCameraProj);
            yield return ("Model", this.LocalMatrix);
        }

        public override IEnumerable<(string, bool)> GetBoolParams()
        {
            yield return ("MetallicExist", true);

            yield return ("NormalExist", true);

            yield return ("MaskExist", false);

            yield return ("MetallicRoughnessOneTexture", true);

            yield return ("RoghnessExist", true);
        }

        public override IEnumerable<(string, string)> GetTextureParams()
        {
            // base 
            if (this.mGLTFAsset.Material.TextureMap.ContainsKey(PBRTextureType.BaseColor))
            {
                var path = this.mGLTFAsset.Material.TextureMap[PBRTextureType.BaseColor];
                yield return ("DiffuseTex", path);
            }

            // normal
            if (this.mGLTFAsset.Material.TextureMap.ContainsKey(PBRTextureType.Normal))
            {
                var path = this.mGLTFAsset.Material.TextureMap[PBRTextureType.Normal];
                yield return ("NormalTex", path);
            }

            // metallic roughness
            if (this.mGLTFAsset.Material.TextureMap.ContainsKey(PBRTextureType.MetallicRoughness))
            {
                var path = this.mGLTFAsset.Material.TextureMap[PBRTextureType.MetallicRoughness];
                yield return ("MetallicTex", path);
                yield return ("RoughnessTex", path);
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
