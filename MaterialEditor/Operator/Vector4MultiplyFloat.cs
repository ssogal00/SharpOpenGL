using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor.Operator
{
    public class Vector4MultiplyFloat : NodeViewModel
    {
        public Vector4MultiplyFloat()
            : base("Vector4 x Float")
        {

        }

        protected override bool IsConnectionValidForEvaluation()
        {
            if(InputConnectors[0].AttachedConnections.Count == 1 && InputConnectors[1].AttachedConnections.Count == 1)
            {
                return true;
            }

            return false;
        }

        public override string ToExpression()
        {
            if (InputConnectors[0].AttachedConnections.Count == 1 && InputConnectors[1].AttachedConnections.Count == 1)
            {
                var expressionA = InputConnectors[0].AttachedConnections[0].SourceConnector.ParentNode.ToExpression();
                var expressionB = InputConnectors[1].AttachedConnections[0].SourceConnector.ParentNode.ToExpression();

                return string.Format("{0}*{1}", expressionA, expressionB);
            }

            return string.Empty;
        }

        public override string GetExpressionForOutput(int outputIndex)
        {
            if(IsConnectionValidForEvaluation() && outputIndex == 0)
            {
                var expressionA = GetExpressionForInput(0);
                var expressionB = GetExpressionForInput(1);

                return string.Format("{0}*{1}", expressionA, expressionB);
            }

            return string.Empty;
        }

        protected override void CreateInputOutputConnectors()
        {
            // input
            this.InputConnectors.Add(new ConnectorViewModel("Vec4", ConnectorDataType.ConstantVector4, 0));
            this.InputConnectors.Add(new ConnectorViewModel("Float", ConnectorDataType.ConstantFloat, 1));
            // output
            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector4, 0));
        }
    }
}
