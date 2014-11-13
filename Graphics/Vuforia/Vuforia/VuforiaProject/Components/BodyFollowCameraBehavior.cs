using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;

namespace VuforiaProject.Components
{
    public class BodyFollowCameraBehavior : Behavior
    {
        #region Variables
        [RequiredComponent]
        public RigidBody3D rigidBody3D;
        #endregion

        #region Initialize
        public BodyFollowCameraBehavior()
            : base("BodyFollowCameraBehavior")
        {
        } 
        #endregion

        #region Public Methods
        protected override void Update(TimeSpan gameTime)
        {
            rigidBody3D.ResetPosition(this.RenderManager.ActiveCamera3D.Position);
        } 
        #endregion
    }
}
