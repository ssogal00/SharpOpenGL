using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using System.Text;
using Core;
using OpenTK.Graphics.OpenGL;


namespace SharpOpenGLCore.UI
{
    public class UIManager : Singleton<UIManager>
    {
        public UIManager()
        {
            RootScreen = new UIScreen();

            RootScreen.AddChild(new UIBox(Vector2.Zero, 600,600));
            RootScreen.AddChild(new UIBox(new Vector2(100,100),600,600 ));
        }

        public void RenderUI()
        {
            RootScreen.Render();
        }

        public UIScreen RootScreen = null;
    }
}
