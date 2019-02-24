using System;
using System.Collections.Concurrent;
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
            if (item is Vector4Property)
            {
                return elemnt.FindResource("Vector4Template") as DataTemplate;
            }
            else if (item is Vector3Property)
            {
                return elemnt.FindResource("Vector3Template") as DataTemplate;
            }
            else if (item is Vector2Property)
            {
                return elemnt.FindResource("Vector2Template") as DataTemplate;
            }
            else if (item is FloatProperty)
            {
                if ((item as FloatProperty).UseSlider)
                {
                    return elemnt.FindResource("SliderFloatTemplate") as DataTemplate;
                }
                else
                {
                    return elemnt.FindResource("FloatTemplate") as DataTemplate;
                }
            }
            else if (item is IntProperty)
            {
                return elemnt.FindResource("IntTemplate") as DataTemplate;
            }
            else if (item is BoolProperty)
            {
                return elemnt.FindResource("BoolTemplate") as DataTemplate;
            }
            else if (item is EnumProperty)
            {
                return elemnt.FindResource("EnumTemplate") as DataTemplate;
            }
            else if (item is NestedObjectProperty)
            {
                return elemnt.FindResource("NestedObjectTemplate") as DataTemplate;
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
        }

        public void AddProperty(string name)
        {
            var label = new Label();
            
        }

        public void SetSceneObjectList(IEnumerable<object> objectList)
        {
            ObjectList.Items.Clear();
            ObjectProxyList.Items.Clear();

            foreach (var obj in objectList)
            {
                var proxy = new ObjectProxy(obj);

                ObjectList.Items.Add(proxy);
            }
        }

        public void SetObject(object target)
        {
            var proxy = new ObjectProxy(target);

            ObjectProxyList.Items.Clear();

            ObjectProxyList.Items.Add(proxy);
        }

        private void SetProxyObject(ObjectProxy obj)
        {
            ObjectProxyList.Items.Clear();

            ObjectProxyList.Items.Add(obj);
        }

        public void AddObjectToWatch(object target)
        {
            var proxy = new ObjectProxy(target);

            ObjectProxyList.Items.Add(proxy);
        }


        public EventHandler<EventArgs> ObjectCreateEventHandler;


        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var property = textBox.DataContext as ObjectProperty;
            property.ApplyValue();
        }

        public IEnumerable<ObjectProperty> SceneObjectList => sceneObjectList;

        private IEnumerable<ObjectProperty> sceneObjectList = null;

        private void ObjectList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetProxyObject((ObjectProxy)ObjectList.SelectedItem);
            
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var property = checkBox.DataContext as ObjectProperty;
            property.ApplyValue();
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            var property = slider.DataContext as ObjectProperty;
            property.ApplyValue();
        }

        private void CreateSphere_OnClick(object sender, RoutedEventArgs e)
        {
            ObjectCreateEventHandler(sender, e);
        }
    }
}
