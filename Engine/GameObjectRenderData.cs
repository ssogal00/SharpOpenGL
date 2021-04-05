using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;
using System.Windows.Media.Media3D;
using Core.Buffer;
using Core.Texture;
using GLTF;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;

namespace Engine
{
    public enum EVertexStructure
    {
        SOA,
        AOS,
    }

    public class GameObjectRenderData
    {
        public GameObjectRenderData()
        {

        }

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

        public Dictionary<string, TextureBase> GetTextureParams()
        {
            return null;
        }

        public bool IsVertexDataSOA = false;

        public bool HasIndexData = false;
    }

    public class CubeRenderData : GameObjectRenderData
    {
        public override List<T> GetVertexAttributeData<T>(int index)
        {
            return null;
        }
    }

    public class SphereRenderData : GameObjectRenderData
    {
        
    }
}
