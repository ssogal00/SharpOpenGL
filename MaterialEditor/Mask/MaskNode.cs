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

        public override string ToExpression()
        {
            if (InputConnectors[0].AttachedConnections.Count == 1)
            {
                var expressionA = InputConnectors[0].AttachedConnections[0].SourceNodeModel.ToExpression();                

                return string.Format("({0}).r", expressionA);
            }

            return string.Empty;
        }

        protected override void CreateInputOutputConnectors()
        {
            this.InputConnectors.Add(new ConnectorViewModel("In", ConnectorDataType.ConstantVector4));            
            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantFloat));
        }

    }
}
