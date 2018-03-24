using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialEditor.Utils;

namespace MaterialEditor
{
    public class NetworkViewModel : AbstractModelBase
    {
        protected ImpObservableCollection<NodeViewModel> nodes = null;

        protected ImpObservableCollection<ConnectionViewModel> connections = null;

        public ImpObservableCollection<ConnectionViewModel> Connections
        {
            get
            {
                if(connections == null)
                {
                    connections = new ImpObservableCollection<ConnectionViewModel>();
                }

                return connections;
            }
        }

        public ImpObservableCollection<NodeViewModel> Nodes
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
    }
}
