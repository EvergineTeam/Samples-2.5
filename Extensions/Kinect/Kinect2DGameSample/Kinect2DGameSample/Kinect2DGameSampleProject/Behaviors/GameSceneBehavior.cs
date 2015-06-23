#region File Description
//-----------------------------------------------------------------------------
// GameSceneBehavior
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using WaveEngine.Framework;
using Kinect2DGameSampleProject.Scenes;
using WaveEngine.Components.Transitions;
using WaveEngine.Framework.Services;
#endregion

namespace Kinect2DGameSampleProject.Behaviors
{
    /// <summary>
    /// Game Scene Behavior
    /// </summary>
    public class GameSceneBehavior : SceneBehavior
    {
        /// <summary>
        /// Resolves the dependencies needed for this instance to work.
        /// </summary>
        protected override void ResolveDependencies()
        {
        }

        /// <summary>
        /// Allows this instance to execute custom logic during its 
        /// <c>Update</c>.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <remarks>
        /// This method will not be executed if it are not 
        /// <c>Active</c>.
        /// </remarks>
        protected override void Update(TimeSpan gameTime)
        {
            var gameScene = this.Scene as GameScene;
            if (gameScene == null)
            {
                return;
            }

            if (gameScene.TimeCounter.Finished)
            {
                WaveServices.TimerFactory.RemoveAllTimers();

                // When Game scene ends, must change to Start Scene 
                WaveServices.ScreenContextManager.Pop();
                WaveServices.ScreenContextManager.Push(new ScreenContext(new StartScene()), new CrossFadeTransition(TimeSpan.FromSeconds(3.0f)));
                this.IsActive = false;
            }
        }
    }
}
