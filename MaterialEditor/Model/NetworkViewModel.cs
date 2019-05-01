using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialEditor.Utils;

using ZeroFormatter;

namespace MaterialEditor
{
    [ZeroFormattable]
    public class NetworkViewModel : AbstractModelBase
    {
        protected ImpObservableCollection<NodeViewModel> nodes = null;

        protected ImpObservableCollection<ConnectionViewModel> connections = null;

        protected NodeViewModel currentSelectedNode = null;

        public NodeViewModel CurrentSelectedNode
        {
            get
            {
                if(currentSelectedNode != null)
                {
                    return currentSelectedNode;
                }

                return new NodeViewModel("Dummy Node");
            }

            set
            {
                currentSelectedNode = value;
                OnPropertyChanged("CurrentSelectedNode");
            }
        }

        [Index(0)]
        public ImpObservableCollection<ConnectionViewModel> Connections
        {
            get
            {
                if(connections == null)
                {
                    connections = new ImpObservableCollection<ConnectionViewModel>();
                    connections.ItemsRemoved += new EventHandler<CollectionItemsChangedEventArgs>(connections_ItemsRemoved);
                }

                return connections;
            }
        }

        [Index(1)]
        public virtual ImpObservableCollection<NodeViewModel> Nodes
        {
            get
            {
                if(nodes == null)
                {
                    nodes = new ImpObservableCollection<NodeViewModel>();
                    
                }
                return nodes;
            }
        }

        private void connections_ItemsRemoved(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectionViewModel connection in e.Items)
            {
                connection.SourceConnector = null;
                connection.DestConnector = null;
            }
        }
    }
}
