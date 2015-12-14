using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL
{
    public class VertexAttribute
    {
        public VertexAttribute(int nIndex, int nSize, ActiveAttribType _AttributeType)
        {
            Index   = nIndex;
            Size    = nSize;
            AttributeType = _AttributeType;
        }

        public int Index { get; set; }
        public int Size { get; set; }
        public ActiveAttribType AttributeType { get; set; }
    }
}
