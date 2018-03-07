using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;

namespace SharpOpenGL.Light
{
    public class DirectionalLight : LightBase
    {
        public Vector3 LightDirection
        {
            get;
            set;
        }

        protected Vector3 m_Direction;
    }
}
