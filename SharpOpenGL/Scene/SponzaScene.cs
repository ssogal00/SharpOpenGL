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
        protected CameraTransform tranform = new CameraTransform();
        private Task<ObjMesh> meshLoadingTask = null;
        private ObjMesh sponzaMesh = null;
        private MaterialBase gbufferDrawMaterial = null;
        private DeferredLight lightPostProcess = null;
        private BlitToScreen blitToScreen = null;

        public SponzaScene()
        {
        }

        public override void CreateSceneResources()
        {
            base.CreateSceneResources();

            

            gbufferDrawMaterial = new GBufferDraw.GBufferDraw();

            lightPostProcess = new DeferredLight();

            blitToScreen = new BlitToScreen();
            blitToScreen.Create();
            blitToScreen.SetGridSize(3, 3);
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

            // draw to gbuffer
            using (var scoped = new ScopedBind(gbuffer))
            {
                gbufferDrawMaterial.Setup();
                gbufferDrawMaterial.SetUniformBufferValue<CameraTransform>("Transform", ref tranform);
                sponzaMesh.Draw(gbufferDrawMaterial);
                lightPostProcess.Render(gbuffer.GetPositionAttachment, gbuffer.GetColorAttachement, gbuffer.GetNormalAttachment);
            }

            //
            blitToScreen.Blit(gbuffer.GetColorAttachement, 0, 0, 3, 3);
        }
    }
}
