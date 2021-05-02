using System;
using Core;
using Core.Camera;
using Core.CustomEvent;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Engine
{
    public class CameraManager : Singleton<CameraManager>
    {
        public CameraManager()
        {
            currentCamera = freeCamera;
        }

        public bool IsOrbitCameraMode()
        {
            return currentCamera == orbitCamera ? true : false;
        }

        public bool IsFreeCameraMode()
        {
            return !IsOrbitCameraMode();
        }

        public void SwitchCamera()
        {
            if (currentCamera == freeCamera)
            {
                orbitCamera.DestLocation = orbitCamera.EyeLocation = freeCamera.EyeLocation;
                orbitCamera.LookAtLocation = freeCamera.EyeLocation + freeCamera.GetLookAtDir() * 50.0f;
                orbitCamera.SetDistanceToLookAt(50.0f);
                orbitCamera.AspectRatio = freeCamera.AspectRatio;
                orbitCamera.FOV = freeCamera.FOV;
                currentCamera = orbitCamera;
            }
            else
            {
                currentCamera = freeCamera;
            }
        }

        public void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (currentCamera != null)
            {
                currentCamera.OnKeyDown(sender, e);
            }
        }

        public void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (currentCamera != null)
            {
                currentCamera.OnKeyUp(sender, e);
            }
        }

        public void OnWindowResized(int width, int height)
        {
            if (currentCamera != null)
            {
                currentCamera.OnWindowResized(width, height);
            }
        }

        public Matrix4 CurrentCameraView => currentCamera.View;

        public Matrix4 CurrentCameraProj => currentCamera.Proj;

        public Matrix4 PrevCameraProj => currentCamera.PrevProj;

        public Matrix4 PrevCameraView => currentCamera.PrevView;

        public Vector3 CurrentCameraEye => currentCamera.EyeLocation;

        public Matrix4 CurrentViewport => currentCamera.Viewport;

        public CameraBase CurrentCamera => currentCamera;

        protected CameraBase currentCamera = null;
        protected OrbitCamera orbitCamera = new OrbitCamera();
        protected FreeCamera freeCamera = new FreeCamera();
    }
}
