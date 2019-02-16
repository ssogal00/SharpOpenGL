using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor
{
    public class ObjectProxy
    {
        public ObjectProxy(object originalObject)
        {
            var t = originalObject.GetType();
            var properties = t.GetProperties();

            foreach (var property in properties)
            {
                if (property.CustomAttributes.Any(x => x.AttributeType.Name == "ExposeUI"))
                {
                    string name = property.Name;

                    Type propertyType = property.PropertyType;

                    if (ObjectProperty.IsSupportedType(propertyType))
                    {
                        var obj = Activator.CreateInstance(propertyType);
                        var propertyValue = property.GetValue(originalObject);

                        var prop = ObjectProperty.CreateProperty(name, propertyType, originalObject);
                        prop.SetValue(propertyValue);

                        propertyList.Add(prop);
                    }
                }
            }
        }

        public List<ObjectProperty> PropertyList => propertyList;

        private List<ObjectProperty> propertyList = new List<ObjectProperty>();
    }
}
