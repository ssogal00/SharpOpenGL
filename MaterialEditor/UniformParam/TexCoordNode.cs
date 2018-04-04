using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor.UniformParam
{
    public class TexCoordNode : NodeViewModel
    {

        public TexCoordNode()
            : base("TexCoord")
        {
        }

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();
            OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector2, 0));
        }

        public override string GetExpressionForOutput(int outputIndex)
        {
            if (outputIndex == 0)
            {
                return "InTexCoord.xy";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
