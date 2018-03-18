using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialEditor.Utils;

namespace MaterialEditor
{
    public class NodeViewModel : AbstractModelBase
    {
        private string name = string.Empty;

        private double x = 0;

        private double y = 0;

        private int zIndex = 0;

        public string Name
        {
            get { return name; }
            set
            {
                if(name == value)
                {
                    return;
                }

                name = value;

                OnPropertyChanged("Name");
            }
        }

        public double X
        {
            get { return x; }
            set
            {
                if (x == value)
                {
                    return;
                }

                x = value;

                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get { return y; }
            set
            {
                if (y == value)
                {
                    return;
                }

                y = value;

                OnPropertyChanged("Y");
            }
        }

        public int ZIndex
        {
            get { return zIndex; }
            set
            {
                if(zIndex == value)
                {
                    return;
                }

                zIndex = value;

                OnPropertyChanged("ZIndex");
            }
        }


        private ImpObservableCollection<ConnectorViewModel> inputConnectors = null;

        private ImpObservableCollection<ConnectorViewModel> outputConnectors = null;

        public ImpObservableCollection<ConnectorViewModel> OutputConnectors
        {
            get
            {
                if (outputConnectors == null)
                {
                    outputConnectors = new ImpObservableCollection<ConnectorViewModel>();
                }

                return outputConnectors;
            }
        }

        public ImpObservableCollection<ConnectorViewModel> InputConnectors
        {
            get
            {
                if(inputConnectors == null)
                {
                    inputConnectors = new ImpObservableCollection<ConnectorViewModel>();
                }

                return inputConnectors;
            }
        }
    }
}
