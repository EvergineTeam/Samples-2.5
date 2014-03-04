// Copyright (C) 2012-2013 Weekend Game Studio
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework.UI;

namespace ProjectProject
{
    public class MainScene : Scene
    {
        TextBlock instructions;

        protected override void CreateScene()
        {
            // Create the camera
            FreeCamera mainCamera = new FreeCamera("MainCamera", Vector3.Zero, new Vector3(0f, 0f, -50f));

            // Add the Picking Behavior to the camera
            mainCamera.Entity.AddComponent(new PickingBehavior());
            
            RenderManager.SetActiveCamera(mainCamera.Entity);
            EntityManager.Add(mainCamera.Entity);

            // Add the ground
            Entity ground = new Entity("Ground")
               .AddComponent(new Transform3D() { Position = new Vector3(0, -1, 0)})
               .AddComponent(new BoxCollider())
               .AddComponent(Model.CreatePlane(Vector3.Up, 50))
               .AddComponent(new MaterialsMap(new BasicMaterial(Color.Gray)))
               .AddComponent(new ModelRenderer());

            EntityManager.Add(ground);

            // Create a few models to add in the world
            CreateRandomModel("Cube_1", Model.CreateCube());
            CreateRandomModel("Cube_2", Model.CreateCube());
            CreateRandomModel("Cube_3", Model.CreateCube());
            CreateRandomModel("Cone", Model.CreateCone());
            CreateRandomModel("Cylinder", Model.CreateCylinder());
            CreateRandomModel("Pyramid", Model.CreatePyramid());
            CreateRandomModel("Sphere", Model.CreateSphere());
            CreateRandomModel("Torus_1", Model.CreateTorus());
            CreateRandomModel("Capsule", Model.CreateCapsule());
            CreateRandomModel("Capsule_2", Model.CreateCapsule());
            CreateRandomModel("Capsule_3", Model.CreateCapsule());
            CreateRandomModel("Torus_2", Model.CreateTorus());
            CreateRandomModel("Torus_3", Model.CreateTorus());

            // Add the textbox to show the picked entity
            instructions = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Selected Entity",
            };
            EntityManager.Add(instructions.Entity);

            // Select a background color
            RenderManager.BackgroundColor = Color.CornflowerBlue;
        }

        /// <summary>
        /// Create a random model
        /// </summary>
        /// <param name="name">The name of the model</param>
        /// <param name="model">The model to create</param>
        private void CreateRandomModel(string name, Model model)
        {
            Entity entity = new Entity(name)
                         .AddComponent(new Transform3D() { Position = GetRandomVector(), Scale = new Vector3(1.5f, 1.5f, 1.5f) })
                         .AddComponent(new BoxCollider())
                         .AddComponent(model)
                         .AddComponent(new MaterialsMap(new BasicMaterial(GetRandomColor())))
                         .AddComponent(new ModelRenderer());

            EntityManager.Add(entity);
        }

        /// <summary>
        /// Generate a random color
        /// </summary>
        /// <returns>A random color</returns>
        private Color GetRandomColor()
        {
            var random = WaveServices.Random;
            var color = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

            return color;
        }

        /// <summary>
        /// Generate a random vector
        /// </summary>
        /// <returns>A random vector</returns>
        private Vector3 GetRandomVector()
        {
            var random = WaveServices.Random;
            var vector = new Vector3(random.Next(-25, 25), 0f, random.Next(-25, 25));

            return vector;
        }

        /// <summary>
        /// Show in the screen the name
        /// </summary>
        /// <param name="entityName">the entity name</param>
        public void ShowPickedEntity(string entityName)
        {
            instructions.Text = string.Format("Selected Entity: {0}", entityName);
        }
    }
}
