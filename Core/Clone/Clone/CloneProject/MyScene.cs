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

namespace CloneProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {            
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

            var clone = cube.Clone(name + "_1");
            clone.FindComponent<Transform3D>().Position = new Vector3(position.X + 5, position.Y + 5, position.Z + 5);
            EntityManager.Add(clone);

            clone = cube.Clone(name + "_2");
            clone.FindComponent<Transform3D>().Position = new Vector3(position.X - 5, position.Y - 5, position.Z - 5);
            EntityManager.Add(clone);
        }

        private Color GenerateRandomColors()
        {
            var random = WaveServices.Random;

            var color = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), 255);

            return color;
        }
    }
}
