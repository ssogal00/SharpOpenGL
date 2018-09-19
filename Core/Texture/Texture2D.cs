using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using FreeImageAPI;
using System.IO;

namespace Core.Texture
{
    public class Texture2D : TextureBase
    {
        FIBITMAP DIB;

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
            if(!File.Exists(FilePath))
            {
                return;
            }

            FREE_IMAGE_FORMAT FileType = FreeImageAPI.FreeImage.GetFileType(FilePath, 0);

            if(FileType == FREE_IMAGE_FORMAT.FIF_UNKNOWN)
            {
                FileType = FreeImage.GetFIFFromFilename(FilePath);
            }

            if(FileType == FREE_IMAGE_FORMAT.FIF_UNKNOWN)
            {
                return;
            }

            if(FreeImage.FIFSupportsReading(FileType))
            {
                DIB = FreeImage.Load(FileType, FilePath, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            }

            if(DIB == null)
            {
                return;
            }

            var Format = FreeImage.GetPixelFormat(DIB);

            DIB = FreeImage.ConvertTo32Bits(DIB);
            IntPtr Bytes = FreeImage.GetBits(DIB);

            m_Width = (int) FreeImage.GetWidth(DIB);
            m_Height = (int) FreeImage.GetHeight(DIB);

            this.BindAtUnit(TextureUnit.Texture0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, m_Width, m_Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, Bytes);

            FreeImage.Unload(DIB);
        }

        TextureUnit m_TextureUnitBinded;
    }
}
