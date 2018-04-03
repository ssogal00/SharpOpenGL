using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{   
    public class MaskRNode : NodeViewModel
    {

        public MaskRNode()
            :base ("Mask R")
        {}
       
        public override string GetExpressionForOutput(int outputIndex)
        {
            if(outputIndex == 0 && InputConnectors[0].AttachedConnections.Count == 1)
            {
                var expressionA = GetExpressionForInput(0);

                return string.Format("({0}).r", expressionA);                
            }

            return string.Empty;
        }

        protected override void CreateInputOutputConnectors()
        {
            // input
            this.InputConnectors.Add(new ConnectorViewModel("In", ConnectorDataType.ConstantVector4, 0));

            // output
            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantFloat, 0));
        }

    }
}
