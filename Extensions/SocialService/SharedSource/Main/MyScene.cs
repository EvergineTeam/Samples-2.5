#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
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

namespace SocialService
{
    public class MyScene : Scene
    {
        private WaveEngine.Framework.Services.SocialService _socialService;

        private int _buttonWidth;
        private int _buttonHeight;
        private int _spaceControl;
        private int _topMargin;
        private int _leftMargin;

        private Color _foregroundButton;
        private Color _backgroundColor;

        protected override void CreateScene()
        {
            InitializeSceneParameters();

            InitializeSocialService();

            CreateCamera();

            CreateTitlePanel();

            CreateLoginPanel();

            IncreaseTopMargin();

            CreateLeaderBoardsAndAchievementsPanel();

            IncreaseTopMargin();

            CreateLeaderBoardByCodePanel();

            IncreaseTopMargin();

            AchievementsByCodePanel();

            IncreaseTopMargin();

            AddNewScorePanel();
        }

        private void InitializeSocialService()
        {
            _socialService = new WaveEngine.Framework.Services.SocialService
            {
                ////SimulationMode = true
            };
        }

        private void InitializeSceneParameters()
        {
            _foregroundButton = Color.Black;
            _backgroundColor = Color.DarkCyan;

            _leftMargin = 330;
            _topMargin = 150;

            _buttonWidth = 300;
            _buttonHeight = 70;

            _spaceControl = 30;
        }

        private void CreateCamera()
        {
            // Camera
            var camera2D = new FixedCamera2D("Camera2D")
            {
                BackgroundColor = Color.BurlyWood
            };

            EntityManager.Add(camera2D);
        }

        private void CreateTitlePanel()
        {
            var titleLabel = new TextBlock()
            {
                Text = "SocialService Sample",
                Width = 700,
                ////FontPath = "Content/arial.wpk",
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 30, 0, 0)
            };

            EntityManager.Add(titleLabel);
        }

        private void CreateLoginPanel()
        {
            // Login
            var login = new Button()
            {
                Width = _buttonWidth,
                Height = _buttonHeight,
                Text = "Login into service",
                Foreground = _foregroundButton,
                BackgroundColor = _backgroundColor,
            };
            login.Click += async (s, e) =>
            {
                var properties = new Dictionary<string, string>();
                _socialService.Initialize(properties);
                var loggedIn = await _socialService.Login();
                if (!loggedIn)
                {
                    await WaveServices.Platform.ShowMessageBoxAsync("Error", "Not logged in");
                }
            };

            // Logout
            var logout = new Button()
            {
                Width = _buttonWidth,
                Height = _buttonHeight,
                Text = "Logout",
                Foreground = _foregroundButton,
                BackgroundColor = _backgroundColor,
                Margin = new Thickness(_spaceControl, 0, 0, 0)
            };
            logout.Click += async (s, e) => { await _socialService.Logout(); };

            var sp1 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(_leftMargin, _topMargin, 0, 0),
            };
            sp1.Add(login);
            sp1.Add(logout);

