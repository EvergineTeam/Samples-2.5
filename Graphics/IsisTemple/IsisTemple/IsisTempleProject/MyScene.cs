#region Using Statements
using IsisTempleProject.Components;
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Gestures;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WaveEngine.Materials;
#endregion

namespace IsisTempleProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {            
            //Create the player character
            this.CreatePlayer();

            //Add the UI
            this.CreateUI();

            //Create the level
            this.CreateLevel();

            //Create the camera
            FreeCamera camera = new FreeCamera("freeCamera", new Vector3(0, 2f, -2.5f), Vector3.UnitY * 0.9f)
            {
                Speed = 5,
                BackgroundColor = Color.Black,
            };
            camera.Entity.AddComponent(new CameraBehavior(EntityManager.Find("isis")));

            //Add some light!
            PointLight light = new PointLight("light", Vector3.Zero)
            {
                Attenuation = 75,
                Color = new Color(1, 0.6f, 0.4f),
                IsVisible = true
            };
            light.Entity.AddComponent(new FollowCameraBehavior(camera.Entity));
            light.Entity.AddComponent(new TorchLightBehaviour());

            EntityManager.Add(light);
            EntityManager.Add(camera);            
        }

        /// <summary>
        /// Create User Interface (UI)
        /// </summary>
        private void CreateUI()
        {
            var isisBehavior = EntityManager.Find("isis").FindComponent<IsisBehavior>();
            CreateJoystickButton("UpButton", 200, WaveServices.Platform.ScreenHeight - 300, 0,
                (o, e) => { isisBehavior.GoUp = true; },
                (o, e) => { isisBehavior.GoUp = false; });
            CreateJoystickButton("DownButton", 200, WaveServices.Platform.ScreenHeight - 100, MathHelper.Pi,
                (o, e) => { isisBehavior.GoDown = true; },
                (o, e) => { isisBehavior.GoDown = false; });
            CreateJoystickButton("LeftButton", 100, WaveServices.Platform.ScreenHeight - 200, -MathHelper.PiOver2,
                (o, e) => { isisBehavior.GoLeft = true; },
                (o, e) => { isisBehavior.GoLeft = false; });
            CreateJoystickButton("RightButton", 300, WaveServices.Platform.ScreenHeight - 200, MathHelper.PiOver2,
                (o, e) => { isisBehavior.GoRight = true; },
                (o, e) => { isisBehavior.GoRight = false; });

            var shiftButton = new Entity("ShiftButton")
                .AddComponent(new Sprite("Content/ShiftButton.wpk"))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new Transform2D() { X = WaveServices.Platform.ScreenWidth - 250, Y = WaveServices.Platform.ScreenHeight - 250 })
                .AddComponent(new TouchGestures())
                .AddComponent(new RectangleCollider());
            shiftButton.FindComponent<TouchGestures>().TouchPressed += (o, e) => { isisBehavior.Run = true; };
            shiftButton.FindComponent<TouchGestures>().TouchReleased += (o, e) => { isisBehavior.Run = false; };
            EntityManager.Add(shiftButton);

            var wireframeModeToggle = new ToggleSwitch()
            {
                IsOn = false,
                OnText = "Wireframe?",
                OffText = "Wireframe?",
                Width = 175,
                Margin = new Thickness(50, 50, 0, 0)
            };

            wireframeModeToggle.Toggled += (o, e) =>
            {
                ((OpaqueLayer)RenderManager.FindLayer(DefaultLayers.Opaque)).FillMode = wireframeModeToggle.IsOn ? FillMode.Wireframe : FillMode.Solid;
            };

            EntityManager.Add(wireframeModeToggle.Entity);
        }

        private void CreateJoystickButton(string name, float x, float y, float rotation, EventHandler<GestureEventArgs> pressed, EventHandler<GestureEventArgs> released)
        {
            var upButton = new Entity(name)
                .AddComponent(new Sprite("Content/JoystickButton.wpk"))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new Transform2D() { Origin = Vector2.One / 2, X = x, Y = y, Rotation = rotation })
                .AddComponent(new TouchGestures())
                .AddComponent(new RectangleCollider());
            upButton.FindComponent<TouchGestures>().TouchPressed += pressed;
            upButton.FindComponent<TouchGestures>().TouchReleased += released;
            EntityManager.Add(upButton);
        }

        /// <summary>
        /// Create Character Player
        /// </summary>
        private void CreatePlayer()
        {
            //Crate Isis character <3
            Entity isis = new Entity("isis")
                .AddComponent(new Transform3D())
                .AddComponent(new SkinnedModel("Content/isis.wpk"))
                .AddComponent(new Animation3D("Content/isis-animations.wpk"))
                .AddComponent(new SkinnedModelRenderer())
                .AddComponent(new MaterialsMap(new NormalMappingMaterial("Content/isis-difuse.wpk", "Content/isis-normal.wpk")));

            isis.AddComponent(new IsisBehavior());
            EntityManager.Add(isis);
        }

        /// <summary>
        /// Create Level Scenery
        /// </summary>
        private void CreateLevel()
        {
            //Floor
            Entity floor = new Entity("floor")
                .AddComponent(new Transform3D())
                .AddComponent(new Model("Content/floor.wpk"))
                .AddComponent(new ModelRenderer())
                .AddComponent(new MaterialsMap(new NormalMappingMaterial("Content/floor1.wpk", "Content/floor1_normal_spec.wpk")));

            EntityManager.Add(floor);

            //Suddenly columns! Thousand of them!  (4x2 columns)
            int nColumns = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Entity column = new Entity("column_" + nColumns++)
                        .AddComponent(new Transform3D() { Position = new Vector3(4 - (8 * j), 0, 8 * i) })
                        .AddComponent(new Model("Content/column.wpk"))
                        .AddComponent(new ModelRenderer())
                        .AddComponent(new MaterialsMap(new NormalMappingMaterial("Content/column1.wpk", "Content/column1_normal_spec.wpk")));

                    EntityManager.Add(column);
                }
            }
        }
    }
}
