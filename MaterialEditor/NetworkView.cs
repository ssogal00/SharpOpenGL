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
    public partial class NetworkView : Control
    {
        public NetworkView()
        {
            this.Nodes = new ImpObservableCollection<object>();

            this.Connections = new ImpObservableCollection<object>();

            AddHandler(NodeItem.NodeDragStartedEvent, new NodeDragStartedEventHandler(NodeItem_DragStarted));
        }

        public static readonly DependencyProperty NodesSourceProperty =
            DependencyProperty.Register("NodesSource", typeof(IEnumerable), typeof(NetworkView), new FrameworkPropertyMetadata(NodesSource_PropertyChanged));

        private static readonly DependencyPropertyKey NodesPropertyKey =
           DependencyProperty.RegisterReadOnly("Nodes", typeof(ImpObservableCollection<object>), typeof(NetworkView),
               new FrameworkPropertyMetadata());

        public static readonly DependencyProperty NodesProperty = NodesPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ConnectionsSourceProperty =
            DependencyProperty.Register("ConnectionsSource", typeof(IEnumerable), typeof(NetworkView), new FrameworkPropertyMetadata(ConnectionsSource_PropertyChanged));

        private static readonly DependencyPropertyKey ConnectionsPropertyKey =
            DependencyProperty.RegisterReadOnly("Connections", typeof(ImpObservableCollection<object>), typeof(NetworkView),
                new FrameworkPropertyMetadata());
        
        public static readonly DependencyProperty ConnectionsProperty = ConnectionsPropertyKey.DependencyProperty;


        public static readonly DependencyPropertyKey IsDraggingNodePropertyKey =
            DependencyProperty.RegisterReadOnly("IsDraggingNode", typeof(bool), typeof(NetworkView),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsDraggingNodeProperty = IsDraggingNodePropertyKey.DependencyProperty;

        public static readonly RoutedEvent NodeDragStartedEvent = EventManager.RegisterRoutedEvent("NodeDragStarted", RoutingStrategy.Bubble,
            typeof(NodeDragStartedEventHandler), typeof(NetworkView));

        private List<object> initialSelectedNodes = null;

        public bool IsDraggingNode
        {
            get
            {
                return (bool)GetValue(IsDraggingNodeProperty);
            }
            private set
            {
                SetValue(IsDraggingNodeProperty, value);
            }
        }


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

        private static void ConnectionsSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NetworkView c = (NetworkView)d;

            c.Connections.Clear();
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

        public ImpObservableCollection<object> Connections
        {
            get
            {
                return (ImpObservableCollection<object>)GetValue(ConnectionsProperty);
            }
            set
            {
                SetValue(ConnectionsPropertyKey, value);
            }
        }

        public IEnumerable ConnectionsSource
        {
            get
            {
                return (IEnumerable)GetValue(ConnectionsSourceProperty);
            }

            set
            {
                SetValue(ConnectionsSourceProperty, value);
            }
        }

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

        public IList SelectedNodes
        {
            get
            {
                if(nodeItemsControl != null)
                {
                    return nodeItemsControl.SelectedItems;
                }
                else
                {
                    if(initialSelectedNodes == null)
                    {
                        initialSelectedNodes = new List<object>();
                    }

                    return initialSelectedNodes;
                }
            }
        }
    }
}
