using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Camera;
using Core.MaterialBase;
using Core;
using System.IO;
using Core.Buffer;
using OpenTK.Graphics.OpenGL;
using  SharpOpenGL.GBufferDraw;
using Core.CustomEvent;
using OpenTK;

namespace SharpOpenGL.Scene
{
    public class TesselationScene : SceneBase
    {
        protected MaterialBase tesselationTest = null;
        
        protected Cube cube = null;

        protected BlitToScreen screenBlit = null;
                

        private Matrix4 Projection;
        private Matrix4 Model;
        private Matrix4 View;

        public override void CreateSceneResources()
        {
            // create camera
            camera = new OrbitCamera();

            gbuffer = new GBuffer(1024,768);

            //
            screenBlit = new BlitToScreen();
            screenBlit.SetGridSize(1,1);
            screenBlit.Create();

            //
            var vsCode = File.ReadAllText("./Resources/Shader/TesselVertex.shader");
            var tcCode = File.ReadAllText("./Resources/Shader/CubeTesselControl.shader");
            var teCode = File.ReadAllText("./Resources/Shader/CubeTesselEvaluation.shader");
            var fsCode = File.ReadAllText("./Resources/Shader/TesselFragment.shader");

            //
            tesselationTest = new MaterialBase(vsCode, fsCode, tcCode, teCode);

            //
            cube = new Cube();

            Model = Matrix4.CreateScale(10.0f);
            View = Matrix4.LookAt(new Vector3(10, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY);
        }

        public override void OnResize(object sender, ScreenResizeEventArgs args)
        {
            this.width = Math.Max(1, args.Width);
            this.height = Math.Max(1, args.Height);

            gbuffer.OnResize(this, args);

            float fAspectRatio = width / (float)height;

            camera.AspectRatio = fAspectRatio;

            Projection = Matrix4.CreatePerspectiveFieldOfView(camera.FOV, fAspectRatio, camera.Near, camera.Far);
        }

        public override void Draw()
        {
            tesselationTest.Setup();
            
            //
            using (var scopedBind = new ScopedBind(gbuffer))
            {
                gbuffer.Clear();
                gbuffer.PrepareToDraw();

                tesselationTest.Setup();
                tesselationTest.SetUniformVarData("proj_matrix", ref Projection);
                tesselationTest.SetUniformVarData("mv_matrix", Model * View);

                using (var dummy = new WireFrameMode())
                {
                    GL.PatchParameter(PatchParameterInt.PatchVertices, 4);
                    cube.Draw();
                }
            }

            screenBlit.Blit(gbuffer.GetColorAttachement, 0, 0, 1, 1);
        }
    }
}
