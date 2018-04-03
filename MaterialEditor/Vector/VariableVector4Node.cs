using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace MaterialEditor
{
    public class VariableVector4Node : NodeViewModel
    {
        protected OpenTK.Vector4 vec4;

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();

            this.InputConnectors.Add(new ConnectorViewModel("X", ConnectorDataType.ConstantFloat,0));
            this.InputConnectors.Add(new ConnectorViewModel("Y", ConnectorDataType.ConstantFloat,1));
            this.InputConnectors.Add(new ConnectorViewModel("Z", ConnectorDataType.ConstantFloat,2));
            this.InputConnectors.Add(new ConnectorViewModel("W", ConnectorDataType.ConstantFloat,3));

            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector4,0));
        }

        public VariableVector4Node()
            : base("Variable Vector4")
        {
        }

        public VariableVector4Node(string name)
            : base(name)
        {
        }

        public override string ToExpression()
        {
            string xExpression = "0";
            string yExpression = "0";
            string zExpression = "0";
            string wExpression = "0";

            if (InputConnectors[0].AttachedConnections.Count == 1)
            {
                xExpression = InputConnectors[0].AttachedConnections[0].SourceNodeModel.ToExpression();
            }

            if (InputConnectors[1].AttachedConnections.Count == 1)
            {
                yExpression = InputConnectors[1].AttachedConnections[0].SourceNodeModel.ToExpression();
            }

            if (InputConnectors[2].AttachedConnections.Count == 1)
            {
                zExpression = InputConnectors[2].AttachedConnections[0].SourceNodeModel.ToExpression();
            }

            if (InputConnectors[3].AttachedConnections.Count == 1)
            {
                wExpression = InputConnectors[3].AttachedConnections[0].SourceNodeModel.ToExpression();
            }


            return string.Format("vec4({0},{1},{2},{3})", xExpression, yExpression, zExpression, wExpression);
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
            get { return vec4.W; }
            set
            {
                vec4.W = value;
                OnPropertyChanged("WValue");
            }
        }

        public Vector4 Vector4Value
        {
            get
            {
                return vec4;
            }

            set
            {
                vec4 = value;

                OnPropertyChanged("Vector4Value");
            }
        }

    }
}
