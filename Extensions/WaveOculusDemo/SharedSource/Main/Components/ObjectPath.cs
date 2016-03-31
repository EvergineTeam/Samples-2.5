using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// Object frame (position and orientation)
    /// </summary>
    public class ObjectFrame
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    /// <summary>
    /// Object Path model
    /// </summary>
    public class ObjectPath
    {
        public int Start { get; private set; }
        public int End { get; private set; }

        public int Duration { get; private set; }
        public int Steps { get; private set; }

        public ObjectFrame[] Frames { get; private set; }

        /// <summary>
        /// Instantiate a object path specified by a file
        /// </summary>
        /// <param name="objectPath">The object path filepath</param>
        public ObjectPath(string objectPath)
            :this(objectPath, Quaternion.Identity)
        {
        }

        /// <summary>
        /// Instantiate a object path specified by a file
        /// </summary>
        /// <param name="objectPath">The object path filepath</param>
        /// <param name="offsetRotation">Orientation offset</param>
        public ObjectPath(string objectPath, Quaternion offsetRotation)
        {
            using (var stream = WaveServices.Storage.OpenContentFile(objectPath))
            {
                var streamReader = new StreamReader(stream);
                string line = streamReader.ReadLine();
                var lineParts = line.Split(' ');

                this.Start = int.Parse(lineParts[0]);
                this.End = int.Parse(lineParts[1]);
                this.Steps = int.Parse(lineParts[2]);
                this.Duration = this.End - this.Start;

                this.Frames = new ObjectFrame[this.Duration];

                Vector3 axis;
                float angle;

                for (int i = 0; i < this.Duration; i++)
                {
                    line = streamReader.ReadLine();
                    lineParts = line.Split(' ');

                    Vector3 position = new Vector3(
                            float.Parse(lineParts[0], System.Globalization.CultureInfo.InvariantCulture),
                            float.Parse(lineParts[1], System.Globalization.CultureInfo.InvariantCulture),
                            float.Parse(lineParts[2], System.Globalization.CultureInfo.InvariantCulture));

                    Quaternion rotation = new Quaternion(
                            float.Parse(lineParts[3], System.Globalization.CultureInfo.InvariantCulture),
                            float.Parse(lineParts[4], System.Globalization.CultureInfo.InvariantCulture),
                            float.Parse(lineParts[5], System.Globalization.CultureInfo.InvariantCulture),
                            float.Parse(lineParts[6], System.Globalization.CultureInfo.InvariantCulture));

                    Quaternion.ToAngleAxis(ref rotation, out axis, out angle);
                    float temp = axis.Z;
                    axis.Z = -axis.Y;
                    axis.Y = -temp;
                    Quaternion fixedRotation;
                    Quaternion.CreateFromAxisAngle(ref axis, angle, out fixedRotation);

                    fixedRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.Pi) * fixedRotation * offsetRotation;

                    ObjectFrame point = new ObjectFrame()
                    {
                        Position = position,
                        Rotation = fixedRotation
                    };

                    this.Frames[i] = point;
                }
            }
        }
    }
}
