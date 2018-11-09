using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Buffer;
using Core.MaterialBase;
using Core.Primitive;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using SharpOpenGL.Asset;

namespace SharpOpenGL
{
    public class GridDrawer : Singleton<GridDrawer>
    {
        public GridDrawer()
        {
            Debug.Assert(OpenGLContext.Get().IsValid);
            SetupVertexBuffer();
        }

        protected void SetupVertexBuffer()
        {
            vertexBuffer = new DynamicVertexBuffer<P_VertexAttribute>();
            // grid x
            for (float xStart = -halfExtent; xStart <= halfExtent; xStart += gridSize)
            {
                var v1 = new Vector3(xStart, 0, halfExtent);
                var v2 = new Vector3(xStart, 0, -halfExtent);

                vertexList.Add(new P_VertexAttribute(v1)); vertexList.Add(new P_VertexAttribute(v2));
            }
            
            // grid y
            for (float yStart = -halfExtent; yStart <= halfExtent; yStart += gridSize)
            {
                var v1 = new Vector3(yStart, 0, halfExtent);
                var v2 = new Vector3(yStart, 0, -halfExtent);

                vertexList.Add(new P_VertexAttribute(v1)); vertexList.Add(new P_VertexAttribute(v2));
            }

            vertexBuffer.Bind();
            var vertexArray = vertexList.ToArray();
            vertexBuffer.BufferData<P_VertexAttribute>(ref vertexArray);
        }

        public void Draw(MaterialBase material)
        {
            
            material.BindAndExecute(vertexBuffer,() =>
            {
                vertexBuffer.BindVertexAttribute();
                GL.DrawArrays(PrimitiveType.Lines, 0, vertexList.Count);
            });
        }

        protected DynamicVertexBuffer<P_VertexAttribute> vertexBuffer = null;
        protected List<P_VertexAttribute> vertexList = new List<P_VertexAttribute>();
        protected float halfExtent = 1000;
        protected float gridSize = 10;
    }
}
