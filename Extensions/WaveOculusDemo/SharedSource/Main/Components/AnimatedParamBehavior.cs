using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Attributes;
using WaveEngine.Framework;

namespace WaveOculusDemoProject.Components
{

    /// <summary>
    /// Behavior that animate a bool value over frame times
    /// </summary>
    [DataContract]
    public class AnimatedParamBehavior : Behavior
    {
        private string path;
        private ObjectAction objectAction;

        private ScreenplayManager screenplay;

        public event EventHandler<bool> OnActionChange;

        private bool currentValue;

        [RenderPropertyAsAsset(AssetType.Unknown)]
        [DataMember]
        public string AnimationPath
        {
            get
            {
                return this.path;
            }

            set
            {
                this.path = value;
                if (this.isInitialized)
                {
                    this.objectAction = new ObjectAction(path);
                }
            }
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.UpdateOrder = 0f;
            this.currentValue = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.objectAction = new ObjectAction(path);

            var screenPlayEntity = this.EntityManager.Find("ScreenplayManager");
            if (screenPlayEntity != null)
            {
                this.screenplay = screenPlayEntity.FindComponent<ScreenplayManager>();
            }            
        }

        /// <summary>
        /// Update action value changed
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        protected override void Update(TimeSpan gameTime)
        {
            if (this.screenplay == null)
            {
                return;
            }

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
                if (this.OnActionChange != null)
                {
                    this.OnActionChange(this, this.currentValue);
                }
            }
        }
    }
}
