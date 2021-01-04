using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public GLTFStaticMeshObject(GLTFMesh asset)
        {
            mGLTFAsset = asset;
        }

        public override void Render()
        {
            mVertexArray.Bind();
            
            // GL.DrawArrays(PrimitiveType.Triangles,0, );
            mVertexArray.Unbind();
        }

        protected override void PrepareRenderingData()
        {
            Debug.Assert(RenderingThread.IsInRenderingThread());

            mVertexArray.Bind();
            
            var attrList = mGLTFAsset.VertexAttributeList.OrderBy(x => x.index);
            
            foreach (var attr in attrList)
            {
                if (attr.attributeType == GLTF.V2.AttributeType.VEC3)
                {
                    var vb = new SOAVertexBuffer<Vec3_VertexAttribute>();
                    var data= mGLTFAsset.Vector3VertexAttributes[attr].ToArray();
                    vb.BindAtIndex(attr.index);
                    vb.BufferData<Vector3>(ref data);
                }
                else if (attr.attributeType == GLTF.V2.AttributeType.VEC2)
                {
                    var vb = new SOAVertexBuffer<Vec2_VertexAttribute>();
                    var data = mGLTFAsset.Vector2VertexAttributes[attr].ToArray();
                    vb.BindAtIndex(attr.index);
                    vb.BufferData<Vector2>(ref data);
                }
                else if (attr.attributeType == GLTF.V2.AttributeType.VEC4)
                {
                    var vb = new SOAVertexBuffer<Vec4_VertexAttribute>();
                    var data = mGLTFAsset.Vector4VertexAttributes[attr].ToArray();
                    vb.BindAtIndex(attr.index);
                    vb.BufferData<Vector4>(ref data);
                }
            }

            mIndexBuffer.Bind();
            var indexArray = mGLTFAsset.UIntIndices.ToArray();
            mIndexBuffer.BufferData(ref indexArray);
            mVertexArray.Unbind();
        }

        protected Dictionary<int, SOAVertexBuffer<Vec3_VertexAttribute>> mVec3Attributes;

        protected IndexBuffer mIndexBuffer;

        protected VertexArray mVertexArray;

        protected GLTFMesh mGLTFAsset;
    }
}
