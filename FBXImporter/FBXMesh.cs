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

            BoneDrawable = new LineDrawable<SharpOpenGL.SimpleMaterial.VertexAttribute>();
            List<SharpOpenGL.SimpleMaterial.VertexAttribute> BoneVertices = new List<SharpOpenGL.SimpleMaterial.VertexAttribute>();
            List<uint> BoneIndices = new List<uint>();
                        
            OpenTK.Vector4 vOrigin = new OpenTK.Vector4(0, 0, 0, 1);

            for (BoneIterator It = new BoneIterator(_ParsedFBXMesh.RootBone); !It.IsEnd(); It.MoveNext())
            {
                OpenTK.Matrix4 TransformMatrix = (OpenTK.Matrix4)It.Current().LinkTransform;
                OpenTK.Vector4 vStart = OpenTK.Vector4.Transform(vOrigin, TransformMatrix);                
                
                for(int i =0; i < It.Current().ChildBoneList.Count; ++i)
                {                    
                    OpenTK.Matrix4 ChildTransform = (OpenTK.Matrix4) It.Current().ChildBoneList[i].LinkTransform;
                    OpenTK.Vector4 vEnd = OpenTK.Vector4.Transform(vOrigin, ChildTransform);

                    SharpOpenGL.SimpleMaterial.VertexAttribute NewVertex1;
                    NewVertex1.VertexPosition = vStart.Xyz;

                    SharpOpenGL.SimpleMaterial.VertexAttribute NewVertex2;
                    NewVertex2.VertexPosition = vEnd.Xyz;

                    BoneVertices.Add(NewVertex1);
                    BoneIndices.Add((uint)BoneIndices.Count);

                    BoneVertices.Add(NewVertex2);
                    BoneIndices.Add((uint)BoneIndices.Count);
                }
            }

            var BoneVertexArray = BoneVertices.ToArray();
            var BoneIndexArray = BoneIndices.ToArray();

            BoneDrawable.SetupData(ref BoneVertexArray, ref BoneIndexArray);
        }
        
        public void Draw()
        {            
            if(MeshDrawable != null)
            {
                MeshDrawable.Draw();
            }
        }

        public void DrawBoneHierarchy()
        {
            if(BoneDrawable != null)
            {
                BoneDrawable.Draw();
            }
        }

        FBXMeshBone RootBone = null;
        TriangleDrawable<SharpOpenGL.BasicMaterial.VertexAttribute> MeshDrawable = null;
        LineDrawable<SharpOpenGL.SimpleMaterial.VertexAttribute> BoneDrawable = null;
    }
}
