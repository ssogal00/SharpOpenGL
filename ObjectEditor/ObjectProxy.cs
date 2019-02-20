using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using OpenTK.Graphics.ES11;

namespace ObjectEditor
{
    public class ObjectProxy
    {
        public ObjectProxy(object originalObject, ObjectProperty parentProperty = null)
        {
            this.ParentProperty = parentProperty;

            var t = originalObject.GetType();

            var properties = t.GetProperties();

            var fields = t.GetFields();

            foreach (var property in properties)
            {
                if (property.Name == "Name")
                {   
                    var propertyValue = property.GetValue(originalObject);
                    objectName = (string) propertyValue;
                    continue;
                }

                if (property.CustomAttributes.Any(x => x.AttributeType.Name == "ExposeUI"))
                {
                    string name = property.Name;

                    Type propertyType = property.PropertyType;

                    if (ObjectProperty.IsSupportedType(propertyType))
                    {
                        var obj = Activator.CreateInstance(propertyType);
                        var propertyValue = property.GetValue(originalObject);

                        var prop = ObjectProperty.CreateProperty(name, propertyType, originalObject, this);
                        prop.SetValue(propertyValue);

                        propertyList.Add(prop);
                    }
                    else if (propertyType.IsClass || propertyType.IsValueType)
                    {
                        var obj = Activator.CreateInstance(propertyType);
                        var propertyValue = property.GetValue(originalObject);

                        var prop = ObjectProperty.CreateProperty(name, propertyType, originalObject, this);
                        prop.SetValue(propertyValue);

                        propertyList.Add(prop);
                    }
                }
            }

            foreach (var field in fields)
            {
                if (field.CustomAttributes.Any(x => x.AttributeType.Name == "ExposeUI"))
                {
                    string name = field.Name;

                    Type fieldType = field.FieldType;

                    if (ObjectProperty.IsSupportedType(fieldType))
                    {
                        var obj = Activator.CreateInstance(fieldType);
                        var propertyValue = field.GetValue(originalObject);

                        var prop = ObjectProperty.CreateProperty(name, fieldType, originalObject, this, true);
                        prop.SetValue(propertyValue);

                        propertyList.Add(prop);
                    }
                    else if (fieldType.IsClass || fieldType.IsValueType)
                    {
                        var obj = Activator.CreateInstance(fieldType);
                        var propertyValue = field.GetValue(originalObject);

                        var prop = (NestedObjectProperty) ObjectProperty.CreateProperty(name, fieldType, originalObject, this, true);
                        prop.SetValue(propertyValue);

                        var objProxy = new ObjectProxy(propertyValue, prop);
                        prop.NestedObject = objProxy;
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

        public ObjectProperty ParentProperty = null;
    }
}
