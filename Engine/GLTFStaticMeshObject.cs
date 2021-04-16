using Core.Primitive;
using Engine.Rendering;
using GLTF;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Engine
{
    public class GLTFStaticMeshObject : GameObject
    {
        public GLTFStaticMeshObject(List<GLTFMeshAsset> assetList)
        {
            MaterialName = "GBufferMacro2";
            mGLTFAssetList = assetList;
            foreach (var asset in assetList)
            {
                var section = new MeshSection(MaterialName, asset.VertexAttributeMap,
                    asset.Vector2VertexAttributes, asset.Vector3VertexAttributes,
                    asset.Vector4VertexAttributes, asset.UIntIndices,asset.UShortIndices);

                mMeshSectionList.Add(section);
            }
        }

        public GLTFStaticMeshObject(string mtlName, List<GLTFMeshAsset> assetList)
        {
            MaterialName = mtlName;
            mGLTFAssetList = assetList;
            foreach (var asset in assetList)
            {
                var section = new MeshSection(MaterialName, asset.VertexAttributeMap,
                    asset.Vector2VertexAttributes, asset.Vector3VertexAttributes,
                    asset.Vector4VertexAttributes, asset.UIntIndices, asset.UShortIndices);

                mMeshSectionList.Add(section);
            }
        }

        public GLTFStaticMeshObject(GLTFMeshAsset asset)
        {
            mGLTFAsset = asset;

            MaterialName = "GBufferMacro2";

            var section = new MeshSection(MaterialName, asset.VertexAttributeMap,
                asset.Vector2VertexAttributes, asset.Vector3VertexAttributes,
                asset.Vector4VertexAttributes, asset.UIntIndices, asset.UShortIndices);

            mMeshSectionList.Add(section);
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

        public override IEnumerable<(string, Matrix4)> GetMatrix4Params(int sectionIndex)
        {
            yield return ("View", CameraManager.Instance.CurrentCameraView);
            yield return ("Proj", CameraManager.Instance.CurrentCameraProj);
            yield return ("Model", this.LocalMatrix);
        }

        public override IEnumerable<(string, float)> GetFloatParams(int sectionIndex)
        {
            yield return ("Metallic", 0.5f);
            yield return ("Roughness", 0.5f);
        }

        public override IEnumerable<(string, bool)> GetBoolParams(int sectionIndex)
        {
            yield return ("MetallicExist", true);

            yield return ("NormalExist", true);

            yield return ("MaskExist", false);

            yield return ("MetallicRoughnessOneTexture", true);

            yield return ("RoghnessExist", true);
        }

        public override IEnumerable<(string, string)> GetTextureParams(int index)
        {
            Debug.Assert(index < mGLTFAssetList.Count);

            if (mGLTFAssetList[index].Material.TextureMap.ContainsKey(PBRTextureType.BaseColor))
            {
                var path = mGLTFAssetList[index].Material.TextureMap[PBRTextureType.BaseColor];
                yield return ("DiffuseTex", path);
            }

            if (mGLTFAssetList[index].Material.TextureMap.ContainsKey(PBRTextureType.Normal))
            {
                var path = mGLTFAssetList[index].Material.TextureMap[PBRTextureType.Normal];
                yield return ("NormalTex", path);
            }

            if (mGLTFAssetList[index].Material.TextureMap.ContainsKey(PBRTextureType.MetallicRoughness))
            {
                var path = mGLTFAssetList[index].Material.TextureMap[PBRTextureType.MetallicRoughness];
                yield return ("MetallicTex", path);
                yield return ("RoughnessTex", path);
                yield return ("MetallicRoughnessTex", path);
            }
        }

        protected GLTFMeshAsset mGLTFAsset;

        protected List<GLTFMeshAsset> mGLTFAssetList = new List<GLTFMeshAsset>();

        protected uint mEncodedPBRInfo = 0;
    }
}
