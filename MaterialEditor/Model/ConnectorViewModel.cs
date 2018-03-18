using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class ConnectorViewModel : AbstractModelBase
    {
        public ConnectorViewModel(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
