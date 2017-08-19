using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core;
using FBXWrapper;

namespace FBXImporter
{
    public class FBXMeshAnimation
    {
        public FBXMeshAnimation(ParsedFBXAnimation _ParsedAnimation, int KeyIndex)
        {
            ParsedAnim = _ParsedAnimation;

            BoneDrawable = new LineDrawable<SharpOpenGL.SimpleMaterial.VertexAttribute>();
            List<SharpOpenGL.SimpleMaterial.VertexAttribute> BoneVertices = new List<SharpOpenGL.SimpleMaterial.VertexAttribute>();
            List<uint> BoneIndices = new List<uint>();

            OpenTK.Vector4 vOrigin = new OpenTK.Vector4(0, 0, 0, 1);

            Dictionary<String, OpenTK.Matrix4> TransformMap = new Dictionary<string, OpenTK.Matrix4>();

            OpenTK.Matrix4 RootMatrix = ParsedAnim.GetTransform(ParsedAnim.Mesh.RootBone.BoneName, KeyIndex);

            TransformMap.Add(ParsedAnim.Mesh.RootBone.BoneName, RootMatrix);

            for (BoneIterator It = new BoneIterator(ParsedAnim.Mesh.RootBone); !It.IsEnd(); It.MoveNext())
            {
                OpenTK.Matrix4 ParentTransform = OpenTK.Matrix4.Identity;

                if(TransformMap.ContainsKey(It.Current().BoneName))
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

        public void DrawBoneHierarchy()
        {
            if (BoneDrawable != null)
            {
                BoneDrawable.Draw();
            }
        }


        ParsedFBXAnimation ParsedAnim = null;
        LineDrawable<SharpOpenGL.SimpleMaterial.VertexAttribute> BoneDrawable = null;
    }
}
