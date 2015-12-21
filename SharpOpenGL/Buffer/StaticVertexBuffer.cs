using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL.Buffer 
{
    public class StaticVertexBuffer<T> : OpenGLBuffer where T : struct
    {
        public StaticVertexBuffer()
        {
            m_BufferTarget = BufferTarget.ArrayBuffer;
            m_Hint = BufferUsageHint.StaticDraw;
        }
        
        public void VertexAttribPointer(T[] Data)
        {
            var VertexAttrType = typeof(T);

            var fields = VertexAttrType.GetFields();

            for(int index = 0; index < fields.Count(); ++index)
            {
                var CustomAttributeDic = fields[index].CustomAttributes.ToDictionary(x=>x.AttributeType.Name, x => x.ConstructorArguments[0]);

                var nComponentCount = Convert.ToInt32(CustomAttributeDic["ComponentCount"].Value);
                var nOffset = Convert.ToInt32(CustomAttributeDic["FieldOffsetAttribute"].Value);

                GL.EnableVertexAttribArray(index);
                GL.VertexAttribPointer(index, nComponentCount, VertexAttribPointerType.Float, false, VertexAttrType.StructLayoutAttribute.Size, new IntPtr(nOffset));
            }
        }
    }
}
