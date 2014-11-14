using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Common.Media;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;

namespace WaveOculusDemoProject.Audio
{
    public class Sound3DInstance
    {
        public float MaxVolume;
        public bool Loop;
        public SoundInstance Instance;

        public void Stop()
        {
            this.Instance.Stop();
        }
    }

    /// <summary>
    /// This class represent a 3D sound emitter
    /// </summary>
    public class Sound3DEmitter : Behavior
    {
        private float maxPan = 1f;
        private float attenuation = 6000;        

        [RequiredComponent]
        private Transform3D transform = null;

        private SoundManager soundManager;

        private List<Sound3DInstance> playingInstances;

        protected override void Initialize()
        {
            base.Initialize();

            this.playingInstances = new List<Sound3DInstance>();
            this.soundManager = this.EntityManager.Find("soundManager").FindComponent<SoundManager>();
        }

        /// <summary>
        /// Play a sound using pan and volume to positionate
        /// </summary>
        /// <param name="sound">Sound type</param>
        /// <param name="maxVolume">Max sound volume</param>
        /// <param name="loop">If the sound is repeated</param>
        /// <returns>A 3D instance sound wrapper</returns>
        public Sound3DInstance Play(SoundType sound, float maxVolume, bool loop)
        {
            var instance = this.soundManager.Play(sound, loop);

            Sound3DInstance newSound3DInstance = new Sound3DInstance()
            {
                MaxVolume = maxVolume,
                Loop = loop,
                Instance = instance
            };

            this.playingInstances.Add(newSound3DInstance);

            return newSound3DInstance;
        }

        /// <summary>
        /// Update each sound 3d instance to adjust its pan and volume
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        protected override void Update(TimeSpan gameTime)
        {
            // Gets camera related vectors
            Vector3 cameraUp = this.RenderManager.ActiveCamera3D.UpVector;
            Vector3 cameraPosition = this.RenderManager.ActiveCamera3D.Position;
            Vector3 cameraLookAtDir = this.RenderManager.ActiveCamera3D.LookAt - cameraPosition;
            Vector3 positionDir = this.transform.Position - cameraPosition;

            float cameraDistanceSqr = positionDir.LengthSquared();
            positionDir.Normalize();
            cameraLookAtDir.Normalize();

            Vector3 lookUpCross = Vector3.Cross(cameraLookAtDir, cameraUp);
            lookUpCross.Normalize();

            float pan = Vector3.Dot(lookUpCross, positionDir);
            pan = MathHelper.Clamp(pan, -1, 1);
            pan *= maxPan;

            for (int i = this.playingInstances.Count - 1; i >= 0; i--)
            {
                var instance = this.playingInstances[i];

                if (instance.Instance == null || instance.Instance.State == SoundState.Stopped)
                {
                    this.playingInstances.RemoveAt(i);
                    break;
                }

                instance.Instance.Volume = Math.Min(1, instance.MaxVolume * attenuation / cameraDistanceSqr);
                instance.Instance.Pan = pan;
            }
        }
    }
}
