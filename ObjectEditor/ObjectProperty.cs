using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenTK;
using System.Drawing;
using System.Windows.Media;

namespace ObjectEditor
{
    public class ObjectProperty 
    {

        protected string propertyName = "";

        protected object targetObject = null;

        public string PropertyName
        {
            get => propertyName;
            set => propertyName = value;
        }

        public virtual void SetValue(object value) { }

        public void SetTargetObject(object obj)
        {
            targetObject = obj;
        }

        public virtual void ApplyValue()
        {
        }

        private static List<Type> supportedTypes =new List< Type>()
        {
            typeof(OpenTK.Vector3),
            typeof(OpenTK.Vector4),
            typeof(OpenTK.Vector2),
            typeof(bool),
            typeof(float),
            typeof(int)
        };

        private static Dictionary<Type, Type> typeDictionary = new Dictionary<Type, Type>()
        {
            { typeof(OpenTK.Vector4), typeof(Vector4Property) },
            { typeof(OpenTK.Vector3), typeof(Vector3Property) },
            { typeof(OpenTK.Vector2), typeof(Vector2Property) },
            { typeof(float), typeof(FloatProperty) },
            { typeof(int), typeof(IntProperty) },
            { typeof(bool), typeof(BoolProperty) },
        };

        public static bool IsSupportedType(Type t)
        {
            if (supportedTypes.Contains(t))
            {
                return true;
            }
            else if (t.IsEnum)
            {
                return true;
            }

            return false;
        }

        public static ObjectProperty CreateProperty(string name, Type originalType, object targetObject)
        {
            // supported types
            if (typeDictionary.ContainsKey(originalType))
            {
                var result = (ObjectProperty) Activator.CreateInstance(typeDictionary[originalType]);
                result.PropertyName = name;
                result.SetTargetObject(targetObject);
                return result;
            }
            // for enum type
            else if (originalType.IsEnum)
            {
                var result = new EnumProperty(name, originalType);
                return result;
            }

            return null;
        }
    }

    public class ColorProperty : ObjectProperty
    {
        public ColorProperty(string name, OpenTK.Vector3 color)
        {

        }

        public override void SetValue(object value)
        {
            vec = (Vector3)value;
        }

        public override void ApplyValue()
        {
            var prop = targetObject.GetType().GetProperties().First(x => x.Name == PropertyName);
            prop.SetValue(targetObject, vec);
        }

        private OpenTK.Vector3 vec;

        public float R
        {
            get => vec.X;
            set => vec.X = value;
        }

        public float G
        {
            get => vec.Y;
            set => vec.Y = value;
        }

        public float B
        {
            get => vec.Z;
            set => vec.Z = value;
        }
    }

    public class StructProperty : ObjectProperty
    {

    }

    public class FloatProperty : ObjectProperty
    {
        public FloatProperty(string name, float value)
        {
            propertyName = name;
            FloatValue = value;
        }

        public FloatProperty()
        {
        }

        public override void ApplyValue()
        {
            var prop = targetObject.GetType().GetProperties().First(x => x.Name == PropertyName);
            prop.SetValue(targetObject, FloatValue);
        }

        public override void SetValue(object value)
        {
            FloatValue = (float)value;
        }

        public float FloatValue { get; set; }
    }

    public class IntProperty : ObjectProperty
    {
        public IntProperty(string name, int value)
        {
            propertyName = name;
            IntValue = value;
        }

        public int IntValue { get; set; }

        public override void SetValue(object value)
        {
            IntValue = (int) value;
        }

        public IntProperty()
        {
        }

        public override void ApplyValue()
        {
            var prop = targetObject.GetType().GetProperties().First(x => x.Name == PropertyName);
            prop.SetValue(targetObject, IntValue);
        }
    }

    public class Vector4Property : ObjectProperty
    {
        public Vector4Property(string name, OpenTK.Vector4 vectorValue)
        {
            propertyName = name;
            vec = vectorValue;
        }
        public Vector4Property() { }

        public override void SetValue(object value)
        {
            vec = (Vector4)value;
        }

        public override void ApplyValue()
        {
            var prop = targetObject.GetType().GetProperties().First(x => x.Name == PropertyName);
            prop.SetValue(targetObject, vec);
        }

        private OpenTK.Vector4 vec;

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

        public float W
        {
            get => vec.W;
            set => vec.W = value;
        }
    }

    public class BoolProperty : ObjectProperty
    {
        public BoolProperty(string name, bool value)
        {
            propertyName = name;
            BoolValue = value;
        }

        public BoolProperty()
        {

        }

        public override void SetValue(object value)
        {
            value = (bool)value;
        }

        public override void ApplyValue()
        {
            var prop = targetObject.GetType().GetProperties().First(x => x.Name == PropertyName);
            prop.SetValue(targetObject, BoolValue);
        }
        
        public bool BoolValue { get; set; }
    }

    #region EnumProperty Definition
    public class EnumProperty : ObjectProperty
    {
        public EnumProperty(string name, Type enumType)
        {
            propertyName = name;
            this.enumType = enumType;
            EnumNames = Enum.GetNames(enumType).ToList();
        }

        public override void ApplyValue()
        {
            var prop = targetObject.GetType().GetProperties().First(x => x.Name == PropertyName);
            prop.SetValue(targetObject, Enum.Parse(enumType, CurrentEnum));
        }

        public List<string> EnumNames { get; set; } = new List<string>();
        public string CurrentEnum { get; set; }

        private Type enumType;
    }
    #endregion
    public class Vector3Property : ObjectProperty
    {
        public Vector3Property(string name, OpenTK.Vector3 vectorValue)
        {
            propertyName = name;
            vec = vectorValue;
        }
        public Vector3Property() { }

        public override void SetValue(object value)
        {
            vec = (Vector3) value;
        }

        public override void ApplyValue()
        {
            var prop = targetObject.GetType().GetProperties().First(x => x.Name == PropertyName);
            prop.SetValue(targetObject, vec);
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

        public Vector2Property() { }

        public override void SetValue(object value)
        {
            vec = (Vector2)value;
        }

        public override void ApplyValue()
        {
            var prop = targetObject.GetType().GetProperties().First(x => x.Name == PropertyName);
            prop.SetValue(targetObject, vec);
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
