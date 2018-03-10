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
        public virtual string MaterialName { get; set; } = null;

        [Index(1)]
        public virtual string DiffuseMap { get; set; } = null;

        [Index(2)]
        public virtual string NormalMap { get; set; } = null;

        [Index(3)]
        public virtual string SpecularMap { get; set; } = null;

        [Index(4)]
        public virtual string RoughnessMap { get; set; } = null;

        [Index(5)]
        public virtual string MaskMap { get; set; } = null;
    }
}
