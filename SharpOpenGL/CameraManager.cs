using System;
using Core;
using Core.Camera;
using Core.CustomEvent;
using OpenTK;

namespace SharpOpenGL
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

        public void OnKeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (currentCamera != null)
            {
                currentCamera.OnKeyDown(sender, e);
            }
        }

        public void OnKeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (currentCamera != null)
            {
                currentCamera.OnKeyUp(sender, e);
            }
        }
        

        public void OnWindowResized(object sender, ScreenResizeEventArgs eventArgs)
        {
            if (currentCamera != null)
            {
                currentCamera.OnWindowResized(sender, eventArgs);
            }
        }

        public Matrix4 CurrentCameraView => currentCamera.View;

        public Matrix4 CurrentCameraProj => currentCamera.Proj;

        public Vector3 CurrentCameraEye => currentCamera.EyeLocation;

        public CameraBase CurrentCamera => currentCamera;

        protected CameraBase currentCamera = null;
        protected OrbitCamera orbitCamera = new OrbitCamera();
        protected FreeCamera freeCamera = new FreeCamera();
    }
}
