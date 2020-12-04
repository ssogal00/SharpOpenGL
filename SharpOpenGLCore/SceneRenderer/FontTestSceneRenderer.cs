using System;
using System.Collections.Generic;
using System.Text;
using Core.Texture;
using SharpOpenGL.Scene;
using FreeTypeGLWrapper;
using OpenTK.Graphics.OpenGL;
using SharpOpenGL;

namespace SharpOpenGLCore.SceneRenderer
{
    class FontTestSceneRenderer : SceneRendererBase
    {
        public override void Initialize()
        {
            ManagedTextureAtlas result = FreeTypeGL.GenerateTextureAtlas(512, 512, 72, "./Resources/Fonts/Vera.ttf");
            mFontAtlas = new Texture2D();
            mFontAtlas.LoadFromMemory(result.data, 512, 512, PixelInternalFormat.R8, PixelFormat.Red);
            mScreenBlit = new BlitToScreen();
            mScreenBlit.SetGridSize(1,1);
        }

        public override void RenderScene(SceneBase scene)
        {
            mScreenBlit.Blit(mFontAtlas, 0, 0, 1, 1);
        }

        private Texture2D mFontAtlas = null;
        private BlitToScreen mScreenBlit;
    }
}
