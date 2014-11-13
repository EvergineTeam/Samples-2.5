#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using VuforiaProject.Components;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#if IOS
using WaveEngine.Vuforia; 
#endif
#endregion

namespace VuforiaProject
{
    public class MyScene : Scene
    {
        Physic3DCollisionGroup sceneGroup;
        Physic3DCollisionGroup ballGroup;
        Physic3DCollisionGroup cameraGroup;

        protected override void CreateScene()
        {
            this.DefineCollisionGroups();

            this.CreateCamera();
            this.CreateLight();
            this.CreateCrates();
            this.CreateFloor();

            this.CreateUI();
        }

        protected override void Start()
        {
            base.Start();

#if IOS
            WaveServices.GetService<VuforiaService>().StartTrack();
            WaveServices.GetService<VuforiaService>().TrackNameChanged += (s) =>
                {
                    Console.WriteLine("Track Changed");
                };
#endif
        }

        private void DefineCollisionGroups()
        {
            this.sceneGroup = new Physic3DCollisionGroup();
            this.ballGroup = new Physic3DCollisionGroup();
            this.cameraGroup = new Physic3DCollisionGroup();

            this.cameraGroup.DefineCollisionWith(this.sceneGroup);
            this.cameraGroup.IgnoreCollisionWith(this.ballGroup);
            this.ballGroup.IgnoreCollisionWith(this.sceneGroup);
            this.ballGroup.IgnoreCollisionWith(this.ballGroup);

            this.sceneGroup.DefineCollisionWith(this.cameraGroup);
            this.sceneGroup.DefineCollisionWith(this.ballGroup);
        }

        private void CreateCamera()
        {
#if IOS
            WaveServices.RegisterService(new VuforiaService("TestVuforia.xml"));

            ARCamera camera = new ARCamera("vrCamera")
                {
                    NearPlane = 0.01f
                };


            this.EntityManager.Add(camera);
#else
            //Create a 3D camera
            var camera = new FreeCamera("Camera3D", new Vector3(0, 15, 20), Vector3.Zero)
            {
                NearPlane = 0.01f,
                BackgroundColor = Color.CornflowerBlue
            };
            EntityManager.Add(camera);

#endif
            camera.Entity.AddComponent(new FireBehavior(this.ballGroup));

            var cameraCollision = new Entity()
                           .AddComponent(new Transform3D() { LocalScale = new Vector3(3) })
                           .AddComponent(Model.CreateSphere())
                           .AddComponent(new SphereCollider())
                           .AddComponent(new BodyFollowCameraBehavior())
                           .AddComponent(new RigidBody3D()
                           {
                               IsKinematic = true,
                               CollisionGroup = this.cameraGroup
                           });

            this.EntityManager.Add(cameraCollision);

        }

        private void CreateLight()
        {
            PointLight light = new PointLight("light", Vector3.One * 30)
            {
                Color = Color.White,
                Attenuation = 400,
                Falloff = 400,
            };

            light.Entity.AddComponent(new FollowCameraBehavior());

            this.EntityManager.Add(light);
        }

        private void CreateCrates()
        {
            Material crateMaterial = new NormalMappingMaterial("Content/Textures/woodCrate_diffuse.png", "Content/Textures/woodCrate_normal_spec.png")
            {
                AmbientColor = Color.White * 0.4f
            };
            int baseSize = 5;
            float ySeparation = 1.5f;
            float xSeparation = 1.1f;

            for (int i = 0; i < baseSize; i++)
            {
                for (int j = baseSize - i - 1; j >= 0; j--)
                {
                    float xOffset = -(baseSize - i - 1) / 2f;
                    var cratePosition = new Vector3(xSeparation * (j + xOffset), ySeparation * i + 1, 0);
                    // Draw a cube
                    Entity crate = new Entity() { Tag = "Removable" }
                        .AddComponent(new Transform3D() { LocalPosition = cratePosition })
                        .AddComponent(new Model("Content/Models/crate.FBX"))
                        .AddComponent(new BoxCollider() { })
                        .AddComponent(new RigidBody3D() { CollisionGroup = this.sceneGroup })
                        .AddComponent(new MaterialsMap(crateMaterial))
                        .AddComponent(new ModelRenderer());

                    EntityManager.Add(crate);
                }
            }
        }

        private void CreateUI()
        {
            Button resetButton = new Button()
            {
                Text = "Reset",
                Width = 150,
                Height = 80,
                Margin = new WaveEngine.Framework.UI.Thickness(10)
            };

            resetButton.Click += (s, e) =>
                {
                    this.Reset();
                };

            this.EntityManager.Add(resetButton);
        }

        private void CreateFloor()
        {
            Entity plane = new Entity()
                .AddComponent(new Transform3D())
                .AddComponent(Model.CreatePlane(Vector3.Up, 100))
                .AddComponent(new BoxCollider())
                .AddComponent(new RigidBody3D() 
                { 
                    IsKinematic = true,
                    CollisionGroup = this.sceneGroup
                })
                ;

            EntityManager.Add(plane);
        }

        private void Reset()
        {
            var removables = this.EntityManager.FindAllByTag("Removable").ToList();

            foreach (var removable in removables)
            {
                this.EntityManager.Remove((removable as Entity).Name);
            }

            this.CreateCrates();
        }

    }
}
