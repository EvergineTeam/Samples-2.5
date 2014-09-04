#region Using Statements
using PiedraPapelOTijeraProject.Entities;
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
#endregion

namespace PiedraPapelOTijeraProject
{
    public class MyScene : Scene
    {
        private const float GameplayDurationInSeconds = 5;

        private Timer countdownTimer;

        private CpuMoveHandEntity cpuMoveHandEntity;

        private bool shouldPlayerWin;

        private Image goalWinImage;

        private Image goalLoseImage;

        private Button successLayerButton;

        private Button failLayerButton;

        private Animation2D countdownAnimationComponent;

        private enum GameStates
        {
            Begin,
            YouWin,
            CpuWins
        }

        protected override void CreateScene()
        {
            // Create the camera entity
            FixedCamera2D camera = new FixedCamera2D("MainCamera")
            {
                BackgroundColor = Color.Black,
            };
            this.EntityManager.Add(camera);

            this.CreateBackground();

            this.cpuMoveHandEntity = new CpuMoveHandEntity();
            this.EntityManager.Add(cpuMoveHandEntity);

            this.CreatePlayerHandButtons();

            this.CreateSuccessAndFailPanel();

            this.CreateGoalImages();
        }

        protected override void Start()
        {
            base.Start();

            // MAYBE: Improve to see the countdown anim. start at frame 0
            this.ChangeGameState(GameStates.Begin);
        }

        private void CreateGoalImages()
        {
            this.goalWinImage = new Image("Goal win", "Content/_0009_GANAS!.wpk")
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            this.EntityManager.Add(this.goalWinImage);

            this.goalLoseImage = new Image("Goal lose", "Content/_0008_objetivo-pierdes.wpk")
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            this.EntityManager.Add(this.goalLoseImage);
        }

        private void CreateBackground()
        {
            var background = new Entity("Background")
                .AddComponent(new Sprite("Content/BG.wpk"))
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque))
                .AddComponent(new Transform2D { DrawOrder = 1 });
            this.EntityManager.Add(background);

            var countdownAnimation = new Entity("Countdown")
                .AddComponent(new Transform2D()
                {
                    X = WaveServices.ViewportManager.VirtualWidth - 20,
                    Y = 20,
                    Origin = Vector2.UnitX
                })
                .AddComponent(new Sprite("Content/Countdown.wpk"))
                .AddComponent(Animation2D.Create<TexturePackerGenericXml>("Content/Countdown.xml")
                    .Add("Default", new SpriteSheetAnimationSequence() { First = 1, Length = 5, FramesPerSecond = 1 }))
                .AddComponent(new AnimatedSpriteRenderer());
            this.EntityManager.Add(countdownAnimation);

            this.countdownAnimationComponent = countdownAnimation.FindComponent<Animation2D>();
        }

        private void CreateSuccessAndFailPanel()
        {
            this.successLayerButton = new Button("Success")
            {
                Opacity = 0.9f,
                DrawOrder = -1,
                BackgroundImage = "Content/_0001_CORRECTO.wpk",
                Text = string.Empty,
                IsBorder = false
            };
            this.EntityManager.Add(successLayerButton);

            this.failLayerButton = new Button("Fail")
            {
                Opacity = 0.9f,
                DrawOrder = -1,
                BackgroundImage = "Content/_0000_Group-6-copy.wpk",
                Text = string.Empty,
                IsBorder = false
            };
            this.EntityManager.Add(this.failLayerButton);

            EventHandler restartGame = (sender, args) => this.ChangeGameState(GameStates.Begin);
            this.successLayerButton.Click += restartGame;
            this.failLayerButton.Click += restartGame;
        }

        private void CreatePlayerHandButtons()
        {
            var playerStoneHandButton = new Button("Player stone hand")
            {
                BackgroundImage = "Content/_0013_piedra.wpk",
                PressedBackgroundImage = "Content/_0015_piedra_active.wpk",
                Text = string.Empty,
                IsBorder = false,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 175)
            };

            playerStoneHandButton.Click += (sender, args) => this.CheckWhoWins(GameplayHandMovesEnum.Stone);

            this.EntityManager.Add(playerStoneHandButton);

            var playerPaperHandButton = new Button("Player paper hand")
            {
                BackgroundImage = "Content/_0017_papel.wpk",
                PressedBackgroundImage = "Content/_0016_papel_active.wpk",
                Text = string.Empty,
                IsBorder = false,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(20, 0, 0, 20)
            };

            playerPaperHandButton.Click += (sender, args) => this.CheckWhoWins(GameplayHandMovesEnum.Paper);

            this.EntityManager.Add(playerPaperHandButton);

            var playerScissorsHandButton = new Button("Player scissors hand")
            {
                BackgroundImage = "Content/_0002_tijeras.wpk",
                PressedBackgroundImage = "Content/_0014_tijeras-active.wpk",
                Text = string.Empty,
                IsBorder = false,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 20, 20)
            };

            playerScissorsHandButton.Click += (sender, args) => this.CheckWhoWins(GameplayHandMovesEnum.Scissors);

            this.EntityManager.Add(playerScissorsHandButton);
        }

        private void CheckWhoWins(GameplayHandMovesEnum playerMove)
        {
            var playerWins = false;

            switch (this.cpuMoveHandEntity.CurrentHandMove)
            {
                case GameplayHandMovesEnum.Stone:
                    playerWins = playerMove == GameplayHandMovesEnum.Paper;

                    break;
                case GameplayHandMovesEnum.Paper:
                    playerWins = playerMove == GameplayHandMovesEnum.Scissors;

                    break;
                case GameplayHandMovesEnum.Scissors:
                    playerWins = playerMove == GameplayHandMovesEnum.Stone;

                    break;
                default:
                    throw new InvalidOperationException();
            }

            if (!this.shouldPlayerWin)
            {
                playerWins = !playerWins;
            }

            var nextGameState = playerWins ? GameStates.YouWin : GameStates.CpuWins;
            this.ChangeGameState(nextGameState);
        }

        private void ChangeGameState(GameStates newGameState)
        {
            switch (newGameState)
            {
                case GameStates.Begin:
                    this.SetUpInitialGameplay();
                    this.StartCountdown();

                    break;
                case GameStates.YouWin:
                    this.successLayerButton.IsVisible = true;
                    this.StopCountdown();

                    break;
                case GameStates.CpuWins:
                    this.failLayerButton.IsVisible = true;
                    this.StopCountdown();

                    break;
                default:
                    break;
            }
        }

        private void StopCountdown()
        {
            this.countdownAnimationComponent.Stop();
            this.countdownTimer.IsPaused = true;
        }

        private void SetUpInitialGameplay()
        {
            this.cpuMoveHandEntity.UpdateGameplay();

            this.UpdateGoalImagesVisibility();

            this.successLayerButton.IsVisible = false;
            this.failLayerButton.IsVisible = false;
        }

        private void UpdateGoalImagesVisibility()
        {
            this.shouldPlayerWin = WaveServices.Random.NextBool();

            this.goalLoseImage.IsVisible = !this.shouldPlayerWin;
            this.goalWinImage.IsVisible = this.shouldPlayerWin;
        }

        private void StartCountdown()
        {
            this.countdownTimer = WaveServices.TimerFactory.CreateTimer("Countdown",
                TimeSpan.FromSeconds(GameplayDurationInSeconds),
                () => this.ChangeGameState(GameStates.CpuWins));
            this.countdownTimer.IsPaused = false;

            this.countdownAnimationComponent.Play();
        }
    }
}
