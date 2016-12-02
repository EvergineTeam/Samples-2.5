#region Using Statements
using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Input;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace NavigationFlow.Components
{
    [DataContract]
    public class KeyboardNavigationComponent : Behavior
    {
        [RequiredService]
        protected Input inputService;

        [RequiredComponent]
        protected NavigationComponent navComponent;

        [DataMember]
        public Keys KeyTrigger;

        private KeyboardState lastKeyboardState;

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.KeyTrigger = Keys.Escape;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var currentKeyboardState = this.inputService.KeyboardState;

            if (this.lastKeyboardState.IsKeyPressed(this.KeyTrigger) &&
                currentKeyboardState.IsKeyReleased(this.KeyTrigger))
            {
                this.navComponent.DoNavigation();
            }

            this.lastKeyboardState = currentKeyboardState;
        }
    }
}
