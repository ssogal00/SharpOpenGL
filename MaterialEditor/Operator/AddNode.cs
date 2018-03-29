using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class AddNode : NodeViewModel
    {
        public AddNode()
            : base ("Add")
        {
            this.InputConnectors.Add(new ConnectorViewModel("In1"));
            this.InputConnectors.Add(new ConnectorViewModel("In2"));

            this.OutputConnectors.Add(new ConnectorViewModel("Out"));
        }
    }

    
}
