using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class Vector2AddNode : NodeViewModel
    {
        public Vector2AddNode()
            : base("Vector2 Add")
        {
        }

        public override string ToExpression()
        {
            if (InputConnectors[0].IsConnectionAttached && InputConnectors[1].IsConnectionAttached)
            {
                var expressionA = InputConnectors[0].AttachedConnections[0].SourceConnector.ParentNode.ToExpression();

                if(InputConnectors[0].AttachedConnections[0].SourceConnector.DataType != 
                    InputConnectors[0].AttachedConnections[0].DestConnector.DataType)
                {
                    if(ConnectionHelper.SupportsCast(
                        InputConnectors[0].AttachedConnections[0].SourceConnector.DataType,
                        InputConnectors[0].AttachedConnections[0].DestConnector.DataType))
                    {
                        
                    }
                }

                var expressionB = InputConnectors[1].AttachedConnections[0].SourceConnector.ParentNode.ToExpression();

                return string.Format("{0} + {1}", expressionA, expressionB);
            }

            return string.Empty;
        }

        protected override void CreateInputOutputConnectors()
        {
            // input 
            this.InputConnectors.Add(new ConnectorViewModel("A", ConnectorDataType.ConstantVector2, 0));
            this.InputConnectors.Add(new ConnectorViewModel("B", ConnectorDataType.ConstantVector2, 1));

            // output
            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector2, 0));
        }
    }
}
