#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
#endregion

namespace IsisTemple.Components
{
    [DataContract]
    public class EntityFollowerBehavior : Behavior
    {
        #region Variables
        [RequiredComponent(false)]
        protected Transform3D transform;

        private string followEntityPath;

        private Entity followEntity;
        private Vector3 positionOffset;
        #endregion

        [DataMember]
        public string FollowEntityPath
        {
            get
            {
                return this.followEntityPath;
            }

            set
            {
                this.followEntityPath = value;

                if (this.isInitialized)
                {
                    this.followEntity = EntityManager.Find(this.followEntityPath);
                }
            }
        }

        #region Initialize
        public EntityFollowerBehavior()
            : base("EntityFollowerBehavior")
        {

        }

        public EntityFollowerBehavior(Entity followEntity)
            : this()
        {
            this.followEntity = followEntity;
            this.followEntityPath = followEntity.EntityPath;
        }
        #endregion

        #region Public Methods
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            if (this.followEntity == null && !string.IsNullOrEmpty(this.followEntityPath))
            {
                this.followEntity = EntityManager.Find(this.followEntityPath);
            }

            var followTransform = followEntity.FindComponent<Transform3D>();
            this.positionOffset = this.transform.Position - followTransform.Position;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var followTransform = followEntity.FindComponent<Transform3D>();

            this.transform.Position = followTransform.Position + positionOffset;
        }
        #endregion
    }
}