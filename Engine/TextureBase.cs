
using OpenTK.Graphics.OpenGL;
using System;
using DirectXTexWrapper;


namespace Core.Texture
{
    public class TextureBase : IDisposable
    {
        public TextureBase()
        {
            textureObject = GL.GenTexture();
        }

        public void Dispose()
        {
            if (IsValid)
            {
                GL.DeleteTexture(textureObject);
                textureObject = -1;
            }
        }

        public virtual void Load(string path)
        {
        }

        public virtual void Load(string path, PixelInternalFormat internalFormat, PixelFormat pixelFormat)
        {
        }

        public virtual void Load(byte[] data, int width, int height, PixelInternalFormat internalFormat, PixelFormat pixelFormat)
        { }
        public virtual void Load(float[] data, int width, int height, PixelInternalFormat internalFormat, PixelFormat pixelFormat)
        { }

        public virtual void LoadFromDDSFile(string path)
        {
        }

        public virtual byte[] GetTexImageAsByte()
        {
            return null;
        }

        public virtual float[] GetTexImageAsFloat()
        {
            return null;
        }

        public virtual void Bind()
        {
            if (IsValid)
            {
                GL.BindTexture(TextureTarget.Texture2D, textureObject);
            }
        }

        public virtual void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }


        public virtual void BindShader(TextureUnit Unit, int SamplerLoc)
        {
            if (IsValid)
            {
                GL.Uniform1(SamplerLoc, (int)(Unit - TextureUnit.Texture0));
            }
        }

        public static PixelType ToPixelType(DXGI_FORMAT format)
        {
            switch (format)
            {
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_FLOAT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_FLOAT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32_FLOAT:
                case DXGI_FORMAT.DXGI_FORMAT_R32_FLOAT:
                    return PixelType.Float;

                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_UINT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_UINT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32_UINT:
                case DXGI_FORMAT.DXGI_FORMAT_R32_UINT:
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_UINT:
                case DXGI_FORMAT.DXGI_FORMAT_R16G16_UINT:
                case DXGI_FORMAT.DXGI_FORMAT_R16_UINT:
                    return PixelType.UnsignedInt;

                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_FLOAT:
                case DXGI_FORMAT.DXGI_FORMAT_R16G16_FLOAT:
                case DXGI_FORMAT.DXGI_FORMAT_R16_FLOAT:
                    return PixelType.HalfFloat;

                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_SINT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_SINT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32_SINT:
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_SINT:
                case DXGI_FORMAT.DXGI_FORMAT_R16G16_SINT:
                case DXGI_FORMAT.DXGI_FORMAT_R16_SINT:
                    return PixelType.Int;

                case DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM:
                    return PixelType.UnsignedByte;
            }

            return PixelType.UnsignedByte;
        }

        public static PixelFormat ToPixelFormat(DXGI_FORMAT format)
        {
            switch (format)
            {
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_FLOAT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_FLOAT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_UINT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_SINT:
                case DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_SINT:
                    return PixelFormat.Rgba;
                
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_FLOAT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_SINT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_TYPELESS:
                    return PixelFormat.Rgb;

                case DXGI_FORMAT.DXGI_FORMAT_R32G32_FLOAT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32_SINT:
                case DXGI_FORMAT.DXGI_FORMAT_R32G32_UINT:
                case DXGI_FORMAT.DXGI_FORMAT_R16G16_FLOAT:
                case DXGI_FORMAT.DXGI_FORMAT_R16G16_SINT:
                case DXGI_FORMAT.DXGI_FORMAT_R16G16_UINT:
                    return PixelFormat.Rg;

                case DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM:
                case DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM_SRGB:
                case DXGI_FORMAT.DXGI_FORMAT_B4G4R4A4_UNORM:
                    return PixelFormat.Bgra;
            }

            return PixelFormat.Bgra;
        }

        public static PixelInternalFormat ToInternalFormat(DXGI_FORMAT format)
        {
            switch (format)
            {
                // r32g32b32a32
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_FLOAT:
                    return PixelInternalFormat.Rgba32f;
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_UINT:
                    return PixelInternalFormat.Rgba32ui;
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_SINT:
                    return PixelInternalFormat.Rgba32i;

                // r16g16b16a16
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_FLOAT:
                    return PixelInternalFormat.Rgba16f;
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_UINT:
                    return PixelInternalFormat.Rgba16ui;
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_SINT:
                    return PixelInternalFormat.Rgba16i;
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_SNORM:
                    return PixelInternalFormat.Rgba16Snorm;
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_UNORM:
                    return PixelInternalFormat.Rgba16;
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_TYPELESS:
                    return PixelInternalFormat.Rgba16;

                // r32g32b32
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_FLOAT:
                    return PixelInternalFormat.Rgb32f;
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_SINT:
                    return PixelInternalFormat.Rgb32i;
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_UINT:
                    return PixelInternalFormat.Rgb32ui;
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_TYPELESS:
                    return PixelInternalFormat.Rgb;

                // r8g8b8a8
                case DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_SINT:
                    return PixelInternalFormat.Rgba8i;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UINT:
                    return PixelInternalFormat.Rgba8ui;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_SNORM:
                    return PixelInternalFormat.Rgba8Snorm;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM:
                    return PixelInternalFormat.Rgba;
            }

            return PixelInternalFormat.CompressedRgba;
        }

        public bool IsValid
        {
            get
            {
                return textureObject != -1;
            }
        }

        public int Width { get { return m_Width; } }
        public int Height { get { return m_Height; } }
        public int TextureObject { get { return textureObject; } }

        protected int m_Width = 0;
        protected int m_Height = 0;
        protected int textureObject = -1;

        protected Sampler m_Sampler = null;
        protected string m_TextureName = "";
    }
}
