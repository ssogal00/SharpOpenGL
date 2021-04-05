using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;
using System.Windows.Media.Media3D;
using Core.Buffer;
using GLTF;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;

namespace Engine
{

    public class GameObjectRenderData
    {
        public virtual List<uint> GetIndexData()
        {
            return null;
        }

        // when Array of Structure
        public virtual List<T> GetVertexData<T>()
        {
            return null;
        }

        // when Strucure of array
        public virtual List<T> GetVertexAttributeData<T>(int index)
        {
            return null;
        }

        public Material GetMaterial()
        {
            return null;
        }

        // shader param
        public Dictionary<string, float> GetFloatParams()
        {
            return null;
        }

        public Dictionary<string, Matrix4> GetMatrix4Params()
        {
            return null;
        }

        public Dictionary<string, Vector3> GetVector3Params()
        {
            return null;
        }

        public bool IsVertexDataSOA = false;
    }

    public class CubeRenderData : GameObjectRenderData
    {
        public override List<T> GetVertexAttributeData<T>(int index)
        {
            return null;
        }
    }
}
