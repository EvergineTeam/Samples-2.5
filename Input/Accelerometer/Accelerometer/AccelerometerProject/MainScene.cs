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

using AccelerometerProject.Factories;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Components;
using AccelerometerProject.Behaviors;
using WaveEngine.Framework.Graphics;

namespace AccelerometerProject
{
    public class MainScene : Scene
    {
        protected override void CreateScene()
        {
            Entity camera = new Entity("MainCamera")
                                .AddComponent(new Camera()
                                {
                                    Position = Vector3.Up,
                                    LookAt = Vector3.Forward,
                                })         
                                .AddComponent(new CameraBehavior());

            EntityManager.Add(camera);

            RenderManager.SetActiveCamera(camera);

            var size = 500f;

            EntityManager.Add(EntitiesFactory.CreatePlane("plane1", new Vector3(0, -150, 0), new Vector3(size,1f,size)));
            EntityManager.Add(EntitiesFactory.CreatePlane("plane2", new Vector3(0, 150, 0),new Vector3(size,1f,size)));
            EntityManager.Add(EntitiesFactory.CreatePlane("plane3", new Vector3(-150, 0, 0), new Vector3(1f, size, size)));
            EntityManager.Add(EntitiesFactory.CreatePlane("plane4", new Vector3(150, 0, 0), new Vector3(1f, size, size)));
            EntityManager.Add(EntitiesFactory.CreatePlane("plane5", new Vector3(0, 0, -150), new Vector3(size, size, 1f)));
            EntityManager.Add(EntitiesFactory.CreatePlane("plane6", new Vector3(0, 0, 150), new Vector3(size, size, 1f)));

            for (int i = 0; i < 100; i++)
            {
                var ball1 = EntitiesFactory.CreateBall("ball" + i);
                EntityManager.Add(ball1);
            }

            RenderManager.BackgroundColor = Color.White;
        }
    }
}
