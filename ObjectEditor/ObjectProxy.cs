using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using OpenTK.Graphics.ES11;

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
                if (property.Name == "Name")
                {   
                    var propertyValue = property.GetValue(originalObject);
                    objectName = (string) propertyValue;
                }

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

        private ObservableCollection<ObjectProperty> propertyList = new ObservableCollection<ObjectProperty>();
        public ObservableCollection<ObjectProperty> PropertyList => propertyList;

        public string ObjectName
        {
            get { return objectName; }
        }
        private string objectName = "";

        public override string ToString()
        {
            return objectName;
        }
    }
}
