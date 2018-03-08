using System;
using ZeroFormatter;
using ZeroFormatter.Formatters;
using ZeroFormatter.Internal;

namespace SharpOpenGL.StaticMesh
{
    [ZeroFormattable]
    public class ObjMeshSection
    {
        [Index(0)]
        public UInt32 StartIndex = 0;
        [Index(1)]
        public UInt32 EndIndex = 0;
        [Index(2)]
        public string SectionName;
    }
}

