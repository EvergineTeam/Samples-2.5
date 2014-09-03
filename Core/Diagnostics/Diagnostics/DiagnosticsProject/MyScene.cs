#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace DiagnosticsProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {            
            // Set to true the diagnostic value
            WaveServices.ScreenContextManager.SetDiagnosticsActive(true);
            // And set the compilation symbol PROFILE in the project

            FreeCamera mainCamera = new FreeCamera("MainCamera", new Vector3(-100f, -100f, -100f), Vector3.Zero)
            {
                Speed = 500f,
            };
            mainCamera.BackgroundColor = Color.CornflowerBlue;
            EntityManager.Add(mainCamera.Entity);            

            float radius = 15;
            float angleStep = 1 / radius;
            var scale = 5f;
            for (double angle = 0; angle < Math.PI * 2; angle += angleStep * 2)
            {
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                CreateCube("Cube1_" + angle, new Vector3((x * scale) + scale, 0, (y * scale) + scale), scale, (float)angle, 1f);
                CreateCube("Cube2_" + angle, new Vector3(0f, (x * scale) + scale, (y * scale) + scale), scale, (float)angle, 0.5f);
                CreateCube("Cube3_" + angle, new Vector3((x * scale) + scale, (y * scale) + scale, 0f), scale, (float)angle, 0.75f);
            }            
        }

        private void CreateCube(string name, Vector3 position, float scaleFactor, float angleStep, float speed)
        {
            var cube = new Entity(name)
                           .AddComponent(new Transform3D() { Position = position, Scale = new Vector3(scaleFactor) })
                           .AddComponent(Model.CreateCube())
                            .AddComponent(new MaterialsMap(new BasicMaterial(GenerateRandomColors())))
                       .AddComponent(new ModelRenderer())
                       .AddComponent(new CubeBehavior(name, angleStep, speed));

            EntityManager.Add(cube);
        }

        private Color GenerateRandomColors()
        {
            var random = WaveServices.Random;

            var color = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), 255);

            return color;
        }
    }
}
