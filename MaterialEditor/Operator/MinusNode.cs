using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class MinusNode : NodeViewModel
    {
        public MinusNode()
            : base("Minus")
        {
            this.InputConnectors.Add(new ConnectorViewModel("A"));
            this.InputConnectors.Add(new ConnectorViewModel("B"));

            this.OutputConnectors.Add(new ConnectorViewModel("Out"));
        }
    }
}
