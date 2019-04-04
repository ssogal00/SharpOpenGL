using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace Core.Texture
{
    public class ColorAttachmentTexture : TextureBase
    {
        /*public ColorAttachmentTexture(int widthParam, int heightParam, PixelInternalFormat format = PixelInternalFormat.Rgba16f)
        {   
            m_Width = widthParam;
            m_Height = heightParam;
            textureFormat = format;
        }*/

        public ColorAttachmentTexture(int widthParam, int heightParam, PixelInternalFormat format, PixelFormat pixelFormat, PixelType pixelType)
        {
            m_Width = widthParam;
            m_Height = heightParam;
            textureFormat = format;
            this.pixelType = pixelType;
        }

        protected virtual void RecreateTexture()
        {
            if (textureObject != 0)
            {
                GL.DeleteTexture(textureObject);
                textureObject = GL.GenTexture();
            }
        }

        public void SaveAsBmp(string filename)
        {
            var colorData = GetTexImageAsByte();
            FreeImageHelper.SaveAsBmp(ref colorData, this.Width, this.Height, filename);
        }

        public override void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, textureObject);            
        }

        protected virtual void Alloc()
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, textureFormat, m_Width, m_Height, 0, PixelFormat.Rgba, PixelType.Float, new IntPtr(0));
        }

        public virtual void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            RecreateTexture();
            m_Width = newWidth;
            m_Height = newHeight;
            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, textureFormat, m_Width, m_Height, 0, PixelFormat.Rgba, PixelType.Float, new IntPtr(0));
        }

        public override byte[] GetTexImageAsByte()
        {
            Bind();
            var data = new byte[m_Width * m_Height * 4];
            GL.GetTexImage<byte>(TextureTarget.Texture2D, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data);
            Unbind();
            return data;
        }

        public override float[] GetTexImageAsFloat()
        {
            Bind();
            var data = new float[m_Width * m_Height * 4];
            GL.GetTexImage<float>(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.Float, data);
            Unbind();
            return data;
        }


        protected PixelInternalFormat textureFormat;
        public PixelInternalFormat TextureFormat => textureFormat;

        protected PixelType pixelType = PixelType.Float;
        public PixelType TexturePixelType => pixelType;

        protected PixelFormat pixelFormat = PixelFormat.Rgba;
        public PixelFormat TexturePixelFormat => pixelFormat;

        public int GetTextureObject => textureObject;        
    }
}
