using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class ResultNode : NodeViewModel
    {
        public ResultNode()
            : base("Material")
        { }

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();

            // input
            InputConnectors.Add(new ConnectorViewModel("Albedo", ConnectorDataType.ConstantVector4,0));            
            InputConnectors.Add(new ConnectorViewModel("Normal", ConnectorDataType.ConstantVector4,1));
        }

        public override string ToExpression()
        {
            if(this.InputConnectors[0].AttachedConnections.Count == 1)
            {
                return InputConnectors[0].AttachedConnections[0].SourceConnector.ParentNode.ToExpression();
            }

            return "No compile result";
        }
    }
}
