using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

using Core;
using FBXWrapper;

using SharpOpenGL;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;

using OpenTK.Graphics.OpenGL;

namespace FBXImporter
{
    public class FBXMesh
    {
        public FBXMesh(FBXWrapper.ParsedFBXMesh _ParsedFBXMesh)
        {                        
            for(int i = 0; i < _ParsedFBXMesh.VertexList.Count; ++i)
            {
                SharpOpenGL.BasicMaterial.VertexAttribute NewVertex = new SharpOpenGL.BasicMaterial.VertexAttribute();
                NewVertex.VertexPosition = _ParsedFBXMesh.VertexList[i];
                NewVertex.TexCoord = _ParsedFBXMesh.UVList[i];

                Vertices.Add(NewVertex);
                VertexIndices.Add((uint)VertexIndices.Count);
            }
        }

        public void PrepareToDraw()
        {
            VB = new Core.Buffer.StaticVertexBuffer<SharpOpenGL.BasicMaterial.VertexAttribute>();
            IB = new Core.Buffer.IndexBuffer();

            var VertexArray = Vertices.ToArray();
            VB.Bind();
            VB.BufferData<SharpOpenGL.BasicMaterial.VertexAttribute>(ref VertexArray);

            var IndexArray = VertexIndices.ToArray();
            IB.Bind();            
            IB.BufferData<uint>(ref IndexArray);
        }

        public void Draw()
        {
            VB.Bind();
            IB.Bind();

            SharpOpenGL.BasicMaterial.VertexAttribute.VertexAttributeBinding();

            GL.DrawElements(PrimitiveType.Triangles, (int)VertexIndices.Count, DrawElementsType.UnsignedInt, 0);
        }
                
        protected List<SharpOpenGL.BasicMaterial.VertexAttribute> Vertices = new List<SharpOpenGL.BasicMaterial.VertexAttribute>();

        Core.Buffer.StaticVertexBuffer<SharpOpenGL.BasicMaterial.VertexAttribute> VB = null;
        Core.Buffer.IndexBuffer IB = null;
        List<uint> VertexIndices = new List<uint>();
    }
}
