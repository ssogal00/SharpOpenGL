
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

        public override void Load(string FilePath)
        {
            using (var bitmap = new ScopedFreeImage(FilePath))
            {
                this.BindAtUnit(TextureUnit.Texture0);
                m_Width = bitmap.Width;
                m_Height = bitmap.Height;
                GL.TexImage2D(TextureTarget.Texture2D, 0, bitmap.ImagePixelInternalFormat, bitmap.Width, bitmap.Height, 0, bitmap.OpenglPixelFormat, PixelType.UnsignedByte, bitmap.Bytes);
            }
        }

        public override void Load(string filePath, PixelInternalFormat internalFormat, PixelFormat pixelFormat)
        {
            using (var bitmap = new ScopedFreeImage(filePath))
            {
                this.BindAtUnit(TextureUnit.Texture0);
                m_Width = bitmap.Width;
                m_Height = bitmap.Height;
                GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, m_Width, m_Height, 0, pixelFormat, PixelType.UnsignedByte, bitmap.Bytes);
            }
        }

        public override void Load(byte[] data, int width, int height, PixelInternalFormat internalFormat, PixelFormat pixelFormat)
        {
            this.BindAtUnit(TextureUnit.Texture0);
            m_Width = width;
            m_Height = height;
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, m_Width, m_Height, 0, pixelFormat, PixelType.UnsignedByte, data);
        }
        
        

        TextureUnit m_TextureUnitBinded;
    }
}
