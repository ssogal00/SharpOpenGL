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
            List<uint> ControlPointIndexList = _ParsedFBXMesh.IndexList;
            List<uint> NewIndexList = new List<uint>();

            List<OpenTK.Vector4> TransformedVertices = new List<Vector4>();

            for(int i = 0; i < ControlPointIndexList.Count; ++i)
            {
                int index = (int)ControlPointIndexList[i];
                List<SkinningInfo> SkinningInfoList = _ParsedFBXMesh.SkinningInfoMap[index];

                OpenTK.Vector4 TransformedPosition = new OpenTK.Vector4();

                for (int j = 0; j < SkinningInfoList.Count; ++j)
                {
                    OpenTK.Vector4 vPosition = new OpenTK.Vector4(_ParsedFBXMesh.ControlPointList[index]);

                    OpenTK.Matrix4 OffsetMatrix = (OpenTK.Matrix4) _ParsedFBXMesh.BoneMap[SkinningInfoList[j].BoneName].Transform;

                    //vPosition = OpenTK.Vector4.Transform(vPosition, (Matrix4)SkinningInfoList[j].OffsetMatrix) * SkinningInfoList[j].Weight;
                    vPosition = OpenTK.Vector4.Transform(vPosition, OffsetMatrix) * SkinningInfoList[j].Weight;

                    TransformedPosition = TransformedPosition + vPosition;
                }

                TransformedVertices.Add(TransformedPosition);                
            }
            
            for (int i = 0; i < TransformedVertices.Count; ++i)
            {
                SharpOpenGL.BasicMaterial.VertexAttribute NewVertex = new SharpOpenGL.BasicMaterial.VertexAttribute();
                NewVertex.VertexPosition = TransformedVertices[i].Xyz;

                Vertices.Add(NewVertex);
                NewIndexList.Add((uint)NewIndexList.Count);                
            }

            var TempVertices = Vertices.ToArray();
            var TempIndices =  NewIndexList.ToArray();
            

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

        ParsedFBXMeshBone RootBone = null;
        TriangleDrawable<SharpOpenGL.BasicMaterial.VertexAttribute> MeshDrawable = null;
        LineDrawable<SharpOpenGL.SimpleMaterial.VertexAttribute> BoneDrawable = null;
    }
}
