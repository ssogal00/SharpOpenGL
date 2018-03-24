using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;

namespace MaterialEditor
{
    public class ConnectionViewModel : AbstractModelBase
    {
        private ConnectorViewModel sourceConnector = null;

        private ConnectorViewModel destConnector = null;

        private Point sourceConnectorHotspot;

        private Point destConnectorHotspot;
        
        public Point SourceConnectorHotspot
        {
            get
            {
                return sourceConnectorHotspot;
            }

            set
            {
                sourceConnectorHotspot = value;
                OnPropertyChanged("SourceConnectorHotspot");
            }
        }

        public Point DestConnectorHotspot
        {
            get
            {
                return destConnectorHotspot;
            }

            set
            {
                destConnectorHotspot = value;
                OnPropertyChanged("DestConnectorHotspot");
            }
        }
      
        public ConnectorViewModel SourceConnector
        {
            get
            {
                return sourceConnector;
            }

            set
            {
                if(sourceConnector == value)
                {
                    return;
                }

                if(sourceConnector != null)
                {
                    Trace.Assert(sourceConnector.AttachedConnection == this);

                    sourceConnector.AttachedConnection = null;
                    sourceConnector.HotspotUpdated -= new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                }

                sourceConnector = value;

                if(sourceConnector != null)
                {
                    Trace.Assert(sourceConnector.AttachedConnection == null);

                    sourceConnector.AttachedConnection = this;
                    sourceConnector.HotspotUpdated += new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                    this.SourceConnectorHotspot = sourceConnector.Hotspot;
                }

                OnPropertyChanged("SourceConnector");
            }
        }

        private void sourceConnector_HotspotUpdated(object sender, EventArgs e)
        {
            this.SourceConnectorHotspot = this.SourceConnector.Hotspot;
        }
        
    }
}
