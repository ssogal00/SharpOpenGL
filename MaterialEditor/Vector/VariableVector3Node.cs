using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MaterialEditor.Model
{
    public class VariableVector3Node : NodeViewModel
    {
        protected OpenTK.Vector3 vec3;

        public VariableVector3Node(string name)
            : base(name)
        {
            this.InputConnectors.Add(new ConnectorViewModel("X"));
            this.InputConnectors.Add(new ConnectorViewModel("Y"));
            this.InputConnectors.Add(new ConnectorViewModel("Z"));

            this.OutputConnectors.Add(new ConnectorViewModel("Out"));
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
