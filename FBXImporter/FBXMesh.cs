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

using Core;

using OpenTK.Graphics.OpenGL;
using Core.Primitive;

namespace FBXImporter
{
    public class FBXMesh
    {
        public FBXMesh()
        {            
        }

        public void SetFBXMeshInfo(FBXWrapper.ParsedFBXMesh _ParsedFBXMesh)
        {
            RootBone = _ParsedFBXMesh.RootBone;

            List<SharpOpenGL.BasicMaterial.VertexAttribute> Vertices = new List<SharpOpenGL.BasicMaterial.VertexAttribute>();
            List<uint> VertexIndices = new List<uint>();

            for (int i = 0; i < _ParsedFBXMesh.VertexList.Count; ++i)
            {
                SharpOpenGL.BasicMaterial.VertexAttribute NewVertex = new SharpOpenGL.BasicMaterial.VertexAttribute();
                NewVertex.VertexPosition = _ParsedFBXMesh.VertexList[i];
                NewVertex.TexCoord = _ParsedFBXMesh.UVList[i];

                Vertices.Add(NewVertex);
                VertexIndices.Add((uint)VertexIndices.Count);
            }

            var TempVertices = Vertices.ToArray();
            var TempIndices = VertexIndices.ToArray();

            MeshDrawable = new TriangleDrawable<SharpOpenGL.BasicMaterial.VertexAttribute>();
            MeshDrawable.SetupData(ref TempVertices, ref TempIndices);
        }
        
        public void Draw()
        {
            MeshDrawable.Draw();
        }

        FBXMeshBone RootBone = null;
        TriangleDrawable<SharpOpenGL.BasicMaterial.VertexAttribute> MeshDrawable = null;
    }
}
