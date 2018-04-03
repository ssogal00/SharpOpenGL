using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class FloatUniformVariableNode : NodeViewModel
    {
        public FloatUniformVariableNode()
            : base("Float Uniform")
        {
            UniformVariableName = "time";
        }

        protected float value = 0;

        public float Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        public string UniformVariableName
        {
            get;set;
        }

      
        public override string GetExpressionForOutput(int outputIndex)
        {
            if(outputIndex == 0)
            {
                return UniformVariableName;
            }

            return string.Empty;
        }

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();
            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantFloat,0));
        }
    }
}
