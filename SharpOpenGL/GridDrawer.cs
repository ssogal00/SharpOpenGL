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
using SharpOpenGL.GridRenderMaterial;

namespace SharpOpenGL
{
    public class GridDrawer : Singleton<GridDrawer>
    {
        public GridDrawer()
        {
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
            for (float zStart = -halfExtent; zStart <= halfExtent; zStart += gridSize)
            {
                var v1 = new Vector3(halfExtent, 0, zStart);
                var v2 = new Vector3(-halfExtent, 0, zStart);

                vertexList.Add(new P_VertexAttribute(v1)); vertexList.Add(new P_VertexAttribute(v2));
            }

            vertexBuffer.Bind();
            var vertexArray = vertexList.ToArray();
            vertexCount = vertexArray.Length;
            vertexBuffer.BufferData<P_VertexAttribute>(ref vertexArray);
            vertexList.Clear();
        }

        public void Draw(MaterialBase material)
        {   
            if (bInitialized == false)
            {
                SetupVertexBuffer();
                bInitialized = true;
            }

            material.BindAndExecute(vertexBuffer,() =>
            {
                vertexBuffer.BindVertexAttribute();

                cameraTransform.Proj = CameraManager.Get().CurrentCamera.Proj;
                cameraTransform.View = CameraManager.Get().CurrentCamera.View;
                modelTransform.Model = Matrix4.Identity;

                material.SetUniformBufferValue<CameraTransform>("CameraTransform", ref cameraTransform);
                material.SetUniformBufferValue<ModelTransform>("ModelTransform", ref modelTransform);
                material.SetUniformVarData("LineColor", Vector3.UnitX);

                GL.DrawArrays(PrimitiveType.Lines, 0, vertexCount);
            });
        }


        protected bool bInitialized = false;
        protected DynamicVertexBuffer<P_VertexAttribute> vertexBuffer = null;
        protected List<P_VertexAttribute> vertexList = new List<P_VertexAttribute>();
        protected float halfExtent = 10000;
        protected float gridSize = 100;
        protected int vertexCount = 0;

        GridRenderMaterial.ModelTransform modelTransform = new ModelTransform();
        GridRenderMaterial.CameraTransform cameraTransform = new CameraTransform();

    }
}
