#region Using Statements
using NavigationFlow.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using WaveEngine.Common;
using WaveEngine.Components.GameActions;
using WaveEngine.Components.Transitions;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace NavigationFlow.Navigation
{
    public class NavigationService : Service, INavigationService<NavigateCommands>
    {
        private enum NavigationStates
        {
            Undefined = 0,
            MainMenu,
            ExitConfirmation,
            LevelSelection,
            Loading,
            Gameplay,
            Pause,
            Finish,
            Exit,
        }

        private NavigationStates CurrentNavigationState
        {
            get
            {
                if (this.stateHistory == null ||
                    this.stateHistory.Count == 0)
                {
                    return NavigationStates.Undefined;
                }
                else
                {
                    return this.stateHistory.Last();
                }
            }
        }

        private List<NavigationStates> stateHistory;

        private bool isGlobalAssetsLoaded;

        public bool IsPerformingNavigation
        {
            get;
            private set;
        }

        public void StartNavigation()
        {
            if (this.CurrentNavigationState != NavigationStates.Undefined)
            {
                throw new InvalidOperationException("Navegation has been started");
            }

            this.stateHistory = new List<NavigationStates>();

            this.ChangeNavigationState(NavigationStates.MainMenu);
        }

        public bool CanNavigate(NavigateCommands command)
        {
            var nextState = this.GetNewStateForCommand(command);
            return nextState != NavigationStates.Undefined;
        }

        public void Navigate(NavigateCommands command)
        {
            if (this.IsPerformingNavigation)
            {
                return;
            }

            var nextState = this.GetNewStateForCommand(command);

            if (nextState == NavigationStates.Undefined)
            {
                throw new InvalidOperationException("Invalid navigation command detected for this state");
            }

            this.ChangeNavigationState(nextState);
        }

        private NavigationStates GetNewStateForCommand(NavigateCommands command)
        {
            var newState = NavigationStates.Undefined;

            switch (this.CurrentNavigationState)
            {
                case NavigationStates.MainMenu:
                    if (command == NavigateCommands.Back)
                    {
                        newState = NavigationStates.ExitConfirmation;
                    }
                    else if (command == NavigateCommands.Play)
                    {
                        newState = NavigationStates.LevelSelection;
                    }
                    break;

                case NavigationStates.ExitConfirmation:
                    if (command == NavigateCommands.Back)
                    {
                        newState = NavigationStates.MainMenu;
                    }
                    else if (command == NavigateCommands.Quit)
                    {
                        newState = NavigationStates.Exit;
                    }
                    break;

                case NavigationStates.LevelSelection:
                    if (command == NavigateCommands.Back)
                    {
                        newState = NavigationStates.MainMenu;
                    }
                    else if (command == NavigateCommands.Play)
                    {
                        newState = NavigationStates.Loading;
                    }
                    break;

                case NavigationStates.Loading:
                    if (command == NavigateCommands.DefaultForward)
                    {
                        newState = NavigationStates.Gameplay;
                    }
                    break;

                case NavigationStates.Gameplay:
                    if (command == NavigateCommands.Back ||
                        command == NavigateCommands.Pause)
                    {
                        newState = NavigationStates.Pause;
                    }
                    else if (command == NavigateCommands.DefaultForward)
                    {
                        newState = NavigationStates.Finish;
                    }
                    break;

                case NavigationStates.Pause:
                    if (command == NavigateCommands.Back)
                    {
                        newState = NavigationStates.Gameplay;
                    }
                    else if (command == NavigateCommands.ChooseLevel)
                    {
                        newState = NavigationStates.LevelSelection;
                    }
                    else if (command == NavigateCommands.ReturnMainMenu)
                    {
                        newState = NavigationStates.MainMenu;
                    }
                    break;

                case NavigationStates.Finish:
                    if (command == NavigateCommands.ChooseLevel)
                    {
                        newState = NavigationStates.LevelSelection;
                    }
                    else if (command == NavigateCommands.ReturnMainMenu)
                    {
                        newState = NavigationStates.MainMenu;
                    }
                    else if (command == NavigateCommands.NextLevel)
                    {
                        newState = NavigationStates.Loading;
                    }
                    break;
            }

            return newState;
        }

        private void ChangeNavigationState(NavigationStates newState)
        {
            if (stateHistory.Contains(newState))
            {
                if (this.CurrentNavigationState != newState)
                {
                    // Navigate back to this state
                    var destIndex = stateHistory.IndexOf(newState) + 1;
                    for (int i = stateHistory.Count - 1; i >= destIndex; i--)
                    {
                        var isDestState = i == destIndex;
                        this.NavigateBack(doTransition: isDestState);
                        stateHistory.RemoveAt(i);
                    }
                }
            }
            else
            {
                switch (newState)
                {
                    case NavigationStates.Exit:
                        WaveServices.Platform.Exit();
                        break;

                    case NavigationStates.MainMenu:
                        this.PreloadAndNavigateFoward(new MainMenu());
                        break;

                    case NavigationStates.ExitConfirmation:
                        this.PreloadAndNavigateFoward(new ExitConfirmation(), isModal: true);
                        break;

                    case NavigationStates.Finish:
                        this.PreloadAndNavigateFoward(new Finish(), isModal: true);
                        break;

                    case NavigationStates.LevelSelection:
                        this.PreloadAndNavigateFoward(new LevelSelection());
                        break;

                    case NavigationStates.Loading:
                        this.PreloadAndNavigateFoward(new Loading());
                        break;

                    case NavigationStates.Gameplay:
                        this.PreloadAndNavigateFoward(new Gameplay());
                        break;

                    case NavigationStates.Pause:
                        this.PreloadAndNavigateFoward(new Pause(), isModal: true);
                        break;

                    default:
                        throw new InvalidOperationException("Not spected state");
                }

                this.stateHistory.Add(newState);
            }

            Labels.Add("CurrentNavigationState", this.CurrentNavigationState);
        }

        private void PreloadGlobalAssets()
        {
            if (!this.isGlobalAssetsLoaded)
            {
                this.isGlobalAssetsLoaded = true;

                var globalCont = WaveServices.Assets.Global;

                // Fonts
                globalCont.LoadAsset<SpriteFont>(WaveContent.Assets.Fonts.Gill_Sans_MT_18_TTF);
                globalCont.LoadAsset<SpriteFont>(WaveContent.Assets.Fonts.Brady_Bunch_Remastered_72_ttf);

                // Spritesheets
                globalCont.LoadAsset<SpriteSheet>(WaveContent.Assets.GUI.Buttons_spritesheet);
            }
        }

        private void NavigateBack(bool doTransition = true)
        {
            this.SetPerformingNavigationFlag(true);

            if (doTransition)
            {
                var transitionDuration = TimeSpan.FromSeconds(0.2f);
                var transition = new CrossFadeTransition(transitionDuration);
                WaveServices.ScreenContextManager.Pop(transition);

                GameActionFactory.CreateWaitConditionGameAction(null, () => transition.CurrentTime >= transitionDuration)
                                .ContinueWithAction(() =>
                                {
                                    this.SetPerformingNavigationFlag(false);
                                })
                                .Run();
            }
            else
            {
                WaveServices.ScreenContextManager.Pop();
                this.SetPerformingNavigationFlag(false);

            }
        }

        private void PreloadAndNavigateFoward(Scene nextScene, bool isModal = false, bool doTransition = true)
        {
            var transitionDuration = TimeSpan.FromSeconds(0.2f);
            ScreenTransition transition = null;
            var nextConxtext = new ScreenContext(nextScene.Name, nextScene);

            var transitionAction = GameActionFactory.CreateGameActionFromAction(null, () =>
            {
                var screenContextManager = WaveServices.ScreenContextManager;

                if (isModal)
                {
                    screenContextManager.CurrentContext.Behavior = ScreenContextBehaviors.DrawInBackground;
                }

                transition = doTransition ? new CrossFadeTransition(transitionDuration) : null;

                screenContextManager.Push(nextConxtext, transition);
            })
            .AndWaitCondition(() => !doTransition || transition.CurrentTime >= transitionDuration);

            this.PreloadAndNavigateFoward(nextConxtext, transitionAction);
        }

        private void PreloadAndNavigateFoward(ScreenContext nextScreenConxtext, IGameAction transitionAction)
        {
            this.SetPerformingNavigationFlag(true);

            var scenesInitialized = false;

			var preloadAction = new Action(() =>
			{ 
				this.PreloadGlobalAssets();

				for (int i = 0; i < nextScreenConxtext.Count; i++)
				{
					var contextScene = nextScreenConxtext[i];
					contextScene.Initialize(WaveServices.GraphicsDevice);
				}

				scenesInitialized = true;
			});

#if UWP
			System.Threading.Tasks.Task.Factory.StartNew(preloadAction);
#elif __UNIFIED__
			preloadAction();
#else
            WaveServices.TaskScheduler.CreateTask(preloadAction);
#endif

            this.CreateContextAnimation(false, WaveServices.ScreenContextManager.CurrentContext)
                .AndWaitCondition(() => scenesInitialized)
                .ContinueWith(transitionAction)
                .ContinueWith(this.CreateContextAnimation(true, nextScreenConxtext))
                .ContinueWithAction(() =>
                {
                    this.SetPerformingNavigationFlag(false);
                })
                .Run();
        }

        private void SetPerformingNavigationFlag(bool value)
        {
            if (value && this.IsPerformingNavigation)
            {
                throw new InvalidOperationException("Another navigation is in progress");
            }

            this.IsPerformingNavigation = value;
            Labels.Add("IsPerformingNavigation", this.IsPerformingNavigation);
        }

        private IGameAction CreateContextAnimation(bool appear, ScreenContext screenContext)
        {
            if (screenContext == null ||
                screenContext.Count <= 0)
            {
                return GameActionFactory.CreateEmptyGameAction(null);
            }

            List<IGameAction> sceneAnimations = new List<IGameAction>();

            for (int i = 0; i < screenContext.Count; i++)
            {
                var animatedNavigationScene = screenContext[i] as IAnimatedNavigationScene;

                if (animatedNavigationScene != null)
                {
                    if (appear)
                    {
                        sceneAnimations.Add(animatedNavigationScene.CreateAppearGameAction());
                    }
                    else
                    {
                        sceneAnimations.Add(animatedNavigationScene.CreateDiappearGameAction());
                    }
                }
            }

            return GameActionFactory.CreateParallelGameActions(null as Scene, sceneAnimations)
                                    .WaitAll();
        }
    }
}
