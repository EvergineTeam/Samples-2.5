using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Materials;

namespace PathCameraProject
{
    public class MainScene : Scene
    {
        protected override void CreateScene()
        {

            var pointList = new List<CameraPoint>() 
            {
                new CameraPoint() { Position = new Vector3(0f,0f,15f), LookAt = Vector3.Zero, Up = Vector3.Up},
                new CameraPoint() { Position = new Vector3(30f,0f,15f), LookAt = Vector3.Zero, Up = Vector3.Up},
                new CameraPoint() { Position = new Vector3(30f,30f,15f), LookAt = new Vector3(-15f, 0f, 0f), Up = Vector3.UnitX},
                new CameraPoint() { Position = new Vector3(-30f,0f,15f), LookAt= new Vector3(-15f, 0f, 0f), Up = Vector3.Up},
                new CameraPoint() { Position = new Vector3(0f,0f,15f), LookAt = Vector3.Zero, Up = Vector3.Up}
            };

            PathCamera pathCamera = new PathCamera("path", new Vector3(0, 15f, 15f), Vector3.Zero, pointList, 500)
            {
                Speed = 0.5f,
                BackgroundColor = Color.CornflowerBlue,
            };

            EntityManager.Add(pathCamera);

            CreateCube("Cube1", Vector3.Zero);
            CreateCube("Cube2", new Vector3(15f, 0f, 0f));
            CreateCube("Cube3", new Vector3(-15f, 0f, 0f));
            CreateCube("Cube4", new Vector3(0f, 0f, 15f));
            CreateCube("Cube5", new Vector3(0f, 0f, -15f));

        }

        private void CreateCube(string cubeName, Vector3 position)
        {
            var cube = new Entity(cubeName)
                                  .AddComponent(new Transform3D() { Position = position })
                                  .AddComponent(Model.CreateCube())
                                  .AddComponent(new MaterialsMap(new BasicMaterial(Color.Orange)))
                                  .AddComponent(new ModelRenderer());

            EntityManager.Add(cube);
        }
    }
}