            EntityManager.Add(sp1);
        }

        private void AddNewScorePanel()
        {
            var addNewScorelabel1 = new TextBlock()
            {
                Text = "Enter leaderboard code:",
                Width = 200,
            };
            var addNewScoreCodeText1 = new TextBox()
            {
                Text = string.Empty,
                Height = 60,
                Width = _buttonWidth,
                Margin = new Thickness(_spaceControl, 0, 0, 0)
            };

            var addNewScorelabel2 = new TextBlock()
            {
                Text = "Enter new score:",
                Width = 200,
            };
            var addNewScoreCodeText2 = new TextBox()
            {
                Text = string.Empty,
                Height = 60,
                Width = _buttonWidth,
                Margin = new Thickness(_spaceControl, 0, 0, 0)
            };

            var sp5 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(100, _topMargin + 5, 0, 0),
            };
            sp5.Add(addNewScorelabel1);
            sp5.Add(addNewScoreCodeText1);

            _topMargin += 70;

            var sp6 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(100, _topMargin + 5, 0, 0),
            };
            sp6.Add(addNewScorelabel2);
            sp6.Add(addNewScoreCodeText2);

            EntityManager.Add(sp5);
            EntityManager.Add(sp6);

            // Add new score
            var addNewScoreCodeButton = new Button()
            {
                Width = _buttonWidth,
                Height = _buttonHeight,
                Text = "Add new Score",
                Foreground = _foregroundButton,
                BackgroundColor = _backgroundColor,
                Margin = new Thickness(_topMargin + _spaceControl + 15, 580, 0, 0)
            };
            addNewScoreCodeButton.Click += (s, e) =>
            {
                var code = addNewScoreCodeText1.Text;
                var score = addNewScoreCodeText2.Text;

                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(score))
                {
                    return;
                }

                var longScore = long.Parse(score);

                _socialService.AddNewScore(code, longScore);
            };

            EntityManager.Add(addNewScoreCodeButton);
        }

        private void AchievementsByCodePanel()
        {
            var unlockAchievementlabel = new TextBlock()
            {
                Text = "Enter achievement code:",
                Width = 200,
            };
            var unlockAchievementText = new TextBox()
            {
                Text = string.Empty,
                Height = 60,
                Width = _buttonWidth,
                Margin = new Thickness(_spaceControl, 0, 0, 0)
            };

            // Unlock achievement by code
            var unlockAchievementCodeButton = new Button()
            {
                Width = _buttonWidth,
                Height = _buttonHeight,
                Text = "Unlock achievement by code",
                Foreground = _foregroundButton,
                BackgroundColor = _backgroundColor,
                Margin = new Thickness(_spaceControl, 0, 0, 0)
            };
            unlockAchievementCodeButton.Click += async (s, e) =>
            {
                var code = unlockAchievementText.Text;
                await _socialService.UnlockAchievement(code);
            };

            var sp3 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(100, _topMargin + 5, 0, 0),
            };
            sp3.Add(unlockAchievementlabel);
            sp3.Add(unlockAchievementText);
            sp3.Add(unlockAchievementCodeButton);

            EntityManager.Add(sp3);
        }

        private void CreateLeaderBoardByCodePanel()
        {
            var leaderboardlabel = new TextBlock()
            {
                Text = "Enter leaderboard code:",
                Width = 200,
            };
            var leaderboardCodeText = new TextBox()
            {
                Text = string.Empty,
                Height = 60,
                Width = _buttonWidth,
                Margin = new Thickness(_spaceControl, 0, 0, 0)
            };

            // Show leaderboard by code
            var leaderboardCodeButton = new Button()
            {
                Width = _buttonWidth,
                Height = _buttonHeight,
                Text = "Show leaderboard by code",
                Foreground = _foregroundButton,
                BackgroundColor = _backgroundColor,
                Margin = new Thickness(_spaceControl, 0, 0, 0)
            };
            leaderboardCodeButton.Click += (s, e) =>
            {
                var code = leaderboardCodeText.Text;
                _socialService.ShowLeaderboard(code);
            };

            var sp4 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(100, _topMargin + 5, 0, 0),
            };
            sp4.Add(leaderboardlabel);
            sp4.Add(leaderboardCodeText);
            sp4.Add(leaderboardCodeButton);

            EntityManager.Add(sp4);
        }

        private void CreateLeaderBoardsAndAchievementsPanel()
        {
            // Show Leaderboards
            var leaderboards = new Button()
            {
                Width = _buttonWidth,
                Height = _buttonHeight,
                Text = "Show all leaderboards",
                Foreground = _foregroundButton,
                BackgroundColor = _backgroundColor,
            };
            leaderboards.Click += (s, e) => { _socialService.ShowAllLeaderboards(); };

            // Show Achievements
            var achievements = new Button()
            {
                Width = _buttonWidth,
                Height = _buttonHeight,
                Text = "Show all achievements",
                Foreground = _foregroundButton,
                BackgroundColor = _backgroundColor,
                Margin = new Thickness(_spaceControl, 0, 0, 0)
            };
            achievements.Click += (s, e) => { _socialService.ShowAchievements(); };

            var sp2 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(_leftMargin, _topMargin, 0, 0),
            };
            sp2.Add(leaderboards);
            sp2.Add(achievements);

            EntityManager.Add(sp2);
        }

        private void IncreaseTopMargin()
        {
            _topMargin += _spaceControl + _buttonHeight;
        }
    }
}
