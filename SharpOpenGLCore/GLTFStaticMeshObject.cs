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

namespace SharpOpenGLCore
{
    public class GLTFStaticMeshObject : GameObject
    {
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
                mDrawable.Draw(0, mIndexCount);
                mtl.Unbind();
            }
        }

        protected override void PrepareRenderingData()
        {
            Debug.Assert(RenderingThread.IsInRenderingThread());

            mDrawable = new GenericMeshDrawable();
            mDrawable.Bind();

            var attrList = mGLTFAsset.VertexAttributeList.OrderBy(x => x.index);
            
            foreach (var attr in attrList)
            {
                if (attr.attributeType == GLTF.V2.AttributeType.VEC3)
                {
                    var data= mGLTFAsset.Vector3VertexAttributes[attr].ToArray();
                    mDrawable.SetVertexBufferData(attr.index - 1, ref data);
                }
                else if (attr.attributeType == GLTF.V2.AttributeType.VEC2)
                {
                    var data = mGLTFAsset.Vector2VertexAttributes[attr].ToArray();
                    mDrawable.SetVertexBufferData(attr.index - 1, ref data);
                }
                else if (attr.attributeType == GLTF.V2.AttributeType.VEC4)
                {
                    var data = mGLTFAsset.Vector4VertexAttributes[attr].ToArray();
                    mDrawable.SetVertexBufferData(attr.index - 1, ref data);
                }
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
