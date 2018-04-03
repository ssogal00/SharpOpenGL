using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor.UniformParam
{
    public class ViewspaceNormalNode : NodeViewModel
    {

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();
            OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector4,0));
        }
    }
}
