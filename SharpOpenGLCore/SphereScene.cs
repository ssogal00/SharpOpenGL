using System;
using System.Collections.Generic;
using System.Text;
using Core.Primitive;
using OpenTK.Mathematics;
using SharpOpenGL;
using SharpOpenGL.Scene;

namespace SharpOpenGLCore
{
    public class SphereScene : SceneBase
    {
        public override void InitializeScene()
        {
            var rusted = this.CreateGameObject<PBRSphere>();
            rusted.Scale = 1.5f;
            rusted.Translation = new Vector3(10, 0, 0);
            rusted.SetNormalTex("./Resources/Texture/rustediron/rustediron2_normal.dds");
            rusted.SetDiffuseTex("./Resources/Texture/rustediron/rustediron2_basecolor.dds");
            rusted.SetMetallicTex("./Resources/Texture/rustediron/rustediron2_metallic.dds");
            rusted.SetRoughnessTex("./Resources/Texture/rustediron/rustediron2_roughness.dds");

            mGameObjectList.Add(rusted);

            InitializeCamera();
        }

        public override void Render()
        {
            foreach (var item in mGameObjectList)
            {
                item.Render();
            }
        }
    }
}
