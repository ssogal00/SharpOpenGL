using Core;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using Core.Primitive;
using FBXWrapper;
using SharpOpenGL.SimpleMaterial;

namespace FBXImporter
{
    public class FBXMesh
    {
        public FBXMesh()
        {            
        }

        public void SetFBXMeshInfo(FBXWrapper.ParsedFBXMesh parsedFBXMesh)
        {
            MinVertex = (OpenTK.Vector3) parsedFBXMesh.MinVertex;
            MaxVertex = (OpenTK.Vector3) parsedFBXMesh.MaxVertex;

            RootBone = parsedFBXMesh.RootBone;

            List<SharpOpenGL.BasicMaterial.VertexAttribute> Vertices = new List<SharpOpenGL.BasicMaterial.VertexAttribute>();
            List<uint> ControlPointIndexList = parsedFBXMesh.IndexList;
            List<uint> NewIndexList = new List<uint>();

            List<OpenTK.Vector4> TransformedVertices = new List<Vector4>();

            for(int i = 0; i < ControlPointIndexList.Count; ++i)
            {
                int index = (int)ControlPointIndexList[i];
                List<SkinningInfo> SkinningInfoList = parsedFBXMesh.SkinningInfoMap[index];

                OpenTK.Vector4 TransformedPosition = new OpenTK.Vector4();

                for (int j = 0; j < SkinningInfoList.Count; ++j)
                {
                    OpenTK.Vector4 vPosition = new OpenTK.Vector4(parsedFBXMesh.ControlPointList[index]);

                    OpenTK.Matrix4 OffsetMatrix = (OpenTK.Matrix4)parsedFBXMesh.BoneMap[SkinningInfoList[j].BoneName].Transform;

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
            

            MeshDrawable = new TriangleDrawable<P_VertexAttribute>();
            MeshDrawable.SetupData(ref TempVertices, ref TempIndices);

            BoneDrawable = new LineDrawable<P_VertexAttribute>();
            var BoneVertices = new List<P_VertexAttribute>();
            List<uint> BoneIndices = new List<uint>();
                        
            OpenTK.Vector4 vOrigin = new OpenTK.Vector4(0, 0, 0, 1);

            for (BoneIterator It = new BoneIterator(parsedFBXMesh.RootBone); !It.IsEnd(); It.MoveNext())
            {
                OpenTK.Matrix4 TransformMatrix = (OpenTK.Matrix4)It.Current().LinkTransform;
                OpenTK.Vector4 vStart = OpenTK.Vector4.Transform(vOrigin, TransformMatrix);                
                
                for(int i =0; i < It.Current().ChildBoneList.Count; ++i)
                {                    
                    OpenTK.Matrix4 ChildTransform = (OpenTK.Matrix4) It.Current().ChildBoneList[i].LinkTransform;
                    OpenTK.Vector4 vEnd = OpenTK.Vector4.Transform(vOrigin, ChildTransform);

                    BoneVertices.Add(vStart);
                    BoneIndices.Add((uint)BoneIndices.Count);

                    BoneVertices.Add(vEnd);
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
        TriangleDrawable<P_VertexAttribute> MeshDrawable = null;
        LineDrawable<P_VertexAttribute> BoneDrawable = null;
        public OpenTK.Vector3 MinVertex = new OpenTK.Vector3();
        public OpenTK.Vector3 MaxVertex = new OpenTK.Vector3();
    }
}
