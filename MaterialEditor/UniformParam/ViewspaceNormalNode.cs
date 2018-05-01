using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.MaterialBase;

namespace MaterialEditor
{
    public class ViewspaceNormalNode : NodeViewModel
    {

        public ViewspaceNormalNode()
            : base("Viewspace Normal")
        {
        }


        public override string GetExpressionForOutput(int outputIndex)
        {
            if(outputIndex == 0)
            {
                return string.Format("{0}.Normal.xyz", MaterialBase.StageInputName);
            }

            return string.Empty;
        }


        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();
            OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector3,0));
        }
    }
}
