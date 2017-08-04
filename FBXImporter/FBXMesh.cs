using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

using Core;
using FBXWrapper;

using SharpOpenGL;

namespace FBXImporter
{
    public class FBXMesh
    {
        public FBXMesh(FBXWrapper.ParsedFBXMesh _ParsedFBXMesh)
        {
            
        }

        public void PrepareToDraw()
        {
            VB = new Core.Buffer.StaticVertexBuffer<SharpOpenGL.BasicMaterial.VertexAttribute>();
            IB = new Core.Buffer.IndexBuffer();
        }

        public void Draw()
        {

        }
                
        protected List<SharpOpenGL.BasicMaterial.VertexAttribute> Vertices = new List<SharpOpenGL.BasicMaterial.VertexAttribute>();

        Core.Buffer.StaticVertexBuffer<SharpOpenGL.BasicMaterial.VertexAttribute> VB = null;
        Core.Buffer.IndexBuffer IB = null;
    }
}
