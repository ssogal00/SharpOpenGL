using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class Vector3UniformVariableNode : NodeViewModel
    {
        static int vec3UniformCount = 0;

        public Vector3UniformVariableNode()
            : base("Uniform Vector3")
        {
            UniformName = string.Format("uniform_vec3_{0}", vec3UniformCount++);
        }

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();

            OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector3, 0));
        }

        public string UniformName
        {
            get;set;
        }

        public override string GetExpressionForOutput(int outputIndex)
        {
            if(outputIndex == 0)
            {
                return UniformName;
            }

            return string.Empty;
        }

    }
}
