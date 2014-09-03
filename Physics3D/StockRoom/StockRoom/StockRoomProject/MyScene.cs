#region Using Statements
using StockRoomProject.Behaviors;
using System;
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
using WaveEngine.Framework.UI;
using WaveEngine.Materials;
#endregion

namespace StockRoomProject
{
    public class MyScene : Scene
    {
        public Entity emiterBoxes, entityWall, entityBridge;
        public TextBlock helpText;

        /// <summary>
        /// Initialize scene
        /// </summary>
        protected override void CreateScene()
        {
            // Scene configuration            
            RenderManager.DebugLines = false;
            PhysicsManager.Gravity3D = new Vector3(0, -120, 0);

            // Camera
            FreeCamera mainCamera = new FreeCamera("MainCamera", new Vector3(-150, 80, 180), Vector3.Zero)
            {
                Speed = 100,
                BackgroundColor = new Color(255 / 255f, 255 / 255f, 192 / 255f),
            };
            mainCamera.Entity.AddComponent(new CombineBehavior());
            mainCamera.Entity.AddComponent(new PickingBehavior());
            //mainCamera.Entity.AddComponent(new LaunchBehavior());
            EntityManager.Add(mainCamera.Entity);            

            // Room
            Entity stockRoom = new Entity("StockRoom")
                        .AddComponent(new Transform3D())
                        .AddComponent(new Model("Content/Models/stockRoomFull.wpk"))
                        .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/roomTexture.wpk")))
                        .AddComponent(new ModelRenderer());

            EntityManager.Add(stockRoom);



            // Ground
            CreateStaticEntities();

            // Boxes
            CreateEmiterBox();

            //CreateBoxStack();

            //CreateBridge();

            // Help text
            CreateHelpText();

            this.AddSceneBehavior(new MySceneBehavior(), SceneBehavior.Order.PostUpdate);
        }

