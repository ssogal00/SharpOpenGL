﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ObjectEditor
{
    public class ObjectPropertyDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement elemnt = container as FrameworkElement;
            if (item is Vector3Property)
            {
                return elemnt.FindResource("Vector3Template") as DataTemplate;
            }
            else if (item is Vector2Property)
            {
                return elemnt.FindResource("Vector2Template") as DataTemplate;
            }
            else if (item is FloatProperty)
            {
                return elemnt.FindResource("FloatTemplate") as DataTemplate;
            }
            else if (item is IntProperty)
            {
                return elemnt.FindResource("IntTemplate") as DataTemplate;
            }

            return elemnt.FindResource("Vector3Template") as DataTemplate;
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //ObjPropList.Items.Add(testValue);
            //ObjPropList.Items.Add(testValue2);
        }

        

        private Vector3Property testValue = new Vector3Property("Position" ,new Vector3(10,0,0));
        private Vector2Property testValue2 = new Vector2Property("Scale", new Vector2(11, 0));

        public void AddProperty(string name)
        {
            var label = new Label();
            
        }

        public void SetCameraPosition(string value)
        {
            
        }

        public void SetObject(object target)
        {
            var t = target.GetType();
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
                        var propvalue = property.GetValue(target);

                        var prop = ObjectProperty.CreateProperty(name, propertyType);
                        prop.SetValue(propvalue);

                        ObjPropList.Items.Add(prop);
                    }
                }
            }
        }

        private void CreateObjectBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ObjectCreateEventHandler(sender, e);
        }

        public EventHandler<EventArgs> ObjectCreateEventHandler;

       
    }
}
