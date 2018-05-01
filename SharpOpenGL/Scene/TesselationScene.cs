using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Camera;
using Core.MaterialBase;
using Core;
using System.IO;
using OpenTK.Graphics.OpenGL;
using  SharpOpenGL.GBufferDraw;

namespace SharpOpenGL.Scene
{
    public class TesselationScene : SceneBase
    {
        protected MaterialBase tesselationTest = null;
        
        protected Cube cube = null;

        protected BlitToScreen screenBlit = null;

        protected GBufferDraw.GBufferDraw gbufferMaterial = null;

        public override void CreateSceneResources()
        {
            // base resource
            base.CreateSceneResources();

            // create camera
            camera = new FreeCamera();

            //
            screenBlit = new BlitToScreen();

            var vsCode = File.ReadAllText("./Resources/Shader/TesselVertex.shader");
            var tcCode = File.ReadAllText("./Resources/Shader/CubeTesselControl.shader");
            var teCode = File.ReadAllText("./Resources/Shader/CubeTesselEvaluation.shader");
            var fsCode = File.ReadAllText("./Resources/Shader/TesselFragment.shader");

            tesselationTest = new MaterialBase(vsCode, fsCode, tcCode, teCode);

        }

        public override void Draw()
        {
            tesselationTest.Setup();
            

        }
    }
}