        /// <summary>
        /// Create Help text
        /// </summary>
        private void CreateHelpText()
        {
            helpText = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 20, 0, 0),
                Text = "Key H show/hide help text \n" +
                          "Key F1 diagnostics mode \n" +
                          "Key F5 Emiter boxes scene \n" +
                          "Key F6 Wall boxes scene \n" +
                          "Key F7 Bridge scene \n" +
                          "Key R restart scene \n" +
                          "Key G change gravity direction \n" +
                          "Key W, A, S, D move camera \n" +
                          "Key 1 apply impulse mode \n" +
                          "Key 2 launch ball mode"
            };
            EntityManager.Add(helpText.Entity);
        }

        /// <summary>
        /// Create emiter boxes entity
        /// </summary>
        public void CreateEmiterBox()
        {
            emiterBoxes = new Entity("Emiter")
                       .AddComponent(new Transform3D())
                       .AddComponent(new EmiterBehavior());

            EntityManager.Add(emiterBoxes);
        }

        /// <summary>
        ///  Create wall of boxes entity
        /// </summary>
        public void CreateBoxStack()
        {
            entityWall = new Entity("EntityWall")
                      .AddComponent(new Transform3D())
                      .AddComponent(new StackBehavior());

            EntityManager.Add(entityWall);
        }

        /// <summary>
        /// Create emiter boxes entity
        /// </summary>
        public void CreateBridge()
        {
            entityBridge = new Entity("Bridge")
                       .AddComponent(new Transform3D())
                       .AddComponent(new BridgeBehavior());

            EntityManager.Add(entityBridge);
        }

        /// <summary>
        /// Create physics static collision objects
        /// </summary>
        private void CreateStaticEntities()
        {
            Entity ground = new Entity("Ground")
               .AddComponent(new Transform3D()
               {
                   Position = Vector3.Zero,
                   Scale = new Vector3(567, 1, 387)
               })
               .AddComponent(new BoxCollider())
               .AddComponent(Model.CreateCube())
                //.AddComponent(new MaterialsMap(new List<IMaterial>() { new BasicMaterial(Color.Red) }))
                //.AddComponent(new ModelRenderer())
               .AddComponent(new RigidBody3D() { IsKinematic = true });

            EntityManager.Add(ground);

            Entity wallLeft = new Entity("WallLeft")
              .AddComponent(new Transform3D()
              {
                  Position = new Vector3(0, 122, 190),
                  Scale = new Vector3(567, 244, 1)
              })
              .AddComponent(new BoxCollider())
              .AddComponent(Model.CreateCube())
                //.AddComponent(new MaterialsMap(new List<IMaterial>() { new BasicMaterial(Color.Red) }))
                //.AddComponent(new ModelRenderer())
              .AddComponent(new RigidBody3D() { IsKinematic = true });

            EntityManager.Add(wallLeft);

            Entity wallFront = new Entity("WallFront")
              .AddComponent(new Transform3D()
              {
                  Position = new Vector3(280, 75, 0),
                  Scale = new Vector3(1, 150, 387)
              })
              .AddComponent(new BoxCollider())
              .AddComponent(Model.CreateCube())
                //.AddComponent(new MaterialsMap(new List<IMaterial>() { new BasicMaterial(Color.Red) }))
                //.AddComponent(new ModelRenderer())
              .AddComponent(new RigidBody3D() { IsKinematic = true });

            EntityManager.Add(wallFront);

            Entity wallback = new Entity("WallBack")
              .AddComponent(new Transform3D()
              {
                  Position = new Vector3(-280, 75, 0),
                  Scale = new Vector3(1, 150, 387)
              })
              .AddComponent(new BoxCollider())
              .AddComponent(Model.CreateCube())
                //.AddComponent(new MaterialsMap(new List<IMaterial>() { new BasicMaterial(Color.Red) }))
                //.AddComponent(new ModelRenderer())
              .AddComponent(new RigidBody3D() { IsKinematic = true });

            EntityManager.Add(wallback);

            Entity wallRight = new Entity("WallRight")
              .AddComponent(new Transform3D()
              {
                  Position = new Vector3(0, 122, -192),
                  Scale = new Vector3(567, 244, 1)
              })
              .AddComponent(new BoxCollider())
              .AddComponent(Model.CreateCube())
                //.AddComponent(new MaterialsMap(new List<IMaterial>() { new BasicMaterial(Color.Red) }))
                //.AddComponent(new ModelRenderer())
              .AddComponent(new RigidBody3D() { IsKinematic = true });

            EntityManager.Add(wallRight);

            Entity roff = new Entity("Roff")
              .AddComponent(new Transform3D()
              {
                  Position = new Vector3(142, 190, 0),
                  Rotation = new Vector3(0, 0, MathHelper.ToRadians(-18.26f)),
                  Scale = new Vector3(300, 2, 387)
              })
              .AddComponent(new BoxCollider())
              .AddComponent(Model.CreateCube())
                //.AddComponent(new MaterialsMap(new List<IMaterial>() { new BasicMaterial(Color.Purple) }))
                //.AddComponent(new ModelRenderer())
              .AddComponent(new RigidBody3D() { IsKinematic = true });

            EntityManager.Add(roff);

            Entity roff2 = new Entity("Roff2")
             .AddComponent(new Transform3D()
             {
                 Position = new Vector3(-142, 190, 0),
                 Rotation = new Vector3(0, 0, MathHelper.ToRadians(18.26f)),
                 Scale = new Vector3(300, 2, 387)
             })
             .AddComponent(new BoxCollider())
             .AddComponent(Model.CreateCube())
                //.AddComponent(new MaterialsMap(new List<IMaterial>() { new BasicMaterial(Color.Yellow) }))
                //.AddComponent(new ModelRenderer())
             .AddComponent(new RigidBody3D() { IsKinematic = true });

            EntityManager.Add(roff2);
        }
    }
}
