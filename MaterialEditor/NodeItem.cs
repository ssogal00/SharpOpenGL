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

        private bool isLeftMouseDown = false;
        private bool isLeftMouseAndControlDown = false;
        private bool isDragging = false;

        private Point lastMousePoint;

        public NetworkView ParentNetworkView
        {
            get
            {
                return (NetworkView)GetValue(ParentNetworkViewProperty);
            }

            set
            {
                SetValue(ParentNetworkViewProperty, value);
            }
        }

        internal static readonly RoutedEvent NodeDragStartedEvent =
          EventManager.RegisterRoutedEvent("NodeDragStarted", RoutingStrategy.Bubble, typeof(NodeDragStartedEventHandler), typeof(NodeItem));


        static NodeItem()
        { 
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeItem), new FrameworkPropertyMetadata(typeof(NodeItem)));
        }

        private static void ParentNetworkView_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var nodeItem = (NodeItem)o;
            //nodeItem.BringToFront();
        }

        internal void BringToFront()
        {
            if(ParentNetworkView == null)
            {
                return;
            }

            int maxZ = this.ParentNetworkView.FindMaxZIndex();
            this.ZIndex = maxZ + 1;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            BringToFront();

            if(this.ParentNetworkView != null)
            {
                this.ParentNetworkView.Focus();
            }

            if(e.ChangedButton == MouseButton.Left && this.ParentNetworkView != null)
            {
                lastMousePoint = e.GetPosition(this.ParentNetworkView);
                isLeftMouseDown = true;

                LeftMouseDownSelectionLogic();

                e.Handled = true;
            }
        }

        internal void LeftMouseDownSelectionLogic()
        {
            if((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                isLeftMouseAndControlDown = true;
            }
            else
            {
                isLeftMouseAndControlDown = false;
                if(this.ParentNetworkView.SelectedNodes.Count == 0)
                {
                    this.IsSelected = true;
                }
                else if(this.ParentNetworkView.SelectedNodes.Contains(this) || this.ParentNetworkView.SelectedNodes.Contains(this.DataContext))
                {
                    return;
                }
                else
                {
                    this.ParentNetworkView.SelectedNodes.Clear();
                    this.IsSelected = true;
                }
            }
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
