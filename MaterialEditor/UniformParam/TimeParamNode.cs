using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class TimeParamNode : NodeViewModel
    {
        public TimeParamNode()
            : base("Time")
        {
        }

        public override string ToExpression()
        {
            return "time";
        }

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();
            //
            OutputConnectors.Add(new ConnectorViewModel("Value", ConnectorDataType.ConstantFloat, 0));
        }
    }
}
