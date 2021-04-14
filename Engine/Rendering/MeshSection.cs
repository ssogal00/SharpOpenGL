using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLTF;
using OpenTK.Mathematics;

namespace Engine.Rendering
{
    public class MeshSection
    {
        public string MaterialName = string.Empty;
        public List<uint> UIntIndices => mUIntIndices;

        public List<ushort> UShortIndices => mUShortIndices;

        public Dictionary<string, VertexAttributeSemantic> VertexAttributeMap
        {
            get => mVertexAttributeMap;
        }

        public Dictionary<VertexAttributeSemantic, List<float>> FloatVertexAttributes
        {
            get => mFloatVertexAttributes;
        }
        public Dictionary<VertexAttributeSemantic, List<Vector2>> Vector2VertexAttributes
        {
            get => mVector2VertexAttributes;
        }
        public Dictionary<VertexAttributeSemantic, List<Vector3>> Vector3VertexAttributes
        {
            get => mVector3VertexAttributes;
        }
        public Dictionary<VertexAttributeSemantic, List<Vector4>> Vector4VertexAttributes
        {
            get => mVector4VertexAttributes;
        }
        

        protected Dictionary<string, VertexAttributeSemantic> mVertexAttributeMap = new Dictionary<string, VertexAttributeSemantic>();

        protected Dictionary<VertexAttributeSemantic, List<float>> mFloatVertexAttributes = new Dictionary<VertexAttributeSemantic, List<float>>();

        protected Dictionary<VertexAttributeSemantic, List<Vector3>> mVector3VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector3>>();

        protected Dictionary<VertexAttributeSemantic, List<Vector2>> mVector2VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector2>>();

        protected Dictionary<VertexAttributeSemantic, List<Vector4>> mVector4VertexAttributes = new Dictionary<VertexAttributeSemantic, List<Vector4>>();

        protected List<uint> mUIntIndices = new List<uint>();

        protected List<ushort> mUShortIndices = new List<ushort>();
    }
}
