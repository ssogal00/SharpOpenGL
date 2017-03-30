using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

using Core;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using System.Drawing;
using System.Drawing.Imaging;

using FreeImageAPI;
using System.Diagnostics;
using System.IO;

namespace Core.Texture
{
    public class Texture2D : IDisposable
    {
        FIBITMAP DIB;

        public Texture2D()
        {
            m_TextureObject = GL.GenTexture();

            m_Sampler = new Sampler();
            m_Sampler.SetMagFilter(TextureMagFilter.Linear);
            m_Sampler.SetMinFilter(TextureMinFilter.Linear);
        }

        public bool IsValid
        {
            get
            {
                return m_TextureObject != -1;
            }
        }

        public void Dispose()
        {
            if(IsValid)
            {
                GL.DeleteTexture(m_TextureObject);
                m_TextureObject = -1;                
            }
        }

        public void Bind()
        {
            if(IsValid)
            {
                GL.BindTexture(TextureTarget.Texture2D, m_TextureObject);
            }
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

        public void BindShader(TextureUnit Unit, int SamplerLoc)
        {
            if(IsValid)
            {
                m_Sampler.BindSampler(m_TextureUnitBinded);                
                GL.Uniform1(SamplerLoc, (int)(m_TextureUnitBinded - TextureUnit.Texture0));
            }
        }

        public void LoadBitmap(string FilePath)
        {
            using(var bitmap = new Bitmap(FilePath))
            {
                var bmpData = bitmap.LockBits(new Rectangle(0,0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, bmpData.Scan0);

                bitmap.UnlockBits(bmpData);
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

            DIB = FreeImage.ConvertTo32Bits(DIB);
            IntPtr Bytes = FreeImage.GetBits(DIB);

            m_Width = (int) FreeImage.GetWidth(DIB);
            m_Height = (int) FreeImage.GetHeight(DIB);

            this.BindAtUnit(TextureUnit.Texture0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, m_Width, m_Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, Bytes);

            FreeImage.Unload(DIB);
        }
        
        public int Width { get { return m_Width; } }
        public int Height { get { return m_Height; } }
        
        protected int m_Width = 0;
        protected int m_Height = 0;

        int m_TextureObject = -1;

        Sampler m_Sampler = null;

        TextureUnit m_TextureUnitBinded;
    }
}
