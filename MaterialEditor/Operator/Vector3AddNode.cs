using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class Vector3AddNode : NodeViewModel
    {
        public Vector3AddNode()
            : base ("Vector3 Add")
        {
        }

        public override string ToExpression()
        {   
            if(InputConnectors[0].AttachedConnections.Count == 1 && InputConnectors[1].AttachedConnections.Count == 1)
            {
                var expressionA = InputConnectors[0].AttachedConnections[0].SourceConnector.ParentNode.ToExpression();
                var expressionB = InputConnectors[1].AttachedConnections[0].SourceConnector.ParentNode.ToExpression();

                return string.Format("{0} + {1}", expressionA, expressionB);
            }

            return string.Empty;
        }

        protected override void CreateInputOutputConnectors()
        {
            this.InputConnectors.Add(new ConnectorViewModel("A", ConnectorDataType.ConstantVector3));
            this.InputConnectors.Add(new ConnectorViewModel("B", ConnectorDataType.ConstantVector3));
            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector3));
        }        
    }
}
