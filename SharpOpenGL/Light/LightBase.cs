using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;

namespace SharpOpenGL.Light
{
    public class LightBase
    {
        public Vector3 AmbientColor => m_AmbientColor;
        public Vector3 DiffuseColor => m_DiffuseColor;
        public Vector3 SpecularColor => m_SpecularColor;


        protected Vector3 m_AmbientColor;
        protected Vector3 m_DiffuseColor;
        protected Vector3 m_SpecularColor;
    }
}
