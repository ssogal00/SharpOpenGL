using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class CosineNode : NodeViewModel
    {
        public CosineNode()
            : base("Cosine")
        {
        }

        
        public override string GetExpressionForOutput(int outputIndex)
        {
            if(InputConnectors[0].AttachedConnections.Count == 1 && outputIndex == 0)
            {
                var expression = GetExpressionForInput(0);

                return string.Format("cos({0})", expression);
            }

            return string.Empty;
        }

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();
            // input
            InputConnectors.Add(new ConnectorViewModel("In", ConnectorDataType.ConstantFloat, 0));

            // output
            OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantFloat, 0));
        }
    }
}
