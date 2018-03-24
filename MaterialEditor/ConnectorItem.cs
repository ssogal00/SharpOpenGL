using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;


namespace MaterialEditor
{
    public class ConnectorItem : ContentControl
    {
        public static readonly DependencyProperty HotspotProperty = DependencyProperty.Register("Hotspot", typeof(Point), typeof(ConnectorItem));

        public Point Hotspot
        {
            get
            {
                return (Point)GetValue(HotspotProperty);
            }

            set
            {
                SetValue(HotspotProperty, value);
            }
        }

        internal static readonly DependencyProperty ParentNetworkViewProperty =
            DependencyProperty.Register("ParentNetworkView", typeof(NetworkView), typeof(ConnectorItem), new FrameworkPropertyMetadata(ParentNetworkView_PropertyChanged));

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

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            
        }

        private static void ParentNetworkView_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectorItem c = (ConnectorItem)d;

            c.UpdateHotspot();
        }

        private void UpdateHotspot()
        {
            if(this.ParentNetworkView == null)
            {
                return;
            }

            if(!this.ParentNetworkView.IsAncestorOf(this))
            {
                this.ParentNetworkView = null;
                return;
            }

            var centerPoint = new Point(this.ActualWidth / 2, this.ActualHeight / 2);

            this.Hotspot = this.TransformToAncestor(this.ParentNetworkView).Transform(centerPoint);
        }

    }
}
