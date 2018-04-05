using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class ResultNode : NodeViewModel
    {
        public ResultNode()
            : base("Material")
        { }

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();

            // input
            InputConnectors.Add(new ConnectorViewModel("Albedo", ConnectorDataType.ConstantVector4,0));
            InputConnectors.Add(new ConnectorViewModel("Normal", ConnectorDataType.ConstantVector4,1));
            InputConnectors.Add(new ConnectorViewModel("Specular", ConnectorDataType.ConstantVector4,2));
        }

        public string GetDiffuseColorCode()
        {
            return GetExpressionForInput(0);
        }

        public string GetNormalColorCode()
        {
            return GetExpressionForInput(1);
        }

        public string GetSpecularColorCode()
        {
            return GetExpressionForInput(2);
        }

    }
}
