using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace IsisTempleProject.Components
{
    public class FollowCameraBehavior : Behavior
    {
        #region Variables
        [RequiredComponent]
        public Transform3D transform3D;
        private Camera camera; 
        #endregion

        #region Initialize
        public FollowCameraBehavior(Entity cameraEntity)
            : base("FollowCamera")
        {
            this.camera = cameraEntity.FindComponent<Camera>();
        } 
        #endregion

        #region Public Methods
        protected override void Update(TimeSpan gameTime)
        {
            transform3D.Position = camera.Position;
        } 
        #endregion
    }
}
