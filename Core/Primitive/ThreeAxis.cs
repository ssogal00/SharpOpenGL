﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Core.Primitive
{
    public class ThreeAxis : SceneObject
    {
        // @ ISceneobject interface

        public override OpenTK.Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }

        public void Draw()
        {
        }

        public override void Draw(MaterialBase.MaterialBase material)
        {
            // 
            xAxis.ParentMatrix = LocalMatrix * ParentMatrix;
            xAxis.Draw(material);

            //
            yAxis.ParentMatrix = Matrix4.CreateRotationZ(OpenTK.MathHelper.DegreesToRadians(90))* LocalMatrix * ParentMatrix;
            yAxis.Draw(material);
            //

            zAxis.ParentMatrix = Matrix4.CreateRotationY(OpenTK.MathHelper.DegreesToRadians(-90)) *LocalMatrix* ParentMatrix;
            zAxis.Draw(material);
        }
        // @ ISceneobject interface

        public ThreeAxis()
        {
        }


        protected Arrow xAxis = new Arrow(10, new Vector3(1,0,0));
        protected Arrow yAxis = new Arrow(10, new Vector3(0, 1, 0));
        protected Arrow zAxis = new Arrow(10, new Vector3(0, 0, 1));
    }
}
