using System;
using System.Collections.Generic;
using System.Text;
using Core;

namespace SharpOpenGLCore.UI
{
    public class UIManager : Singleton<UIManager>
    {
        public UIManager()
        {
            RootScreen = new UIScreen();
        }

        public UIScreen RootScreen = null;
    }
}
