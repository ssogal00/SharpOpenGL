using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public partial class NetworkView
    {
        private void NodeItem_DragStarted(object source, NodeDragStartedEventArgs e)
        {
            e.Handled = true;

            var eventArgs = new NodeDragStartedEventArgs(NodeDragStartedEvent,this, this.SelectedNodes);
            RaiseEvent(eventArgs);

            e.Cancel = eventArgs.Cancel;
        }
    }
}
