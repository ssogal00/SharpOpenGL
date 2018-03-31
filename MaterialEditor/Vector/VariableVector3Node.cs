using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MaterialEditor
{
    public class VariableVector3Node : NodeViewModel
    {
        protected OpenTK.Vector3 vec3;

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();
            this.InputConnectors.Add(new ConnectorViewModel("X", ConnectorDataType.ConstantFloat));
            this.InputConnectors.Add(new ConnectorViewModel("Y", ConnectorDataType.ConstantFloat));
            this.InputConnectors.Add(new ConnectorViewModel("Z", ConnectorDataType.ConstantFloat));
            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector3));
        }

        public VariableVector3Node()
            : base("Variable Vector3")
        {   
        }

        public VariableVector3Node(string name)
            : base(name)
        {
        }

        public override string ToExpression()
        {
            if(InputConnectors[0].AttachedConnections.Count == 1)
            {
                var xExpression = InputConnectors[0].AttachedConnections[0].SourceConnector.ParentNode.ToExpression();

                return string.Format("vec3({0}, 0, 0)", xExpression);
            }

            return string.Empty;
        }

        public float XValue
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

        public float YValue
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

        public float ZValue
        {
            get { return vec3.Z; }
            set
            {
                vec3.Z = value;
                OnPropertyChanged("ZValue");
            }
        }

        public Vector3 Vector3Value
        {
            get
            {
                return vec3;
            }

            set
            {
                vec3 = value;

                OnPropertyChanged("Vector3Value");
            }
        }

    }
}
