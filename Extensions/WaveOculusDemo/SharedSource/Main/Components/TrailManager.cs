using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// Trail point.
    /// </summary>
    public struct TrailPoint
    {
        public Vector3 Position;
        public double Time;
    }

    /// <summary>
    /// Trail properties
    /// </summary>
    public class TrailSetting 
    {
        public Transform3D Transform;

        public double TimeStep;
        public double NextPointTime;
        public double CurrentTime;
        public double ExpirationTime;
        public float Thickness;

        public bool Finished;

        public List<TrailPoint> Points = new List<TrailPoint>();

        public void Reset()
        {
            this.Points.Clear();
            this.CurrentTime = 0;
            this.NextPointTime = 0;
            this.Finished = false;
        }
    }

    /// <summary>
    /// Trail manager behavior
    /// </summary>
    [DataContract]
    public class TrailManager : Behavior
    {
        private int InitTrailCount;
        private const int TrailCountIncrement = 8;

        public List<TrailSetting> BusyTrails;
        public List<TrailSetting> FreeTrails;
        public int Capacity;
        public int TrailPointsCount;
        public int NumTrails;

        /// <summary>
        /// INstantiate a new trail manager
        /// </summary>
        /// 
        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.InitTrailCount = 8;
            this.BusyTrails = new List<TrailSetting>(InitTrailCount);
            this.FreeTrails = new List<TrailSetting>(InitTrailCount);
        }

        /// <summary>
        /// Initialize the trail manager
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.InstantiateTrails(InitTrailCount);
        }

        /// <summary>
        /// Instantiate the trails
        /// </summary>
        /// <param name="numTrails">The number of trails to instantiate</param>
        private void InstantiateTrails(int numTrails)
        {
            for (int i = 0; i < numTrails; i++)
            {
                TrailSetting trail = new TrailSetting();
                this.FreeTrails.Add(trail);
            }

            this.Capacity += numTrails;
        }

        /// <summary>
        /// Get a free trail
        /// </summary>
        /// <param name="transform">The transform to follow the trail</param>
        /// <returns></returns>
        public TrailSetting GetFreeTrail(Transform3D transform)
        {
            if (this.FreeTrails.Count == 0)
            {
                this.InstantiateTrails(TrailCountIncrement);
            }

            TrailSetting trail = this.FreeTrails[0];
            this.FreeTrails.RemoveAt(0);
            this.BusyTrails.Add(trail);
            this.NumTrails++;

            trail.Transform = transform;
            trail.Reset();

            if (!this.IsActive)
            {
                this.IsActive = true;
            }

            return trail;
        }

        /// <summary>
        /// Release a trail
        /// </summary>
        /// <param name="trail">The trail instance to release</param>
        public void FreeTrail(TrailSetting trail)
        {
            trail.Finished = true;
        }

        /// <summary>
        /// Remove the trail
        /// </summary>
        /// <param name="trail"></param>
        private void RemoveTrail(TrailSetting trail)
        {
            if (this.BusyTrails.Remove(trail))
            {
                this.TrailPointsCount -= trail.Points.Count;
                this.NumTrails--;

                this.FreeTrails.Add(trail);

                if (this.BusyTrails.Count == 0)
                {
                    this.IsActive = false;
                }
            }
        }

        /// <summary>
        /// Update the trail manager
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Update(TimeSpan gameTime)
        {
            for (int i = this.BusyTrails.Count - 1; i >= 0; i--)
            {
                TrailSetting trail = this.BusyTrails[i];

                trail.CurrentTime += gameTime.TotalSeconds;

                if (!trail.Finished && trail.NextPointTime <= trail.CurrentTime)
                {
                    trail.Points.Add(new TrailPoint()
                    {
                        Position = trail.Transform.Position,
                        Time = trail.CurrentTime
                    });
                    trail.NextPointTime = trail.CurrentTime + trail.TimeStep;
                    this.TrailPointsCount++;                                      
                }

                if (trail.CurrentTime - trail.Points[0].Time > trail.ExpirationTime)
                {
                    trail.Points.RemoveAt(0);
                    this.TrailPointsCount--;
                }

                if (trail.Finished && trail.Points.Count == 0)
                {
                    this.RemoveTrail(trail);
                }
            }
        }
    }
}
