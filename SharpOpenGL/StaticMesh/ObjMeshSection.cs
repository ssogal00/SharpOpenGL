using System;
using ZeroFormatter;
using ZeroFormatter.Formatters;
using ZeroFormatter.Internal;

namespace SharpOpenGL.StaticMesh
{
    [ZeroFormattable]
    public class ObjMeshSection
    {
        public ObjMeshSection()
        {
        }

        [Index(0)]
        public virtual UInt32 StartIndex { get; set; }
                
        [Index(1)]
        public virtual UInt32 EndIndex { get; set; }
        
        [Index(2)]
        public virtual string SectionName { get; set; }
    }
}

