using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace SharpOpenGL.StaticMesh
{
    [Serializable] public class ObjMeshMaterial
    {
        public string MaterialName;
        public string DiffuseMap;
        public string NormalMap;
        public string SpecularMap;
        public string RoughnessMap;
        public string MaskMap;
    }
}
