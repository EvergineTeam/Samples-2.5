using System;
using WaveEngine.Common.Math;

namespace AnimationSequence.Animations
{
    public struct AnimationSlot
    {
        [Flags]
        public enum TransformationTypes
        {
            Position = 0x2,
            Rotation = 0x4,
            Scale = 0x8,
        };

        public TransformationTypes TransformationType { get; set; }
        public TimeSpan TotalTime { get; set; }
        public Vector3 StartPosition { get; set; }
        public Vector3 StartRotation { get; set; }
        public Vector3 StartScale { get; set; }
        public Vector3 EndPosition { get; set; }
        public Vector3 EndRotation { get; set; }
        public Vector3 EndScale { get; set; }
    }
}
