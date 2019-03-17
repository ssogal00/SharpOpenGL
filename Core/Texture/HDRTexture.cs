
using OpenTK.Graphics.OpenGL;
using FreeImageAPI;
using Core.Texture;
using ImageLibWrapper;

namespace Core.Texture
{
    public class HDRTexture : TextureBase
    {
        public HDRTexture()
            : base()
        {
        }

        public void BindAtUnit(TextureUnit Unit)
        {
            if (IsValid)
            {
                GL.ActiveTexture(Unit);
                Bind();
            }
        }

        public override void Load(string FilePath)
        {
            using (var bitmap = new ScopedSTBImage(FilePath))
            {
                this.BindAtUnit(TextureUnit.Texture0);
                m_Width = bitmap.Width;
                m_Height = bitmap.Height;
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f, bitmap.Width, bitmap.Height, 0, PixelFormat.Rgb, PixelType.Float, bitmap.Data);
            }
        }
    }
}
