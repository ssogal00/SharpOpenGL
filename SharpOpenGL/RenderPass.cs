using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.MaterialBase;
using Core.Primitive;
using Core.Texture;

namespace SharpOpenGL
{
    public class RenderPass
    {
        public RenderPass()
        {

        }

        // to draw 3d object

        // render @sceneobject to
        // @renderTarget with
        // @material
        public void Render(RenderTarget renderTarget, MaterialBase material, ISceneObject sceneobject)
        {
            renderTarget.BindAndExecute(material, () =>
            {
                sceneobject.Draw(material);
            });
        }
    }
}
