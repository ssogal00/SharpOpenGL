using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor
{
    public class ObjectProperty
    {
        protected string propertyName = "";
        public string PropertyName => propertyName;

        private static Dictionary<string, Type> supportedTypes = new Dictionary<string, Type>()
        {
            {typeof(OpenTK.Vector3).ToString(), typeof(OpenTK.Vector3)},
            {typeof(OpenTK.Vector2).ToString(), typeof(OpenTK.Vector2)},
            {typeof(float).ToString(), typeof(float)}
        };
    }

    public class FloatProperty : ObjectProperty
    {
        public FloatProperty(string name, float value)
        {
            propertyName = name;
            floatValue = value;
        }

        public float FloatValue
        {
            get => floatValue;
            set => floatValue = value;
        }

        private float floatValue = 0;
    }

    public class Vector3Property : ObjectProperty
    {
        public Vector3Property(string name, OpenTK.Vector3 vectorValue)
        {
            propertyName = name;
            vec = vectorValue;
        }

        private OpenTK.Vector3 vec;

        public float X
        {
            get => vec.X;
            set => vec.X = value;
        }

        public float Y
        {
            get => vec.Y;
            set => vec.Y = value;
        }

        public float Z
        {
            get => vec.Z;
            set => vec.Z = value;
        }
    }

    public class Vector2Property : ObjectProperty
    {
        public Vector2Property(string name, OpenTK.Vector2 vectorValue)
        {
            propertyName = name;
            vec = vectorValue;
        }

        private OpenTK.Vector2 vec;

        public float X
        {
            get => vec.X;
            set => vec.X = value;
        }

        public float Y
        {
            get => vec.Y;
            set => vec.Y = value;
        }
    }
}
