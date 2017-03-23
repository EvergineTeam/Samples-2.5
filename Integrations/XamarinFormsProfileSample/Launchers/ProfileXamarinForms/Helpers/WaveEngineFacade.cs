using System;
using System.Collections.Generic;
using System.Linq;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using XamarinFormsProfileSample.Behaviors;
using XamarinFormsProfileSample.Components;
using XamarinFormsProfileSample.Events;
using XamarinFormsProfileSampleXamarinForms;

namespace XamarinFormsProfileSample.Helpers
{
    public static class WaveEngineFacade
    {
        private static List<Transform3D> _cameras;

        public static event EventHandler<PushpinTappedEventArgs> PushpinTapped;

        public static ScreenContextManager GetScreenContextManager()
        {
            return WaveServices.ScreenContextManager;
        }

        public static Scene GetCurrentScene()
        {
            if (WaveServices.ScreenContextManager.CurrentContext.Count == 0)
                return null;

            return WaveServices.ScreenContextManager.CurrentContext[0];
        }

        public static void Initialize()
        {
            InitializeCameras();
            SetActiveCamera(AppSettings.DefaultCamera);

            var scene = GetCurrentScene();
            var pushPinBehavior = scene.EntityManager.Find(AppSettings.CameraName)?.
                FindComponent<CameraPushpinBehavior>();

            if (pushPinBehavior != null)
            {
                pushPinBehavior.PushpinTapped += OnPushpinTapped;
            }
        }

        private static void OnPushpinTapped(object sender, PushpinTappedEventArgs e)
        {
            PushpinTapped?.Invoke(sender, e);
        }

        private static void InitializeCameras()
        {
            if (_cameras != null && _cameras.Any())
            {
                return;
            }

            _cameras = new List<Transform3D>();

            var scene = GetCurrentScene();

            var camera1 = scene.EntityManager.Find("anchor1")?.FindComponent<Transform3D>();
            _cameras.Add(camera1);

            var camera2 = scene.EntityManager.Find("anchor2")?.FindComponent<Transform3D>();
            _cameras.Add(camera2);

            var camera3 = scene.EntityManager.Find("anchor3")?.FindComponent<Transform3D>();
            _cameras.Add(camera3);
        }

        public static void SetActiveCamera(int cameraIndex)
        {
            if (cameraIndex < 0 || cameraIndex > AppSettings.NumberOfCameras)
            {
                return;
            }

            var scene = GetCurrentScene();
                       

            var activeCamera = _cameras[cameraIndex];
            var animationCameraComponent = scene.EntityManager.Find(AppSettings.CameraName)?.
                FindComponent<AnimationCameraComponent>();

            if (activeCamera != null)
            {
                animationCameraComponent.MoveTo(activeCamera);
            }
        }
    }
}
