using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ObjectEditor
{
    public class FloatProperty : ObjectProperty
    {
        public FloatProperty(string name, float value)
        {
            propertyName = name;
            FloatValue = value;
        }

        public FloatProperty(string name, float value, float minValue, float maxValue)
        : this(name, value)
        {
            Min = minValue;
            Max = maxValue;
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

        public override void InitializeCustomAttributes(IEnumerable<CustomAttributeData>  customAttributes)
        {
            if (customAttributes.Any(x => x.AttributeType.Name == "Range"))
            {
                var attr = customAttributes.First(x => x.AttributeType.Name == "Range");
                Min = Convert.ToSingle(attr.ConstructorArguments[0].Value);
                Max = Convert.ToSingle(attr.ConstructorArguments[1].Value);
                bIsRangeExist = true;
            }
        }

        public float FloatValue { get; set; }

        public bool UseSlider { get; set; } = false;

        public float Min { get; set; } = float.MinValue;
        public float Max { get; set; } = float.MaxValue;

        public Visibility SliderVisibility
        {
            get
            {
                if (bIsRangeExist)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
        }

        private bool bIsRangeExist = false;
    }
}
