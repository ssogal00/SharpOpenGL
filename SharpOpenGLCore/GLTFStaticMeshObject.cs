using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CompiledMaterial.GBufferPNT;
using Core;
using Core.Buffer;
using Core.Primitive;
using GLTF;
using GLTF.V2;
using OpenTK.Mathematics;
using SharpOpenGL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Threading;

namespace SharpOpenGLCore
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


        public GLTFStaticMeshObject(GLTFMeshAsset asset)
        {
            mGLTFAsset = asset;
            RenderingThread.Get().ExecuteImmediatelyIfRenderingThread(() =>
            {
                PrepareRenderingData();
            });
        }

        public override void Render()
        {
            if (bReadyToDraw)
            {
                var mtl = ShaderManager.Get().GetMaterial<GBufferPNT>();

                mtl.Bind();
                mtl.CameraTransform_View = CameraManager.Get().CurrentCameraView;
                mtl.CameraTransform_Proj = CameraManager.Get().CurrentCameraProj;
                mtl.ModelTransform_Model = this.LocalMatrix;
                mDrawable.DrawIndexed();
                mtl.Unbind();
            }
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
                    mDrawable.SetVertexBufferData(i, ref data);
                }
                else if (attrList[i].attributeType == GLTF.V2.AttributeType.VEC2)
                {
                    var data = mGLTFAsset.Vector2VertexAttributes[attrList[i]].ToArray();
                    mDrawable.SetVertexBufferData(i, ref data);
                }
                else if (attrList[i].attributeType == GLTF.V2.AttributeType.VEC4)
                {
                    var data = mGLTFAsset.Vector4VertexAttributes[attrList[i]].ToArray();
                    mDrawable.SetVertexBufferData(i, ref data);
                }
            }


            foreach (var attr in attrList)
            {
                
            }

            if (mGLTFAsset.UIntIndices.Count > 0)
            {
                var arr = mGLTFAsset.UIntIndices.ToArray();
                mDrawable.SetIndexBufferData(ref arr);
                mIndexCount = (uint)arr.Length;
            }
            else if (mGLTFAsset.UShortIndices.Count > 0)
            {
                var arr = mGLTFAsset.UShortIndices.ToArray();
                mDrawable.SetIndexBufferData(ref arr);
                mIndexCount = (uint)arr.Length;
            }

            mDrawable.Unbind();

            bReadyToDraw = true;
        }

        private GenericMeshDrawable mDrawable = null;

        protected GLTFMeshAsset mGLTFAsset;

        protected uint mIndexCount = 0;
    }
}
