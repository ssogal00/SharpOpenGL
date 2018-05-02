using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpOpenGL.GBufferDraw;
using SharpOpenGL.StaticMesh;
using Core.MaterialBase;
using SharpOpenGL.PostProcess;
using Core;


namespace SharpOpenGL.Scene
{
    public class SponzaScene : SceneBase
    {
        protected Transform tranform = new Transform();
        private Task<ObjMesh> meshLoadingTask = null;
        private ObjMesh sponzaMesh = null;
        private MaterialBase gbufferDrawMaterial = null;
        private DeferredLight lightPostProcess = null;

        public SponzaScene()
        {
        }

        public override void CreateSceneResources()
        {
            base.CreateSceneResources();

            meshLoadingTask = ObjMesh.LoadSerializedAsync("sponza.serialized");

            gbufferDrawMaterial = new GBufferDraw.GBufferDraw();

            lightPostProcess = new DeferredLight();
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

            using (var scoped = new ScopedBind(gbuffer))
            {
                gbufferDrawMaterial.Setup();
                gbufferDrawMaterial.SetUniformBufferValue<Transform>("Transform", ref tranform);
                sponzaMesh.Draw(gbufferDrawMaterial);
                lightPostProcess.Render(gbuffer.GetPositionAttachment, gbuffer.GetColorAttachement, gbuffer.GetNormalAttachment);
            }


        }
    }
}
