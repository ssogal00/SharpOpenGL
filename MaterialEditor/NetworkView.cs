using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Specialized;
using MaterialEditor.Utils;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Input;


namespace MaterialEditor
{
    public class NetworkView : Control
    {
        public static readonly DependencyProperty NodesSourceProperty =
            DependencyProperty.Register("NodesSource", typeof(IEnumerable), typeof(NetworkView), new FrameworkPropertyMetadata());

    }
}
