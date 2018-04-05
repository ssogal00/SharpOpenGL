using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    class MaxNode : NodeViewModel
    {
        public MaxNode()
            : base("Max")
        { }

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();

            // in
            InputConnectors.Add(new ConnectorViewModel("A", ConnectorDataType.ConstantFloat, 0));
            InputConnectors.Add(new ConnectorViewModel("B", ConnectorDataType.ConstantFloat, 1));

            // out
            InputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantFloat, 0));
        }

        protected override bool IsConnectionValidForEvaluation()
        {
            if(InputConnectors[0].AttachedConnections.Count == 1 && InputConnectors[1].AttachedConnections.Count == 1)
            {
                return true;
            }

            return false;
        }


        public override string GetExpressionForOutput(int outputIndex)
        {
            if (outputIndex == 0 && IsConnectionValidForEvaluation())
            {
                return string.Format("max({0}, {1})", GetExpressionForInput(0), GetExpressionForInput(1));
            }

            return string.Empty;
        }

    }
}
