
using OpenTK.Graphics.OpenGL;
using FreeImageAPI;
using Core.Texture;

namespace Core.Texture
{
    public class Texture2D : TextureBase
    {
        public Texture2D()
            : base()
        {
        }

        public void BindAtUnit(TextureUnit Unit)
        {
            if(IsValid)
            {
                GL.ActiveTexture(Unit);
                Bind();
                m_TextureUnitBinded = Unit;
            }
        }

        public void Load(string FilePath)
        {
            using (var bitmap = new ScopedFreeImage(FilePath))
            {
                this.BindAtUnit(TextureUnit.Texture0);
                m_Width = bitmap.Width;
                m_Height = bitmap.Height;
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmap.Bytes);
            }
        }

        TextureUnit m_TextureUnitBinded;
    }
}
