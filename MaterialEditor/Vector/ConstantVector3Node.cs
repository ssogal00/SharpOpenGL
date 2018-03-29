using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MaterialEditor
{
    public class ConstantVector3Node : NodeViewModel
    {
        protected OpenTK.Vector3 vec3;

        public ConstantVector3Node()
        {
            this.OutputConnectors.Add(new ConnectorViewModel("Out"));
        }

        public ConstantVector3Node(string name)
            : base(name)
        {
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
