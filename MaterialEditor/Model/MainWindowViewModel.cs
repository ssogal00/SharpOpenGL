using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialEditor
{
    public class MainWindowViewModel : AbstractModelBase
    {
        public MainWindowViewModel()
        {
            PopulateWithTestData();
        }

        public NodeViewModel CreateNode(string name, Point nodeLocation)
        {
            var node = new NodeViewModel(name);
            node.X = nodeLocation.X;
            node.Y = nodeLocation.Y;

            //
            // Create the default set of four connectors.
            //
            node.InputConnectors.Add(new ConnectorViewModel("In1"));
            node.InputConnectors.Add(new ConnectorViewModel("In2"));
            node.InputConnectors.Add(new ConnectorViewModel("In3"));
            node.OutputConnectors.Add(new ConnectorViewModel("Out1"));
            node.OutputConnectors.Add(new ConnectorViewModel("Out2"));

            //
            // Add the new node to the view-model.
            //
            this.Network.Nodes.Add(node);

            return node;
        }

        private void PopulateWithTestData()
        {
            this.Network = new NetworkViewModel();

            var node1 = CreateNode("Node1", new Point(50, 50));
        }

        public NetworkViewModel network = null;

        public NetworkViewModel Network
        {
            get
            {
                return network;
            }
            set
            {
                network = value;
                OnPropertyChanged("Network");
            }
        }


    }
}
