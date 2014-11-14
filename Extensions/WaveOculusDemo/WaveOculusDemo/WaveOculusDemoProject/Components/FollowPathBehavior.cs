using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// Behavior that move and rotate an entity across a path.
    /// </summary>
    public class FollowPathBehavior : Behavior
    {
        [RequiredComponent]
        private Transform3D transform = null;

        private string path;        
        private ObjectPath objectPath;

        private Quaternion offsetRotation;

        private ScreenplayManager screenplay;
        
        /// <summary>
        /// Instantiate a new follow path behavior
        /// </summary>
        /// <param name="path">The path of the file that describes the path</param>
        public FollowPathBehavior(string path)
            :this(path, Quaternion.Identity)
        {            
        }

        /// <summary>
        /// Instantiate a new follow path behavior
        /// </summary>
        /// <param name="path">The path of the file that describes the path</param>
        /// <param name="offsetRotation">An offset rottation applied to the entity</param>
        public FollowPathBehavior(string path, Quaternion offsetRotation)
        {
            this.path = path;
            this.offsetRotation = offsetRotation;
            this.UpdateOrder = 0f;
        }

        /// <summary>
        /// Initializes the behavior
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.objectPath = new ObjectPath(path, this.offsetRotation);
            this.screenplay = this.EntityManager.Find("ScreenplayManager").FindComponent<ScreenplayManager>();

            this.screenplay.StartFrame = Math.Min(this.objectPath.Start, this.screenplay.StartFrame);
            this.screenplay.EndFrame = Math.Max(this.objectPath.End, this.screenplay.EndFrame);
        }
         
        /// <summary>
        /// Update player transform to follow the path
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        protected override void Update(TimeSpan gameTime)
        {
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
