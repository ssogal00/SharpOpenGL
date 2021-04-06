using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Engine.Transform
{
    public class TransformBase : RenderingThreadObject
    {
        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender,e);

        }

        public virtual void Transform()
        { }

        public bool IsTransformCompleted { get; set; }
    }
}
