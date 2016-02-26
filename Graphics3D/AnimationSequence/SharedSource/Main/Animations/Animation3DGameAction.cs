using System;
using WaveEngine.Components.GameActions;
using WaveEngine.Framework;

namespace AnimationSequence.Animations
{
    public class Animation3DGameAction : GameAction
    {
        private static int instances;

        private AnimationSlot animationSlot;

        private Animation3DBehavior animationBehavior;

        public Animation3DGameAction(AnimationSlot animationSlot, Animation3DBehavior animationBehavior, Scene scene = null)
            : base("Animation3DGameAction" + instances++, scene)
        {
            this.animationSlot = animationSlot;
            this.animationBehavior = animationBehavior;
        }

        protected override void PerformRun()
        {
            this.animationBehavior.Completed += this.OnAnimationCompleted;

            this.animationBehavior.BeginAnimation(this.animationSlot);
        }

        private void OnAnimationCompleted(object sender, EventArgs e)
        {
            this.PerformCompleted();
            this.animationBehavior.Completed -= this.OnAnimationCompleted;
        }
    }
}
