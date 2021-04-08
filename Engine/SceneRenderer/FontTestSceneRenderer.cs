using Core.Texture;
using Engine.Scene;
using FreeTypeGLWrapper;
using OpenTK.Graphics.OpenGL;

namespace Engine.SceneRenderer
{
    class FontTestSceneRenderer : SceneRendererBase
    {
        public FontTestSceneRenderer(SceneBase scene)
        : base(scene)
        {

        }
        public override void Initialize()
        {
            ManagedTextureAtlas result = FreeTypeGL.GenerateTextureAtlas(512, 512, 72, "./Resources/Fonts/Vera.ttf");
            mFontAtlas = new Texture2D();
            mFontAtlas.LoadFromMemory(result.data, 512, 512, PixelInternalFormat.R8, PixelFormat.Red);
            mScreenBlit = new BlitToScreen();
            mScreenBlit.SetGridSize(1,1);

            mFontRender = new FontRender();
        }

        public override void RenderScene(SceneBase scene)
        {
            mFontRender.Render(mFontAtlas);
        }

        private Texture2D mFontAtlas = null;
        private BlitToScreen mScreenBlit;
        private FontRender mFontRender;
    }
}
