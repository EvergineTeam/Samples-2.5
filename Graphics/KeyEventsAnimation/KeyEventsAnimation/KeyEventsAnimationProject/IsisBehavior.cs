// Copyright (C) 2012-2013 Weekend Game Studio
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

using System;
using WaveEngine.Common.Helpers;
using WaveEngine.Components.Animation;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Services;

namespace KeyEventsAnimationProject
{
    public class IsisBehavior : Behavior
    {
        [RequiredComponent()]
        public Animation3D Animation;

        #region Initialization
        public IsisBehavior()
            : base("IsisBehavior")
        { }
        #endregion

        #region Private methods
        protected override void Initialize()
        {
            base.Initialize();

            // Add the event to the animation 3D. This event is captured on configured key event previously.
            Animation.OnKeyFrameEvent += new EventHandler<WaveEngine.Common.Helpers.StringEventArgs>(Animation_OnKeyFrameEvent);

            Animation.PlayAnimation("Jog", true);
        }

        protected override void Update(TimeSpan gameTime)
        {
        }

        private void Animation_OnKeyFrameEvent(object sender, StringEventArgs e)
        {
            var random = WaveServices.Random;
            // if the keyevents calls "DoFootstep", play a sound.
            if (e.Value == "DoFootstep")
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        WaveServices.SoundPlayer.Play(SoundsManager.FootStep1);
                        break;
                    case 1:
                        WaveServices.SoundPlayer.Play(SoundsManager.FootStep2);
                        break;
                }
            }
        }
        #endregion
    }
}
