using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace IsisTempleProject.Components
{
    public class CameraBehavior : Behavior
    {
        #region Variables
        [RequiredComponent]
        public Camera3D camera;

        private Entity followEntity;
        private Vector3 positionOffset;
        private Vector3 lookatOffset; 
        #endregion

        #region Initialize
        public CameraBehavior(Entity followEntity)
            : base("CameraBehavior")
        {
            this.followEntity = followEntity;

        } 
        #endregion

        #region Public Methods
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            this.positionOffset = camera.Position - camera.LookAt;
            this.lookatOffset = camera.LookAt;
        }

        protected override void Update(TimeSpan gameTime)
        {
            camera.LookAt = followEntity.FindComponent<Transform3D>().Position + this.lookatOffset;
            camera.Position = camera.LookAt + positionOffset;
        } 
        #endregion
    }
}
