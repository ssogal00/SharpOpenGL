﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class CosineNode : NodeViewModel
    {
        public CosineNode()
            : base("Cosine")
        {
        }

        public override string ToExpression()
        {
            if (InputConnectors[0].AttachedConnections.Count == 1)
            {
                var expressionA = InputConnectors[0].AttachedConnections[0].SourceConnector.ParentNode.ToExpression();

                return string.Format("cos({0})", expressionA);
            }

            return string.Empty;
        }

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();
            InputConnectors.Add(new ConnectorViewModel("In", ConnectorDataType.ConstantFloat));
            OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantFloat));
        }
    }
}
