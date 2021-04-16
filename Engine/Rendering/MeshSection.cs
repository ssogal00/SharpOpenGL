using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLTF;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Rendering
{
    public class MeshSection
    {
        public string MaterialName = string.Empty;

        public bool HasIndex
        {
            get
            {
                return (mUIntIndices.Count > 0 || mUShortIndices.Count > 0);
            }
        }

        public List<uint> UIntIndices => mUIntIndices;

        public List<ushort> UShortIndices => mUShortIndices;

        public DrawElementsType IndexType
        {
            get
            {
                if (UIntIndices.Count > 0)
                {
                    return DrawElementsType.UnsignedInt;
                }
                else if (UShortIndices.Count > 0)
                {
                    return DrawElementsType.UnsignedShort;
                }

                return DrawElementsType.UnsignedInt;
            }
        }

        public MeshSection(string materialName,
            Dictionary<string, VertexAttributeSemantic> vertexAttributeMap,
            Dictionary<VertexAttributeSemantic, List<Vector2>> vector2VertexAttributes,
            Dictionary<VertexAttributeSemantic, List<Vector3>> vector3VertexAttributes,
            Dictionary<VertexAttributeSemantic, List<Vector4>> vector4VertexAttributes,
            List<uint> uintIndices, List<ushort> ushortIndices)
        {
            this.MaterialName = materialName;
            this.mVertexAttributeMap = vertexAttributeMap;
            this.mVector2VertexAttributes = vector2VertexAttributes;
            this.mVector3VertexAttributes = vector3VertexAttributes;
            this.mVector4VertexAttributes = vector4VertexAttributes;
            
            if (ushortIndices != null)
            {
                this.mUShortIndices = ushortIndices;
            }

            if (uintIndices != null)
            {
                this.mUIntIndices = uintIndices;
            }
        }

        public int IndexCount
        {
            get
            {
                return Math.Max(UIntIndices.Count, UShortIndices.Count);
            }
        }

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
