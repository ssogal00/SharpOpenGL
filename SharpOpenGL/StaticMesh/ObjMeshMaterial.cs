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
        public string MaterialName = null;
        public string DiffuseMap = null;
        public string NormalMap = null;
        public string SpecularMap = null;
        public string RoughnessMap = null;
        public string MaskMap = null;
    }
}
