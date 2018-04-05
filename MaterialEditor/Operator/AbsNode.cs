using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class AbsNode : NodeViewModel
    {
        public AbsNode()
            : base("Abs")
        {}

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();

            // in
            InputConnectors.Add(new ConnectorViewModel("In", ConnectorDataType.ConstantFloat, 0));

            // out
            InputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantFloat, 0));
        }


        public override string GetExpressionForOutput(int outputIndex)
        {
            if(outputIndex == 0 && InputConnectors[0].AttachedConnections.Count == 1)
            {
                return string.Format("abs({0})", GetExpressionForInput(0));
            }

            return string.Empty;
        }

    }
}
