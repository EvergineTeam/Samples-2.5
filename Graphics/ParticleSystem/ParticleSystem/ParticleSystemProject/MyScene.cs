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

#region Using Statements
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components;
using WaveEngine.Framework;
using WaveEngine.Components.UI;
using WaveEngine.Framework.Graphics;
using System.Linq;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework.UI;
#endregion

namespace ParticleSystemProject
{
    public class MyScene : Scene
    {
        public TextBlock textBlock1;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Black;

            // Main Camera
            ViewCamera camera = new ViewCamera("MainCamera", new Vector3(0, 400, 1200), new Vector3(0, 400, 0));

            EntityManager.Add(camera.Entity);
            RenderManager.SetActiveCamera(camera.Entity);

            // Initialize particle system

            Entity entitySystem = new Entity("Particles")
                .AddComponent(new Transform3D())
                .AddComponent(new ParticleBehavior())
                .AddComponent(new ParticleSystemRenderer3D());
            
            var behavior = entitySystem.FindComponent<ParticleBehavior>();
            behavior.ApplyChanges();
            behavior.ApplyMaterial();

            EntityManager.Add(entitySystem);

            textBlock1 = new TextBlock()
            {
                Margin = new Thickness(10),
                Text = "Fire",
                TextWrapping = true,
            };
            EntityManager.Add(textBlock1.Entity);
        }
    }
}
