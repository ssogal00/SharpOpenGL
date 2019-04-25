using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core;
using Core.Primitive;
using FBXWrapper;
using OpenTK;
using CompiledMaterial;

namespace FBXImporter
{
    public class FBXMeshAnimation
    {
        public FBXMeshAnimation(ParsedFBXAnimation _ParsedAnimation, int KeyIndex)
        {
            ParsedAnim = _ParsedAnimation;

            BoneDrawable = new LineDrawable<P_VertexAttribute>();
            List<CompiledMaterial.SimpleMaterial.VertexAttribute> BoneVertices = new List<CompiledMaterial.SimpleMaterial.VertexAttribute>();
            List<uint> BoneIndices = new List<uint>();

            OpenTK.Vector4 vOrigin = new OpenTK.Vector4(0, 0, 0, 1);

            Dictionary<String, OpenTK.Matrix4> TransformMap = new Dictionary<string, OpenTK.Matrix4>();
            Dictionary<String, OpenTK.Matrix4> AnimBoneTransformMap = new Dictionary<string, Matrix4>();

            OpenTK.Matrix4 RootMatrix = ParsedAnim.GetTransform(ParsedAnim.Mesh.RootBone.BoneName, KeyIndex);

            TransformMap.Add(ParsedAnim.Mesh.RootBone.BoneName, RootMatrix);
            AnimBoneTransformMap.Add(ParsedAnim.Mesh.RootBone.BoneName, RootMatrix);

            for (BoneIterator It = new BoneIterator(ParsedAnim.Mesh.RootBone); !It.IsEnd(); It.MoveNext())
            {
                OpenTK.Matrix4 ParentTransform = OpenTK.Matrix4.Identity;

                if (TransformMap.ContainsKey(It.Current().BoneName))
                {
                    ParentTransform = TransformMap[It.Current().BoneName];
                }
                else
                {
                    Console.Write("Parent Bone not Found");
                }

                OpenTK.Vector4 vStart = OpenTK.Vector4.Transform(vOrigin, ParentTransform);

                for (int i = 0; i < It.Current().ChildBoneList.Count; ++i)
                {
                    OpenTK.Matrix4 ChildTransform = ParsedAnim.GetTransform(It.Current().ChildBoneList[i].BoneName, KeyIndex);
                    OpenTK.Matrix4 CurrentTrasnform = ChildTransform * ParentTransform;

                    TransformMap.Add(It.Current().ChildBoneList[i].BoneName, CurrentTrasnform);                    

                    OpenTK.Vector4 vEnd = OpenTK.Vector4.Transform(vOrigin, CurrentTrasnform);

                    CompiledMaterial.SimpleMaterial.VertexAttribute NewVertex1;
                    NewVertex1.VertexPosition = vStart.Xyz;

                    CompiledMaterial.SimpleMaterial.VertexAttribute NewVertex2;
                    NewVertex2.VertexPosition = vEnd.Xyz;

                    BoneVertices.Add(NewVertex1);
                    BoneIndices.Add((uint)BoneIndices.Count);

                    BoneVertices.Add(NewVertex2);
                    BoneIndices.Add((uint)BoneIndices.Count);
                }
            }

            var BoneVertexArray = BoneVertices.ToArray();
            var BoneIndexArray = BoneIndices.ToArray();

            //BoneDrawable.SetupData(ref BoneVertexArray, ref BoneIndexArray);

            MeshDrawable = new TriangleDrawable<P_VertexAttribute>();

            List<CompiledMaterial.BasicMaterial.VertexAttribute> Vertices = new List<CompiledMaterial.BasicMaterial.VertexAttribute>();
            List<uint> ControlPointIndexList = _ParsedAnimation.Mesh.IndexList;
            List<uint> NewIndexList = new List<uint>();

            List<OpenTK.Vector4> TransformedVertices = new List<Vector4>();

            for (int i = 0; i < ControlPointIndexList.Count; ++i)
            {
                int index = (int)ControlPointIndexList[i];
                List<SkinningInfo> SkinningInfoList = _ParsedAnimation.Mesh.SkinningInfoMap[index];

                OpenTK.Vector4 TransformedPosition = new OpenTK.Vector4();

                for (int j = 0; j < SkinningInfoList.Count; ++j)
                {
                    OpenTK.Vector4 vPosition = new OpenTK.Vector4(_ParsedAnimation.Mesh.ControlPointList[index]);

                    OpenTK.Matrix4 OffsetMatrix = (OpenTK.Matrix4) TransformMap[SkinningInfoList[j].BoneName];

                    //vPosition = OpenTK.Vector4.Transform(vPosition, (Matrix4)SkinningInfoList[j].OffsetMatrix) * SkinningInfoList[j].Weight;
                    vPosition = OpenTK.Vector4.Transform(vPosition, OffsetMatrix) * SkinningInfoList[j].Weight;

                    TransformedPosition = TransformedPosition + vPosition;
                }

                TransformedVertices.Add(TransformedPosition);
            }

            //for (int i = 0; i < _ParsedFBXMesh.ControlPointList.Count; ++i)
            for (int i = 0; i < TransformedVertices.Count; ++i)
            {
                CompiledMaterial.BasicMaterial.VertexAttribute NewVertex = new CompiledMaterial.BasicMaterial.VertexAttribute();
                NewVertex.VertexPosition = TransformedVertices[i].Xyz;
                //NewVertex.TexCoord = _ParsedFBXMesh.UVList[i];

                Vertices.Add(NewVertex);
                NewIndexList.Add((uint)NewIndexList.Count);
                //ControlPointIndexList.Add((uint)ControlPointIndexList.Count);
            }

            var TempVertices = Vertices.ToArray();
            var TempIndices = NewIndexList.ToArray();


            MeshDrawable = new TriangleDrawable<P_VertexAttribute>();
            //MeshDrawable.SetupData(ref TempVertices, ref TempIndices);                       
        }

        public void DrawBoneHierarchy()
        {
            if (BoneDrawable != null)
            {
                BoneDrawable.Draw();
            }

            if(MeshDrawable != null)
            {
                //MeshDrawable.Draw();
            }
        }


        ParsedFBXAnimation ParsedAnim = null;
        TriangleDrawable<P_VertexAttribute> MeshDrawable = null;
        LineDrawable<P_VertexAttribute> BoneDrawable = null;
    }
}
