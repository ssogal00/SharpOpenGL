using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;

namespace MaterialEditor
{
    public class NodeDragEventArgs  : RoutedEventArgs
    {
        public ICollection nodes = null;

        protected NodeDragEventArgs(RoutedEvent routedEvent ,object source, ICollection nodes)
            : base(routedEvent, source)
        {
            this.nodes = nodes;
        }

        public ICollection Nodes
        {
            get { return nodes; }
        }
    }

    public delegate void NodeDragEventHandler(object sender, NodeDragEventArgs e);

    public class NodeDragStartedEventArgs : NodeDragEventArgs
    {
        internal NodeDragStartedEventArgs(RoutedEvent routedEvent, object source, ICollection nodes)
            : base(routedEvent, source, nodes)
        {
        }

        private bool cancel = false;

        public bool Cancel
        {
            get
            {
                return cancel;
            }
            set
            {
                cancel = value;
            }
        }
    }

    public class NodeDraggingEventArgs : NodeDragEventArgs
    {
        public double horizontalChange = 0;
        public double verticalChange = 0;

        public NodeDraggingEventArgs(RoutedEvent routedEvent, object source, ICollection nodes, double horizontalChange, double verticalChange)
            : base(routedEvent, source, nodes)
        {
            this.horizontalChange = horizontalChange;
            this.verticalChange = verticalChange;
        }

        public double HorizontalChange
        {
            get { return horizontalChange; }
        }

        public double VerticalChange
        {
            get { return verticalChange; }
        }

        public delegate void NodeDraggingEventHandler(object sender, NodeDraggingEventArgs e);

        /// <summary>
        /// Arguments for event raised when the user has completed dragging a node in the network.
        /// </summary>
        public class NodeDragCompletedEventArgs : NodeDragEventArgs
        {
            public NodeDragCompletedEventArgs(RoutedEvent routedEvent, object source, ICollection nodes) :
                base(routedEvent, source, nodes)
            {
            }
        }

        /// <summary>
        /// Defines the event handler for NodeDragCompleted events.
        /// </summary>
        public delegate void NodeDragCompletedEventHandler(object sender, NodeDragCompletedEventArgs e);
    }
}
