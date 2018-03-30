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
        {
            this.OutputConnectors.Add(new ConnectorViewModel("Out"));
        }

        public ConstantVector2Node(string name)
            : base(name)
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

            set
            {
                vec2 = value;

                OnPropertyChanged("Vector2Value");
            }
        }
    }
}
