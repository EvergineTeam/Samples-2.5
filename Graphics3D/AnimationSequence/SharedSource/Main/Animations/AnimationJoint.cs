using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace AnimationSequence.Animations
{
    [DataContract]
    public class AnimationJoint : Component
    {
        [RequiredComponent]
        public Transform3D Transform = null;

        [DataMember]
        public JointEnum Joint { get; set; }
    }
}
