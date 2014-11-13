using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace VuforiaProject.Components
{
    public class FollowCameraBehavior : Behavior
    {
        #region Variables
        [RequiredComponent]
        public Transform3D transform3D;
        #endregion

        #region Initialize
        public FollowCameraBehavior()
            : base("FollowCamera")
        {
        } 
        #endregion

        #region Public Methods
        protected override void Update(TimeSpan gameTime)
        {
            transform3D.Position = this.RenderManager.ActiveCamera3D.Position;
        } 
        #endregion
    }
}
