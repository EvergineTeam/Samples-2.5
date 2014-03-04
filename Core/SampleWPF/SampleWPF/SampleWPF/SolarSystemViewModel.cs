using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SampleWPFProject;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace SampleWPF
{
    public class SolarSystemViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The wave game
        /// </summary>
        private SampleWPFProject.Game game;

        /// <summary>
        /// The scene
        /// </summary>
        private MyScene scene;

        private float earthRadius;
        private float earthDayLength;

        private float moonRadius;
        private float moonDayLength;
        private float moonOrbitLength;
        private float moonOrbitRadius;

        private float sunFlareRadius;
        private float sunLightIntensity;

        public float EarthDayLength
        {
            get
            {
                return this.earthDayLength;
            }

            set
            {
                this.SetProperty(ref this.earthDayLength, value);
                this.scene.Earth.FindComponent<OrbitBehavior>().DayTime = value;
            }
        }

        public float EarthRadius
        {
            get
            {
                return this.earthRadius;
            }

            set
            {
                this.SetProperty(ref this.earthRadius, value);

                var transform = this.scene.Earth.FindComponent<Transform3D>();
                transform.Scale.X = value;
                transform.Scale.Y = value;
                transform.Scale.Z = value;
            }
        }

        public float MoonDayLength
        {
            get
            {
                return this.moonDayLength;
            }

            set
            {
                this.SetProperty(ref this.moonDayLength, value);

                this.scene.Moon.FindComponent<OrbitBehavior>().DayTime = value;
            }
        }

        public float MoonOrbitRadius
        {
            get
            {
                return this.moonOrbitRadius;
            }

            set
            {
                this.SetProperty(ref this.moonOrbitRadius, value);
                this.scene.Moon.FindComponent<OrbitBehavior>().Radius = value;
            }
        }

        public float MoonOrbitLength
        {
            get
            {
                return this.moonOrbitLength;
            }

            set
            {
                this.SetProperty(ref this.moonOrbitLength, value);
                this.scene.Moon.FindComponent<OrbitBehavior>().YearTime = value;
            }
        }

        public float MoonRadius
        {
            get
            {
                return this.moonRadius;
            }

            set
            {
                this.SetProperty(ref this.moonRadius, value);

                var transform = this.scene.Moon.FindComponent<Transform3D>();
                transform.Scale.X = value;
                transform.Scale.Y = value;
                transform.Scale.Z = value;
            }
        }

        public float SunFlareRadius
        {
            get
            {
                return this.sunFlareRadius;
            }

            set
            {
                this.SetProperty(ref this.sunFlareRadius, value);
                this.scene.Sun.LensFlare.Scale = value;
            }
        }

        public float SunLightIntensity
        {
            get
            {
                return this.sunLightIntensity;
            }

            set
            {
                this.SetProperty(ref this.sunLightIntensity, value);
                this.scene.Sun.Color = new WaveEngine.Common.Graphics.Color(value);
            }
        }

        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="game">The game.</param>
        public void Initialize(SampleWPFProject.Game game)
        {
            this.game = game;
            this.scene = WaveServices.ScreenContextManager.CurrentContext[0] as MyScene;

            this.EarthRadius = this.scene.Earth.FindComponent<Transform3D>().Scale.X;
            this.EarthDayLength = this.scene.Earth.FindComponent<OrbitBehavior>().DayTime;
            this.MoonRadius = this.scene.Moon.FindComponent<Transform3D>().Scale.X;
            this.MoonDayLength = this.scene.Moon.FindComponent<OrbitBehavior>().DayTime;
            this.MoonOrbitLength = this.scene.Moon.FindComponent<OrbitBehavior>().YearTime;
            this.MoonOrbitRadius = this.scene.Moon.FindComponent<OrbitBehavior>().Radius;
            this.SunLightIntensity = this.scene.Sun.Color.R / 255f;
            this.SunFlareRadius = this.scene.Sun.LensFlare.Scale;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
