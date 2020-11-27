﻿
using OpenTK.Graphics.OpenGL;
using FreeImageAPI;
using Core.Texture;
using DirectXTexWrapper;

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
                GL.TexImage2D(TextureTarget.Texture2D, 0, bitmap.ImagePixelInternalFormat, bitmap.Width, bitmap.Height, 0, bitmap.OpenglPixelFormat, bitmap.OpenglPixelType, bitmap.Bytes);
            }
        }

        public override void LoadFromDDSFile(string path)
        {
            var scratchImage = DXTLoader.LoadFromDDSFile(path);
            
            if (scratchImage != null)
            {
                this.BindAtUnit(TextureUnit.Texture0);
                m_Width = (int)scratchImage.m_metadata.width;
                m_Height = (int) scratchImage.m_metadata.height;
                m_MipCount = (int)scratchImage.m_metadata.mipLevels;

                var pixelInternalFormat = ToPixelInternalFormat(scratchImage.m_metadata.format);
                var pixelFormat = ToPixelFormat(scratchImage.m_metadata.format);
                var pixelType = ToPixelType(scratchImage.m_metadata.format);
                var internalFormat = ToInternalFormat(scratchImage.m_metadata.format);

                if (IsCompressed(scratchImage.m_metadata.format))
                {
                    int blocksize = 0;

                    if (internalFormat == InternalFormat.CompressedRgbaS3tcDxt3Ext)
                    {
                        blocksize = 16;
                    }
                    else if (internalFormat == InternalFormat.CompressedRgbaS3tcDxt5Ext)
                    {
                        blocksize = 16;
                    }
                    else if (internalFormat == InternalFormat.CompressedRgbaS3tcDxt1Ext)
                    {
                        blocksize = 8;
                    }

                    int imageSize = ((m_Width + 3) / 4) * ((m_Height + 3) / 4) * blocksize;
                    
                    GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, internalFormat, m_Width, m_Height, 0, imageSize, scratchImage.m_image[0].pixels);
                }
                else
                {
                    GL.TexImage2D(TextureTarget.Texture2D, 0, pixelInternalFormat, m_Width, m_Height, 0, pixelFormat, pixelType, scratchImage.m_image[0].pixels);
                }
            }
        }

        public override void LoadFromTGAFile(string path)
        {
            var scratchImage = DXTLoader.LoadFromTGAFile(path);

            if (scratchImage != null)
            {
                this.BindAtUnit(TextureUnit.Texture0);
                m_Width = (int)scratchImage.m_metadata.width;
                m_Height = (int)scratchImage.m_metadata.height;

                var pixelInternalFormat = ToPixelInternalFormat(scratchImage.m_metadata.format);
                var pixelFormat = ToPixelFormat(scratchImage.m_metadata.format);
                var pixelType = ToPixelType(scratchImage.m_metadata.format);

                GL.TexImage2D(TextureTarget.Texture2D, 0, pixelInternalFormat, m_Width, m_Height, 0, pixelFormat, pixelType, scratchImage.m_image[0].pixels);
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

        public override void Load(float[] data, int width, int height, PixelInternalFormat internalFormat, PixelFormat pixelFormat)
        {
            this.BindAtUnit(TextureUnit.Texture0);
            m_Width = width;
            m_Height = height;
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, m_Width, m_Height, 0, pixelFormat, PixelType.Float, data);
        }



        TextureUnit m_TextureUnitBinded;
    }
}
