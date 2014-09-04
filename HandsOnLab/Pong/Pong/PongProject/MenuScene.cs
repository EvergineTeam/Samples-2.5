using System;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Gestures;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;

namespace PongProject
{
    class MenuScene : Scene
    {
        protected override void CreateScene()
        {
            var camera2D = new FixedCamera2D("Camera2D") { BackgroundColor = Color.Black }; 
            EntityManager.Add(camera2D);

            int offset = 100;

            var title = new Entity("Title")
                               .AddComponent(new Sprite("Content/Texture/TitlePong.wpk"))
                               .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                               .AddComponent(new Transform2D()
                               {
                                   Y = WaveServices.Platform.ScreenHeight / 2 - offset,
                                   X = WaveServices.Platform.ScreenWidth / 2 - 150
                               });

            EntityManager.Add(title);

            var multiplayerButtonEntity = new Entity("MultiplayerButton")
                                .AddComponent(new Transform2D() 
                                { 
                                    Y = WaveServices.Platform.ScreenHeight / 2 + 50,
                                    X = WaveServices.Platform.ScreenWidth / 2 - offset,
                                    XScale = 2f,
                                    YScale = 2f
                                })
                                .AddComponent(new TextControl()
                                {
                                    Text = "Multiplayer",
                                    Foreground = Color.White,
                                })
                                .AddComponent(new TextControlRenderer())
                                .AddComponent(new RectangleCollider())
                                .AddComponent(new TouchGestures());

            multiplayerButtonEntity.FindComponent<TouchGestures>().TouchPressed += new EventHandler<GestureEventArgs>(Multiplayer_TouchPressed);

            EntityManager.Add(multiplayerButtonEntity);

            var singleplayerButtonEntity = new Entity("SingleplayerButton")
                                .AddComponent(new Transform2D() 
                                { 
                                    Y = WaveServices.Platform.ScreenHeight / 2,
                                    X = WaveServices.Platform.ScreenWidth / 2 - offset,
                                    XScale = 2f,
                                    YScale = 2f
                                })
                                .AddComponent(new TextControl()
                                {
                                    Text = "Single Player",
                                    Foreground = Color.White,
                                })
                                .AddComponent(new TextControlRenderer())
                                .AddComponent(new RectangleCollider())
                                .AddComponent(new TouchGestures());

            singleplayerButtonEntity.FindComponent<TouchGestures>().TouchPressed += new EventHandler<GestureEventArgs>(Singleplayer_TouchPressed);

            EntityManager.Add(singleplayerButtonEntity);

        } 

        protected override void Start()
        {
            base.Start();
        }

        private void Multiplayer_TouchPressed(object sender, GestureEventArgs e)
        {
            ScreenContext screenContext = new ScreenContext(new GameScene())
            {
                Name = "FromMultiplayer",
            };
            WaveServices.ScreenContextManager.To(screenContext);
        }

        private void Singleplayer_TouchPressed(object sender, GestureEventArgs e)
        {
            ScreenContext screenContext = new ScreenContext(new GameScene())
            {
                Name = "FromSingleplayer",
            };
            WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
