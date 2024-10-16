﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.MaterialBase;
using Core.Primitive;
using OpenTK;
using OpenTK.Mathematics;

namespace Engine
{
    public class ThreeAxis : GameObject
    {
        // @ ISceneobject interface

        public override Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }

        protected override void PrepareRenderingData()
        {
        }

        public override void Render()
        {
            if (xAxis.IsReadyToDraw && yAxis.IsReadyToDraw && zAxis.IsReadyToDraw)
            {
                // 
                xAxis.ParentMatrix = LocalMatrix * ParentMatrix;
                xAxis.Render();

                //
                yAxis.ParentMatrix = Matrix4.CreateRotationZ(OpenTK.Mathematics.MathHelper.DegreesToRadians(90)) * LocalMatrix * ParentMatrix;
                yAxis.Render();
                //

                zAxis.ParentMatrix = Matrix4.CreateRotationY(OpenTK.Mathematics.MathHelper.DegreesToRadians(-90)) * LocalMatrix * ParentMatrix;
                zAxis.Render();
            }
        }
        // @ ISceneobject interface

        public ThreeAxis()
        {   
        }


        protected Arrow xAxis = new Arrow(10, new Vector3(1, 0, 0));
        protected Arrow yAxis = new Arrow(10, new Vector3(0, 1, 0));
        protected Arrow zAxis = new Arrow(10, new Vector3(0, 0, 1));
    }
}
