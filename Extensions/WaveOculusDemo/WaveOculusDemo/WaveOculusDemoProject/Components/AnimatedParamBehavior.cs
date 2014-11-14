using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Framework;

namespace WaveOculusDemoProject.Components
{

    /// <summary>
    /// Behavior that animate a bool value over frame times
    /// </summary>
    public class AnimatedParamBehavior : Behavior
    {
        private string path;
        private ObjectAction objectAction;

        private ScreenplayManager screenplay;

        public event EventHandler<bool> OnActionChange;

        private bool currentValue;
        
        public AnimatedParamBehavior(string path)
        {
            this.path = path;
            this.UpdateOrder = 0f;
            this.currentValue = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.objectAction = new ObjectAction(path);
            this.screenplay = this.EntityManager.Find("ScreenplayManager").FindComponent<ScreenplayManager>();            
        }

        /// <summary>
        /// Update action value changed
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        protected override void Update(TimeSpan gameTime)
        {
            int frame = (int)this.screenplay.CurrentFrameTime;

            bool actionValue = false;
            for(int i = 0; i < this.objectAction.Periods.Length; i++)
            {
                var period = this.objectAction.Periods[i];
                if(period.Start < frame && period.End > frame)
                {
                    actionValue = true;
                    break;
                }
            }

            if(actionValue != this.currentValue)
            {
                this.currentValue = actionValue;
                this.OnActionChange(this, this.currentValue);
            }
        }
    }
}
