using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class ConnectorInput : AbstractModelBase
    {
        public ConnectorInput() { }

        public ConnectorInput(string name)
        {
            this.name = name;
        }

        protected string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }        
    }

    public class FloatDataInput : ConnectorInput
    {
        public FloatDataInput(string name)
            : base(name)
        {
        }

        public FloatDataInput() { }

        protected float floatValue = 0.1f;

        public float FloatValue
        {
            get { return floatValue; }
            set
            {
                floatValue = value;
                OnPropertyChanged("FloatValue");
            }
        }
    }
}
