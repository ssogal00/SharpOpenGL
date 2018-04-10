using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Buffer
{
    public class DynamicVertexBuffer<T> : OpenGLBuffer where T : struct
    {
        public DynamicVertexBuffer()
        {
            bufferTarget = BufferTarget.ArrayBuffer;
            hint = BufferUsageHint.DynamicDraw;
        }

        public void BindVertexAttribute()
        {
            var VertexAttrType = typeof(T);

            var fields = VertexAttrType.GetFields();

            for (int index = 0; index < fields.Count(); ++index)
            {
                var CustomAttributeDic = fields[index].CustomAttributes.ToDictionary(x => x.AttributeType.Name, x => x.ConstructorArguments[0]);

                var nComponentCount = Convert.ToInt32(CustomAttributeDic["ComponentCount"].Value);
                var nOffset = Convert.ToInt32(CustomAttributeDic["FieldOffsetAttribute"].Value);
                var eType = (VertexAttribPointerType)CustomAttributeDic["ComponentType"].Value;

                GL.EnableVertexAttribArray(index);
                GL.VertexAttribPointer(index, nComponentCount, eType, false, VertexAttrType.StructLayoutAttribute.Size, new IntPtr(nOffset));
            }
        }
    }
}
