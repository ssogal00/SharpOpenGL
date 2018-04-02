using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MaterialEditor
{
    public class ConstantVector4Node : NodeViewModel
    {
        protected OpenTK.Vector4 vec4;

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();

            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector4));
        }

        public ConstantVector4Node()
            : base("Vector4")
        {            
        }

        public ConstantVector4Node(string name)
            : base(name)
        {   
        }

        public override string ToExpression()
        {
            return string.Format("vec4({0}, {1}, {2}, {3})", XValue, YValue, ZValue, WValue);
        }

        public override string ToExpression(ConnectorDataType castType)
        {
            if(castType == ConnectorDataType.ConstantVector3)
            {
                return string.Format("vec3({0},{1},{2})", XValue, YValue, ZValue);
            }
            else if(castType == ConnectorDataType.ConstantVector2)
            {
                return string.Format("vec2({0},{1})", XValue, YValue);
            }

            return string.Empty;
        }

        public float XValue
        {
            get
            {
                return vec4.X;
            }
            set
            {
                vec4.X = value;
                OnPropertyChanged("XValue");
            }
        }

        public float YValue
        {
            get
            {
                return vec4.Y;
            }
            set
            {
                vec4.Y = value;
                OnPropertyChanged("YValue");
            }
        }

        public float ZValue
        {
            get { return vec4.Z; }
            set
            {
                vec4.Z = value;
                OnPropertyChanged("ZValue");
            }
        }



        public float WValue
        {
            get
            {
                return vec4.W;
            }

            set
            {
                vec4.W = value;
                OnPropertyChanged("WValue");
            }
        }
    }
}
