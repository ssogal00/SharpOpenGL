using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class ViewspacePositionNode : NodeViewModel
    {
        public ViewspacePositionNode()
            : base("Viewspace Position")
        {
        }


        public override string GetExpressionForOutput(int outputIndex)
        {
            if (outputIndex == 0)
            {
                return string.Format("InPosition.xyz");
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
