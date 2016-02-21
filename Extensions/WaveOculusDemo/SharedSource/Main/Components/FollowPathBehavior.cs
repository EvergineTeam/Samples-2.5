using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Attributes.Converters;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// Behavior that move and rotate an entity across a path.
    /// </summary>
    [DataContract]
    public class FollowPathBehavior : Behavior
    {
        [RequiredComponent]
        private Transform3D transform = null;

        private string path;        
        private ObjectPath objectPath;

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
                    this.objectPath = new ObjectPath(path);
                }
            }
        }

        [DontRenderProperty]
        [DataMember]
        public Quaternion OffsetOrientation { get; set; }


        [RenderProperty(typeof(Vector3RadianToDegreeConverter))]
        public Vector3 OffsetRotation
        {
            get
            {
                return Quaternion.ToEuler(this.OffsetOrientation);
            }
            set
            {
                this.OffsetOrientation = Quaternion.CreateFromYawPitchRoll(value.Y, value.X, value.Z);
            }
        }

        private ScreenplayManager screenplay;


        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.OffsetOrientation = Quaternion.Identity;
            this.UpdateOrder = 0f;
        }

        /// <summary>
        /// Initializes the behavior
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.objectPath = new ObjectPath(path, this.OffsetOrientation);

            var screenPlayEntity = this.EntityManager.Find("ScreenplayManager");
            if (screenPlayEntity != null)
            {
                this.screenplay = screenPlayEntity.FindComponent<ScreenplayManager>();
                this.screenplay.StartFrame = Math.Min(this.objectPath.Start, this.screenplay.StartFrame);
                this.screenplay.EndFrame = Math.Max(this.objectPath.End, this.screenplay.EndFrame);
            }
        }
         
        /// <summary>
        /// Update player transform to follow the path
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        protected override void Update(TimeSpan gameTime)
        {
            if (this.screenplay == null)
            {
                return;
            }

            // Gets the current frame specified by the screenplay manager.
            int frame = (int)this.screenplay.CurrentFrameTime;

            if (frame < this.objectPath.Start)
            {
                // If the frame is earlier than the path start, return...
                this.Owner.IsVisible = false;
                return;
            }
            
            if (frame >= this.objectPath.End - 1)
            {
                // If the frame is greather than the path end, return...
                this.Owner.IsVisible = false;
                return;
            }

            if (!this.Owner.IsVisible)
            {
                this.Owner.IsVisible = true;
            }
            
            int startFrame = (frame - this.objectPath.Start) % this.objectPath.Duration;
            int endFrame = (startFrame + 1) % this.objectPath.Duration;
            float lerp = (float)(this.screenplay.CurrentFrameTime - frame);

            ObjectFrame startPoint = this.objectPath.Frames[startFrame];
            ObjectFrame endPoint = this.objectPath.Frames[endFrame];

            // Update entity trasnform using the current path frame
            this.transform.Position = Vector3.Lerp(startPoint.Position, endPoint.Position, lerp);
            this.transform.Orientation = Quaternion.Lerp(startPoint.Rotation, endPoint.Rotation, lerp);
        }
    }
}
