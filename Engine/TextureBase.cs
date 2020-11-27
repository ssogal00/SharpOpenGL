
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
                case DXGI_FORMAT.DXGI_FORMAT_BC5_SNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC5_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_BC5_UNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC3_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_BC3_UNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC3_UNORM_SRGB:
                    
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


        public static bool IsCompressed(DXGI_FORMAT fmt)
        {
            switch (fmt)
            {
                case DXGI_FORMAT.DXGI_FORMAT_BC1_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_BC1_UNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC1_UNORM_SRGB:
                case DXGI_FORMAT.DXGI_FORMAT_BC2_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_BC2_UNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC2_UNORM_SRGB:
                case DXGI_FORMAT.DXGI_FORMAT_BC3_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_BC3_UNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC3_UNORM_SRGB:
                case DXGI_FORMAT.DXGI_FORMAT_BC4_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_BC4_UNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC4_SNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC5_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_BC5_UNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC5_SNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC6H_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_BC6H_UF16:
                case DXGI_FORMAT.DXGI_FORMAT_BC6H_SF16:
                case DXGI_FORMAT.DXGI_FORMAT_BC7_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_BC7_UNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC7_UNORM_SRGB:
                    return true;

                default:
                    return false;
            }
        }

        public static InternalFormat ToInternalFormat(DXGI_FORMAT format)
        {
            switch (format)
            {
                // r32g32b32a32
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_FLOAT:
                    return InternalFormat.Rgba32f;
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_UINT:
                    return InternalFormat.Rgba32ui;
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_SINT:
                    return InternalFormat.Rgba32i;

                // r16g16b16a16
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_FLOAT:
                    return InternalFormat.Rgba16f;
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_UINT:
                    return InternalFormat.Rgba16ui;
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_SINT:
                    return InternalFormat.Rgba16i;
                //case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_SNORM:
                //    return InternalFormat.Rgba16Snorm;
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_UNORM:
                    return InternalFormat.Rgba16;
                case DXGI_FORMAT.DXGI_FORMAT_R16G16B16A16_TYPELESS:
                    return InternalFormat.Rgba16;

                // r32g32b32
                //case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_FLOAT:
                //    return InternalFormat.Rgb32f;
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_SINT:
                    return InternalFormat.Rgb32i;
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_UINT:
                    return InternalFormat.Rgb32ui;
                case DXGI_FORMAT.DXGI_FORMAT_R32G32B32_TYPELESS:
                    return InternalFormat.Rgb;

                // r8g8b8a8
                case DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_SINT:
                    return InternalFormat.Rgba8i;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UINT:
                    return InternalFormat.Rgba8ui;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_SNORM:
                    return InternalFormat.Rgba8Snorm;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM:
                    return InternalFormat.Rgba;

                // r8g8
                case DXGI_FORMAT.DXGI_FORMAT_R8G8_SINT:
                    return InternalFormat.Rg8i;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8_UINT:
                    return InternalFormat.Rg8ui;

                case DXGI_FORMAT.DXGI_FORMAT_BC5_SNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC5_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_BC5_UNORM:
                    return InternalFormat.CompressedRgbaS3tcDxt5Ext;
            }

            return InternalFormat.Rgba;
        }

        public static PixelInternalFormat ToPixelInternalFormat(DXGI_FORMAT format)
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

                // r8g8
                case DXGI_FORMAT.DXGI_FORMAT_R8G8_SINT:
                    return PixelInternalFormat.Rg8i;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8_UINT:
                    return PixelInternalFormat.Rg8ui;

                case DXGI_FORMAT.DXGI_FORMAT_BC5_SNORM:
                case DXGI_FORMAT.DXGI_FORMAT_BC5_TYPELESS:
                case DXGI_FORMAT.DXGI_FORMAT_BC5_UNORM:
                    return PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;
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
