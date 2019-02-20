using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor
{
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

        public override void SetValue(object value)
        {
            FloatValue = (float)value;
        }

        public override object GetValue()
        {
            return FloatValue;
        }

        public override void Initialize(MemberInfo memberInfo)
        {
            if (memberInfo.CustomAttributes.Any(x => x.AttributeType.Name == "UseSlider"))
            {
                var attr = memberInfo.CustomAttributes.First(x => x.AttributeType.Name == "UseSlider");
                UseSlider = true;
                Min = Convert.ToSingle(attr.ConstructorArguments[0]);
                Max = Convert.ToSingle(attr.ConstructorArguments[1]);
            }
        }

        public float FloatValue { get; set; }

        public bool UseSlider { get; set; } = false;

        public float Min { get; set; } = float.MinValue;
        public float Max { get; set; } = float.MaxValue;
    }
}
