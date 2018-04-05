using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class ViewDirNode : NodeViewModel
    {
        public ViewDirNode()
            : base("View Dir")
        {
        }

        public override string GetExpressionForOutput(int outputIndex)
        {
            if (outputIndex == 0)
            {
                return string.Format("(-normalize(InPosition))");
            }

            return string.Empty;
        }


        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();
            OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector3, 0));
        }
    }
}
