using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;
using OpenTK;

namespace SharpOpenGL.Scene
{
    [ZeroFormattable]
    public class SceneObject
    {
        [Index(0)]
        public virtual Vector3 Location { get; set; } = new Vector3(0, 0, 0);

        [Index(1)]
        public virtual float Scale { get; set; } = 1.0f;
    }
}
