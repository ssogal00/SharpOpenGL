﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledMaterial.GBufferInstanced;
using Core;
using Core.Primitive;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;


namespace Engine
{
    public class InstancedSphere : Sphere
    {
        public InstancedSphere()
            : base()
        {
            this.instanceCount = 36;
            this.Translation = new Vector3(-100,0,0);
            this.Color = new Vector3(1,1,1);
            this.Scale = 2.0f;
        }
        public InstancedSphere(float radius, int stackcount, int sectorcount, int instanceCount)
            : base(radius, stackcount, sectorcount)
        {
            this.instanceCount = instanceCount;
        }

        public override void Initialize()
        {
            GenerateVertices();

            RenderingThread.Instance.ExecuteImmediatelyIfRenderingThread(() =>
            {
                drawable = new DrawableBase<PNTT_VertexAttribute>();
                var vertexArray = VertexList.ToArray();
                drawable.SetupVertexData(ref vertexArray);

                VertexList.Clear();

                defaultMaterial = ShaderManager.Instance.GetMaterial<GBufferInstanced>();

                bReadyToDraw = true;
            });
        }

        public override void Render()
        {
            if (bReadyToDraw)
            {
                using (var dummy = new ScopedBind(defaultMaterial))
                {
                    var gbufferDraw = (GBufferInstanced) defaultMaterial;

                    gbufferDraw.LightChannel = (int) Light.LightChannel.StaticMeshChannel;
                    
                    gbufferDraw.CameraTransform_View = CameraManager.Instance.CurrentCameraView;
                    gbufferDraw.CameraTransform_Proj = CameraManager.Instance.CurrentCameraProj;
                    gbufferDraw.ModelTransform_Model = this.LocalMatrix;

                    gbufferDraw.NormalMapExist = false;
                    gbufferDraw.MetalicExist = false;
                    gbufferDraw.RoughnessExist = false;
                    gbufferDraw.DiffuseMapExist = false;
                    
                    gbufferDraw.DiffuseOverride = Color;

                    gbufferDraw.MetallicCount = 6;
                    gbufferDraw.RoughnessCount = 6;

                    drawable.DrawArraysInstanced(PrimitiveType.Triangles, 6*6);
                }
            }
        }

        protected int instanceCount = 1;
    }
}
