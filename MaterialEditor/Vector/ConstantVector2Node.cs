using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace MaterialEditor
{
    public class ConstantVector2Node : NodeViewModel
    {
        protected OpenTK.Vector2 vec2;

        public ConstantVector2Node()
            : base("Vector2")
        {   
        }

        public ConstantVector2Node(string name)
            : base(name)
        {            
        }

        public override string GetExpressionForOutput(int outputIndex)
        {
            if(outputIndex == 0)
            {
                return string.Format("vec2({0},{1})", XValue, YValue);
            }

            return string.Empty;
        }

        protected override void CreateInputOutputConnectors()
        {
            this.OutputConnectors.Add(new ConnectorViewModel("Out"));
        }

        public float XValue
        {
            get
            {
                return vec2.X;
            }
            set
            {
                vec2.X = value;
                OnPropertyChanged("XValue");
            }
        }

        public float YValue
        {
            get
            {
                return vec2.Y;
            }
            set
            {
                vec2.Y = value;
                OnPropertyChanged("YValue");
            }
        }
        

        public Vector2 Vector2Value
        {
            get
            {
                return vec2;
            }
        }
    }
}
