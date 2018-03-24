using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialEditor
{
    public class ConnectorViewModel : AbstractModelBase
    {
        public ConnectorViewModel(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

        private Point hotspot;

        public NodeViewModel ParentNode
        {
            get;
            internal set;
        }

        public Point Hotspot
        {
            get
            {
                return hotspot;
            }

            set
            {
                if(hotspot == value)
                {
                    return;
                }

                hotspot = value;

                OnHotspotUpdated();
            }
        }

        public event EventHandler<EventArgs> HotspotUpdated;

        private void OnHotspotUpdated()
        {
            OnPropertyChanged("Hotspot");

            if(HotspotUpdated != null)
            {
                HotspotUpdated(this, EventArgs.Empty);
            }
        }

        public ConnectionViewModel AttachedConnection
        {
            get;
            internal set;
        }

        public bool IsConnectionAttached
        {
            get { return true; }
        }
    }
}
