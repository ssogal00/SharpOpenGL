using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using ZeroFormatter;
using ZeroFormatter.Formatters;
using ZeroFormatter.Internal;

namespace MaterialEditor
{
    [ZeroFormattable]
    public class ConstantVector3Node : NodeViewModel
    {
        protected OpenTK.Vector3 vec3;

        public ConstantVector3Node()
        {
            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector3,0));
        }

        public ConstantVector3Node(string name)
            : base(name)
        {
            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector3,0));
        }
        
        public override string GetExpressionForOutput(int outputIndex)
        {
            if (outputIndex == 0)
            {
                return string.Format("vec3({0}, {1}, {2})", XValue, YValue, ZValue);
            }
            return string.Empty;
        }

        [Index(0)]
        public virtual float XValue
        {
            get
            {
                return vec3.X;
            }
            set
            {
                vec3.X = value;
                OnPropertyChanged("XValue");
            }
        }
        [Index(1)]
        public virtual float YValue
        {
            get
            {
                return vec3.Y;
            }
            set
            {
                vec3.Y = value;
                OnPropertyChanged("YValue");
            }
        }
        [Index(2)]
        public virtual float ZValue
        {
            get { return vec3.Z; }
            set
            {
                vec3.Z = value;
                OnPropertyChanged("ZValue");
            }
        }

        [Index(3)]
        public Vector3 Vector3Value
        {
            get
            {
                return vec3;
            }            
        }
    }
}
