using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace SharpOpenGLCore
{
    public class StaticMeshHelper
    {
        public static List<Vector4> GenerateTangents(List<Vector3> vertexList, List<Vector2> texcoordList, List<Vector3> normalList, List<uint> indexList)
        {
            List<Vector4> result = new List<Vector4>();
            List<Vector3> tan1Accum = new List<Vector3>();
            List<Vector3> tan2Accum = new List<Vector3>();

            for (uint i = 0; i < vertexList.Count; ++i)
            {
                tan1Accum.Add(new Vector3(0, 0, 0));
                tan2Accum.Add(new Vector3(0, 0, 0));
            }

            for (uint i = 0; i < indexList.Count; i++)
            {
                result.Add(new Vector4(0, 0, 0, 0));
            }

            // Compute the tangent vector
            for (uint i = 0; i < indexList.Count; i += 3)
            {
                var p1 = vertexList[(int)indexList[(int)i]];
                var p2 = vertexList[(int)indexList[(int)i + 1]];
                var p3 = vertexList[(int)indexList[(int)i + 2]];

                var tc1 = texcoordList[(int)indexList[(int)i]];
                var tc2 = texcoordList[(int)indexList[(int)i + 1]];
                var tc3 = texcoordList[(int)indexList[(int)i + 2]];

                Vector3 q1 = p2 - p1;
                Vector3 q2 = p3 - p1;
                float s1 = tc2.X - tc1.X, s2 = tc3.X - tc1.X;
                float t1 = tc2.Y - tc1.Y, t2 = tc3.Y - tc1.Y;

                // prevent degeneration
                float r = 1.0f / (s1 * t2 - s2 * t1);
                if (Single.IsInfinity(r))
                {
                    r = 1 / 0.1f;
                }

                var tan1 = new Vector3((t2 * q1.X - t1 * q2.X) * r,
                   (t2 * q1.Y - t1 * q2.Y) * r,
                   (t2 * q1.Z - t1 * q2.Z) * r);

                var tan2 = new Vector3((s1 * q2.X - s2 * q1.X) * r,
                   (s1 * q2.Y - s2 * q1.Y) * r,
                   (s1 * q2.Z - s2 * q1.Z) * r);


                tan1Accum[(int)indexList[(int)i]] += tan1;
                tan1Accum[(int)indexList[(int)i + 1]] += tan1;
                tan1Accum[(int)indexList[(int)i + 2]] += tan1;

                tan2Accum[(int)indexList[(int)i]] += tan2;
                tan2Accum[(int)indexList[(int)i + 1]] += tan2;
                tan2Accum[(int)indexList[(int)i + 2]] += tan2;
            }

            Vector4 lastValidTangent = new Vector4();

            for (uint i = 0; i < indexList.Count; ++i)
            {
                var n = normalList[(int)indexList[(int)i]];
                var t1 = tan1Accum[(int)indexList[(int)i]];
                var t2 = tan2Accum[(int)indexList[(int)i]];

                // Gram-Schmidt orthogonalize                
                var temp = Vector3.Normalize(t1 - (Vector3.Dot(n, t1) * n));
                // Store handedness in w                
                var W = (Vector3.Dot(Vector3.Cross(n, t1), t2) < 0.0f) ? -1.0f : 1.0f;

                bool bValid = true;
                if (Single.IsNaN(temp.X) || Single.IsNaN(temp.Y) || Single.IsNaN(temp.Z))
                {
                    bValid = false;
                }

                if (Single.IsInfinity(temp.X) || Single.IsInfinity(temp.Y) || Single.IsInfinity(temp.Z))
                {
                    bValid = false;
                }

                if (bValid == true)
                {
                    lastValidTangent = new Vector4(temp.X, temp.Y, temp.Z, W);
                }

                if (bValid == false)
                {
                    temp = lastValidTangent.Xyz;
                }

                result[(int)i] = new Vector4(temp.X, temp.Y, temp.Z, W);
            }

            tan1Accum.Clear();
            tan2Accum.Clear();

            return result;
        }
    }
}
