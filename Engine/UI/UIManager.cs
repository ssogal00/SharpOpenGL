using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using System.Text;
using Core;
using Core.Texture;
using OpenTK.Graphics.OpenGL;

using FreeTypeWrapper;

namespace SharpOpenGLCore.UI
{
    public class UIManager : Singleton<UIManager>
    {
        public UIManager()
        {
            RootScreen = new UIScreen();

            // RootScreen.AddChild(new UIBox(Vector2.Zero, 64*4,64*4));
            RootScreen.AddChild(new UIText("abcdef", new Vector2i(0, 200)));
            mFontAtlas = FreeType.GetFontAtlas(null, "abcdefghijklmn", 64);
        }

        public void RenderUI()
        {
            RootScreen.Render();
        }

        public UIScreen RootScreen = null;

        private FontAtlas mFontAtlas;

        public FontAtlas FontAtlas
        {
            get
            {
                return mFontAtlas;
            }
        }
    }
}
