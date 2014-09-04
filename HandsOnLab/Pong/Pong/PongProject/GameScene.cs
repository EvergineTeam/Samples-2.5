
using System;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Gestures;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
using WaveEngine.Framework.UI;


namespace PongProject
{
    public class GameScene : Scene
    {
        //Game Sounds
        private SoundInfo soundPong;
        private SoundInfo soundGoal;

        //Score and Initial TextBlocks 
        public TextBlock textblockGoal1, textblockGoal2, textblockInit;

        //Create Trophy
        public Entity trophy = new Entity("Trophy");

        //Game States
        public enum State
        {
            Init,
            Game,
            Goal,
            Win
        }
        public State CurrentState = State.Init;

        protected override void CreateScene()
        {
            var camera2D = new FixedCamera2D("Camera2D") { BackgroundColor = Color.Black };
            EntityManager.Add(camera2D);

            int offsetTop = 50;

            //Create BackGround
            var background = new Entity("Background")
                            .AddComponent(new Sprite("Content/Texture/real-pong.wpk"))
                            .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                            .AddComponent(new Transform2D()
                            {
                                XScale = 1.5f,
                                YScale = 1.45f,
                            });

            //Create UI
            textblockGoal1 = new TextBlock("Goal1")
                            {
                                Margin = new Thickness((WaveServices.Platform.ScreenWidth / 2f) - 100, offsetTop, 0, 0),
                                FontPath = "Content/Font/Calisto MT.wpk",
                                Text = "0",
                                Height = 130,
                            };



            textblockGoal2 = new TextBlock("Goal2")
                            {
                                Margin = new Thickness((WaveServices.Platform.ScreenWidth / 2f) + 50, offsetTop, 0, 0),
                                FontPath = "Content/Font/Calisto MT.wpk",
                                Text = "0",
                                Height = 130,
                            };


            textblockInit = new TextBlock("TextInit")
                            {
                                Margin = new Thickness((WaveServices.Platform.ScreenWidth / 2f) - 50, offsetTop + 150, 0, 0),
                                FontPath = "Content/Font/Calisto MT.wpk",
                                Text = "",
                                Height = 130,
                                IsVisible = false,
                            };


            //Create Borders
            var barTop = new Entity("BarTop")
                .AddComponent(new Sprite("Content/Texture/wall.wpk"))
                            .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                            .AddComponent(new RectangleCollider())
                            .AddComponent(new Transform2D()
                            {
                                XScale = 1.55f,
                                YScale = 2f,                                
                            });

            var barBot = new Entity("BarBot")
                            .AddComponent(new Sprite("Content/Texture/wall.wpk"))
                            .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                            .AddComponent(new RectangleCollider())
                            .AddComponent(new Transform2D()
                            {
                                XScale = 1.55f,
                                YScale = 2f,
                                Y = WaveServices.Platform.ScreenHeight - 25,                                
                            });

            //Create Players
            var player = new Entity("Player")
                           .AddComponent(new Sprite("Content/Texture/player.wpk"))
                           .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                           .AddComponent(new RectangleCollider())
                           .AddComponent(new Transform2D()
                           {
                               Origin = new Vector2(0.5f, 1),
                               X = WaveServices.Platform.ScreenWidth / 50,
                               Y = WaveServices.Platform.ScreenHeight / 2
                           })
                           .AddComponent(new PlayerBehavior());

            var player2 = new Entity("PlayerIA")
                           .AddComponent(new Sprite("Content/Texture/player.wpk"))
                           .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                           .AddComponent(new RectangleCollider())
                           .AddComponent(new Transform2D()
                           {
                               Origin = new Vector2(0.5f, 1),
                               X = WaveServices.Platform.ScreenWidth - 15,
                               Y = WaveServices.Platform.ScreenHeight / 2
                           });

            //Create Ball
            var ball = new Entity("Ball")
                          .AddComponent(new Sprite("Content/Texture/ball.wpk"))
                          .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                          .AddComponent(new RectangleCollider())
                          .AddComponent(new Transform2D()
                          {
                              Origin = new Vector2(0.5f, 1),
                              X = WaveServices.Platform.ScreenWidth / 2,
                              Y = WaveServices.Platform.ScreenHeight / 2
                          })
                          .AddComponent(new BallBehavior(player, barBot, barTop, player2));


            //Trophy components               
            trophy.AddComponent(new Sprite("Content/Texture/trophy.wpk"));
            trophy.AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            trophy.AddComponent(new Transform2D());
            trophy.IsVisible = false;


            //Add component AI or Second Player controller
            if (WaveServices.ScreenContextManager.CurrentContext.Name == "FromSingleplayer")
                player2.AddComponent(new PlayerAIBehavior(ball));
            else if (WaveServices.ScreenContextManager.CurrentContext.Name == "FromMultiplayer")
                player2.AddComponent(new Player2Behavior());

            //Create Back Button
            var backButtonEntity = new Entity("BackButton")
                           .AddComponent(new Transform2D())
                           .AddComponent(new TextControl()
                           {
                               Text = "Back",
                               HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Center,
                               VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Bottom,
                               Foreground = Color.Black,
                           })
                           .AddComponent(new TextControlRenderer())
                           .AddComponent(new RectangleCollider())
                           .AddComponent(new TouchGestures());

            backButtonEntity.FindComponent<TouchGestures>().TouchPressed += new EventHandler<GestureEventArgs>(Back_TouchPressed);

            //Create a sound bank and sound info for game
            SoundBank bank = new SoundBank(Assets);
            WaveServices.SoundPlayer.RegisterSoundBank(bank);

            soundPong = new SoundInfo("Content/Sound/SoundPong.wpk");
            bank.Add(soundPong);

            soundGoal = new SoundInfo("Content/Sound/SoundGol.wpk");
            bank.Add(soundGoal);

            //Add entities
            EntityManager.Add(backButtonEntity);
            EntityManager.Add(textblockInit);
            EntityManager.Add(player);
            EntityManager.Add(player2);
            EntityManager.Add(ball);
            EntityManager.Add(textblockGoal1);
            EntityManager.Add(textblockGoal2);
            EntityManager.Add(barTop);
            EntityManager.Add(barBot);
            EntityManager.Add(trophy);
            EntityManager.Add(background);

            //Add Scene Behavior Post Update
            AddSceneBehavior(new MySceneBehavior(), SceneBehavior.Order.PostUpdate);
        }

        private void Back_TouchPressed(object sender, GestureEventArgs e)
        {
            WaveServices.TimerFactory.RemoveTimer("Init");
            ScreenContext screenContext = new ScreenContext(new MenuScene())
            {
                Name = "FromGame",
            };
            WaveServices.ScreenContextManager.To(screenContext);
        }

        public void PlaySoundCollision()
        {
            WaveServices.SoundPlayer.Play(soundPong);
        }

        public void PlaySoundGoal()
        {
            WaveServices.SoundPlayer.Play(soundGoal);
        }
    }
}
