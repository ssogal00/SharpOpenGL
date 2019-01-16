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
    }

    public class Texture2DParameter : SamplerParameter
    {
        public Texture2DParameter(Texture2D texture, int location)
        :base()
        {   
        }

        public override void SetParameter()
        {
            GL.ActiveTexture(TextureUnit.Texture0 + Location);
            Texture.Bind();
            Texture.BindShader(TextureUnit.Texture0 + Location, Location);
        }

        private Texture2D Texture = null;
        private int Location = 0;
    }
}
