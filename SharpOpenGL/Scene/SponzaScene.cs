using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpOpenGL.GBufferDraw;
using SharpOpenGL.StaticMesh;


namespace SharpOpenGL.Scene
{
    public class SponzaScene : SceneBase
    {
        protected Transform tranform = new Transform();
        private Task<ObjMesh> meshLoadingTask = null;
        private ObjMesh sponzaMesh = null;

        public SponzaScene()
        {

        }

        public override void CreateSceneResources()
        {
            base.CreateSceneResources();

            meshLoadingTask = ObjMesh.LoadSerializedAsync("sponza.serialized");
        }

        public override void Draw()
        {
            if(meshLoadingTask != null)
            {
                if(!meshLoadingTask.IsCompleted)
                {
                    return;
                }
                else
                {
                    sponzaMesh = meshLoadingTask.Result;
                    sponzaMesh.PrepareToDraw();
                    sponzaMesh.LoadTextures();
                    meshLoadingTask = null;
                }
            }
            


            
        }
    }
}
