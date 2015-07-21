using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;

namespace Project
{
    [DataContract(Namespace = "Project")]
    public class RandomColorAndPositionComponent : Component
    {
        [RequiredComponent]
        private Transform3D transform;

        [RequiredComponent]
        private MaterialsMap materialsMap;

        protected override void Initialize()
        {
            transform.Position = GetRandomVector();
            materialsMap.DefaultMaterial = new StandardMaterial(GetRandomColor(), DefaultLayers.Opaque) { LightingEnabled = false};
        }

        /// <summary>
        /// Generate a random color
        /// </summary>
        /// <returns>A random color</returns>
        private Color GetRandomColor()
        {
            var random = WaveServices.Random;
            var color = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

            return color;
        }

        /// <summary>
        /// Generate a random vector
        /// </summary>
        /// <returns>A random vector</returns>
        private Vector3 GetRandomVector()
        {
            var random = WaveServices.Random;
            var vector = new Vector3(random.Next(-25, 25), 0f, random.Next(-25, 25));

            return vector;
        }
    }
}
