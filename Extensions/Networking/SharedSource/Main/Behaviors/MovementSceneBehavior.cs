using System;
using System.Linq;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;

namespace Networking.Behaviors
{
    public class MovementBehavior : Behavior
    {
        [RequiredComponent]
        protected Transform2D transform;

        private VirtualScreenManager virtualScreenManager;

        protected override void Initialize()
        {
            base.Initialize();

            this.virtualScreenManager = this.Owner.Scene.VirtualScreenManager;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var keyState = WaveServices.Input.KeyboardState;
            var touchState = WaveServices.Input.TouchPanelState;

            var screenXCenter = this.virtualScreenManager.VirtualWidth / 2;
            var screenYCenter = this.virtualScreenManager.VirtualHeight / 2;

            // Collect input
            float xinput = 0;
            float yinput = 0;

            if (touchState.Count > 0)
            {
                var touchPosition = touchState.First().Position;

                xinput = touchPosition.X > screenXCenter ? 1 : -1;
                yinput = touchPosition.Y > screenYCenter ? 1 : -1;
            }
            else if (keyState.IsConnected)
            {
                // use arrows or dpad to move avatar
                if (keyState.IsKeyPressed(Keys.Left))
                    xinput = -1;
                if (keyState.IsKeyPressed(Keys.Right))
                    xinput = 1;
                if (keyState.IsKeyPressed(Keys.Up))
                    yinput = -1;
                if (keyState.IsKeyPressed(Keys.Down))
                    yinput = 1;
            }

            this.transform.Position = new Vector2(this.transform.Position.X + xinput, this.transform.Position.Y + yinput);
        }
    }
}
