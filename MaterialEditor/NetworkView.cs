using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Specialized;
using MaterialEditor.Utils;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Input;


namespace MaterialEditor
{
    public class NetworkView : Control
    {
        public NetworkView()
        {
            this.Nodes = new ImpObservableCollection<object>();
        }

        public static readonly DependencyProperty NodesSourceProperty =
            DependencyProperty.Register("NodesSource", typeof(IEnumerable), typeof(NetworkView), new FrameworkPropertyMetadata(NodesSource_PropertyChanged));

        private static readonly DependencyPropertyKey NodesPropertyKey =
           DependencyProperty.RegisterReadOnly("Nodes", typeof(ImpObservableCollection<object>), typeof(NetworkView),
               new FrameworkPropertyMetadata());

        public static readonly DependencyProperty NodesProperty = NodesPropertyKey.DependencyProperty;


        //public static readonly RoutedEvent NodeDragStartedEvent =
         //   EventManager.RegisterRoutedEvent("NodeDragStarted", RoutingStrategy.Bubble, );

        internal int FindMaxZIndex()
        {
            if(this.nodeItemsControl == null)
            {
                return 0;
            }

            int maxZ = 0;

            for(int nodeIndex = 0; ; ++nodeIndex)
            {
                NodeItem nodeItem = (NodeItem)this.nodeItemsControl.ItemContainerGenerator.ContainerFromIndex(nodeIndex);
                if(nodeItem == null)
                {
                    break;
                }

                if(nodeItem.ZIndex > maxZ)
                {
                    maxZ = nodeItem.ZIndex;
                }
            }

            return maxZ;
        }


        private static void NodesSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NetworkView c = (NetworkView)d;
            c.Nodes.Clear();

            if(e.NewValue != null)
            {
                var enumerable = e.NewValue as IEnumerable;
                if(enumerable != null)
                {
                    foreach(object obj in enumerable)
                    {
                        c.Nodes.Add(obj);
                    }
                }
            }
        }

        static NetworkView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NetworkView), new FrameworkPropertyMetadata(typeof(NetworkView)));
        }

        private NodeItemsControl nodeItemsControl = null;

        public ImpObservableCollection<object> Nodes
        {
            get
            {
                return (ImpObservableCollection<object>) GetValue(NodesProperty);
            }
            private set
            {
                SetValue(NodesPropertyKey, value);
            }
        }

        public IEnumerable NodesSource
        {
            get
            {
                return (IEnumerable)GetValue(NodesSourceProperty);
            }
            set
            {
                SetValue(NodesSourceProperty, value);
            }
        }
    }
}
