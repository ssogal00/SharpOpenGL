using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Texture;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class SamplerParameter
    {
        public virtual void SetParameter()
        { }
        protected int ProgramObject = 0;
        protected string Name = "";
    }

    public class Texture2DParameter : SamplerParameter
    {
        public Texture2DParameter(Texture2D texture, int programObject, string name)
        :base()
        {
            this.ProgramObject = programObject;
            this.Name = name;
            this.Texture = texture;
        }

        public override void SetParameter()
        {
            var Location = GL.GetUniformLocation(ProgramObject, Name);

            GL.ActiveTexture(TextureUnit.Texture0 + Location);
            Texture.Bind();
            Texture.BindShader(TextureUnit.Texture0 + Location, Location);
        }

        private Texture2D Texture = null;
    }

    public class CubemapTextureParameter : SamplerParameter
    {
        public CubemapTextureParameter(CubemapTexture texture, int programObject, string name)
        {
            this.ProgramObject = programObject;
            this.Name = name;
            this.Texture = texture;
        }
        public override void SetParameter()
        {
            var Location = GL.GetUniformLocation(ProgramObject, Name);

            GL.ActiveTexture(TextureUnit.Texture0 + Location);
            Texture.Bind();
            Texture.BindShader(TextureUnit.Texture0 + Location, Location);
        }

        protected CubemapTexture Texture = null;
    }
}
