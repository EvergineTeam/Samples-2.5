#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using WaveEngine.Common.Input;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;

#endregion

namespace PathFindingProject.Behaviors
{
    /// <summary>
    /// Debug Scene Behavior
    /// </summary>
    public class DebugSceneBehavior : SceneBehavior
    {
        private Input inputService;
        private KeyboardState beforeKeyboardState;

        private bool diagnostics;
        private bool debugLines;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugSceneBehavior" /> class.
        /// </summary>
        public DebugSceneBehavior()
            : base("DebugSceneBehavior")
        {
        }

        /// <summary>
        /// Resolves the dependencies needed for this instance to work.
        /// </summary>
        protected override void ResolveDependencies()
        {            
        }

        /// <summary>
        /// Allows this instance to execute custom logic during its <c>Update</c>.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <remarks>
        /// This method will not be executed if it are not <c>Active</c>.
        /// </remarks>
        protected override void Update(TimeSpan gameTime)
        {
            inputService = WaveServices.Input;

            if (inputService.KeyboardState.IsConnected)
            {
                // Key F1
                if (inputService.KeyboardState.F1 == ButtonState.Pressed &&
                    beforeKeyboardState.F1 != ButtonState.Pressed)
                {
                    this.diagnostics = !this.diagnostics;
                    WaveServices.ScreenContextManager.SetDiagnosticsActive(this.diagnostics);
                }

                // Key F3
                if (inputService.KeyboardState.F3 == ButtonState.Pressed &&
                    beforeKeyboardState.F3 != ButtonState.Pressed)
                {
                    this.debugLines = !this.debugLines;
                    this.Scene.RenderManager.DebugLines = this.debugLines;
                }
            }

            beforeKeyboardState = inputService.KeyboardState;
        }
    }
}
