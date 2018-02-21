using System;

namespace SharpOpenGL.StaticMesh
{
    [Serializable] public class ObjMeshSection
    {

        public UInt32 StartIndex;
        public UInt32 EndIndex = 0;        
        public string SectionName;
    }
}
