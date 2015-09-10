#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Components.Gestures;
using WaveEngine.Framework;
#endregion

namespace IsisTemple.Components
{
    [DataContract]
    public class JoystickComponent : Component
    {
        public JoystickComponent()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (!this.isInitialized)
            {
                var isis = this.EntityManager.Find("isis");
                var isisControllerBehavior = isis.FindComponent<IsisControllerBehavior>();

                var upButton = this.Owner.FindChild("upButton");
                this.AddTouchEvents(upButton,
                    (o, e) => { isisControllerBehavior.GoUp = true; },
                    (o, e) => { isisControllerBehavior.GoUp = false; });

                var downButton = this.Owner.FindChild("downButton");
                this.AddTouchEvents(downButton,
                    (o, e) => { isisControllerBehavior.GoDown = true; },
                    (o, e) => { isisControllerBehavior.GoDown = false; });

                var leftButton = this.Owner.FindChild("leftButton");
                this.AddTouchEvents(leftButton,
                    (o, e) => { isisControllerBehavior.GoLeft = true; },
                    (o, e) => { isisControllerBehavior.GoLeft = false; });

                var rightButton = this.Owner.FindChild("rightButton");
                this.AddTouchEvents(rightButton,
                    (o, e) => { isisControllerBehavior.GoRight = true; },
                    (o, e) => { isisControllerBehavior.GoRight = false; });

                var shiftButton = this.Owner.FindChild("shiftButton");
                this.AddTouchEvents(shiftButton,
                    (o, e) => { isisControllerBehavior.Run = true; },
                    (o, e) => { isisControllerBehavior.Run = false; });
            }
        }

        private void AddTouchEvents(Entity buttonEntity, EventHandler<GestureEventArgs> pressed, EventHandler<GestureEventArgs> released)
        {
            var touchGestures = buttonEntity.FindComponent<TouchGestures>();

            touchGestures.TouchPressed += pressed;
            touchGestures.TouchReleased += released;
        }
    }
}
