using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;

namespace LensFlare
{
    /// <summary>
    /// Orbiting Behavior
    /// </summary>
    [DataContract]
    public class OrbitBehavior : Behavior
    {
        [RequiredComponent]
        public Transform3D Transform { get; set; }

        /// <summary>
        /// The orbit center
        /// </summary>
        private Vector3 orbitCenter;

        /// <summary>
        /// The orbit time in seconds
        /// </summary>
        private float yearTime;

        /// <summary>
        /// The day time in seconds
        /// </summary>
        private float dayTime;

        /// <summary>
        /// The angle
        /// </summary>
        private float orbitAngle;

        /// <summary>
        /// The rotation angle
        /// </summary>
        private float rotationAngle;

        /// <summary>
        /// The angle speed
        /// </summary>
        private float orbitSpeed;

        /// <summary>
        /// The rotation speed
        /// </summary>
        private float rotationSpeed;

        /// <summary>
        /// If it's necessary to calculate the orbital speeds.
        /// </summary>
        private bool calculateSpeed;

        /// <summary>
        /// The radius
        /// </summary>
        private float radius;

        /// <summary>
        /// Gets or sets the day time.
        /// </summary>
        [DataMember]
        public float DayTime
        {
            get
            {
                return this.dayTime;
            }

            set
            {
                this.dayTime = value;
                this.calculateSpeed = true;
            }
        }

        /// <summary>
        /// Gets or sets the orbit center.
        /// </summary>
        [DataMember]
        public Vector3 OrbitCenter
        {
            get
            {
                return this.orbitCenter;
            }

            set
            {
                this.orbitCenter = value;
                this.calculateSpeed = true;
            }
        }

        /// <summary>
        /// Gets or sets the year time.
        /// </summary>
        [DataMember]
        public float YearTime
        {
            get
            {
                return this.yearTime;
            }

            set
            {
                this.yearTime = value;
                this.calculateSpeed = true;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.calculateSpeed = true;
        }

        private void CalculateOrbitalSpeed()
        {
            this.radius = (this.Transform.Position - this.orbitCenter).Length();
            this.orbitAngle = 0;
            this.rotationAngle = (float)Math.PI;

            if (yearTime > 0)
            {
                this.orbitSpeed = (2 * (float)Math.PI) / yearTime;
            }

            if (dayTime > 0)
            {
                this.rotationSpeed = (2 * (float)Math.PI) / dayTime;
            }
        }

        /// <summary>
        /// Allows this instance to execute custom logic during its <c>Update</c>.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <remarks>
        /// This method will not be executed if the <see cref="T:WaveEngine.Framework.Component" />, or the <see cref="T:WaveEngine.Framework.Entity" />
        /// owning it are not <c>Active</c>.
        /// </remarks>
        protected override void Update(TimeSpan gameTime)
        {
            if(this.calculateSpeed)
            {
                this.CalculateOrbitalSpeed();
                this.calculateSpeed = false;
            }

            if (this.orbitSpeed > 0)
            {
                this.orbitAngle = this.orbitAngle + ((float)gameTime.TotalSeconds * this.orbitSpeed);
            }

            Vector3 auxPosition = Vector3.Zero;
            auxPosition.X = this.radius * ((float)Math.Sin(this.orbitAngle));
            auxPosition.Z = this.radius * ((float)Math.Cos(this.orbitAngle));
            this.Transform.Position = auxPosition;
            Labels.Add("Position", auxPosition);

            if (this.rotationSpeed > 0)
            {
                this.rotationAngle = this.rotationAngle + ((float)gameTime.TotalSeconds * this.rotationSpeed);
            }

            Vector3 auxRotation = this.Transform.Rotation;
            auxRotation.Y = this.rotationAngle;
            this.Transform.Rotation = auxRotation;

            Labels.Add("Rotation", rotationAngle);
            Labels.Add("RotaitonEntity", auxRotation);
        }
    }
}
