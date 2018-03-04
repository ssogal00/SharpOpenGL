using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.MaterialBase;
using Core.Texture;
using System.Diagnostics;

namespace SharpOpenGL
{
    public class RenderCommand
    {  
        public RenderCommand(MaterialBase material)
        {
        }

        public void MaterialParamSetup()
        {
        }

        public void Render()
        {
            m_Material.Setup();
            m_RenderTarget.Clear();
            m_RenderTarget.Bind();
        }

        // 1. Material(shader setup)
        MaterialBase m_Material = null;

        // 2. RenderTarget 
        RenderTarget m_RenderTarget = null;

        // 3. Vertex, Index Buffer
        
    }
}
