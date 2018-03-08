using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using ZeroFormatter;

namespace SharpOpenGL.StaticMesh
{
    [ZeroFormattable]
    public class ObjMeshMaterial
    {
        [Index(0)]
        public string MaterialName = null;
        [Index(1)]
        public string DiffuseMap = null;
        [Index(2)]
        public string NormalMap = null;
        [Index(3)]
        public string SpecularMap = null;
        [Index(4)]
        public string RoughnessMap = null;
        [Index(5)]
        public string MaskMap = null;
    }
}
