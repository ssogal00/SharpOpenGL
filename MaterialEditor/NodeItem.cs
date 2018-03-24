using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

using System.Windows.Input;

namespace MaterialEditor
{
    public class NodeItem : ListBoxItem
    {
        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(NodeItem),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(NodeItem),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ZIndexProperty = DependencyProperty.Register("ZIndex", typeof(int), typeof(NodeItem),
            new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        internal static readonly DependencyProperty ParentNetworkViewProperty = DependencyProperty.Register("ParentNetworkView", typeof(NetworkView),
            typeof(NodeItem), new FrameworkPropertyMetadata(ParentNetworkView_PropertyChanged));

        //internal static readonly RoutedEvent NodeDragStartedEvent = EventManager.RegisterRoutedEvent("NodeDragStarted", RoutingStrategy.Bubble, typeof(Nodedra)

        static NodeItem()
        { 
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeItem), new FrameworkPropertyMetadata(typeof(NodeItem)));
        }

        private static void ParentNetworkView_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var nodeItem = (NodeItem)o;
            //nodeItem.BringToFront();
        }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public int ZIndex
        {
            get { return (int)GetValue(ZIndexProperty); }
            set { SetValue(ZIndexProperty, value); }
        }
    }
}
