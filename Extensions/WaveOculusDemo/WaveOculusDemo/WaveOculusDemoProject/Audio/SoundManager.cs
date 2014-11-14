using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Media;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;

namespace WaveOculusDemoProject.Audio
{
    // Sound enumeration types
    public enum SoundType
    {
        Engines_1,  // player engine sound
        Engines_2,  // fighters engine sounds
        Shoot,      // Gun shoot sounds
        Explosion   // Explosion sound
    }

    public class SoundManager : Component
    {
        private SoundBank bank;
        private WaveEngine.Framework.Services.Random random;
        private SoundPlayer soundPlayer;

        private Dictionary<SoundType, SoundInfo[]> soundDictionary;

        public SoundManager()
        {
        }

        /// <summary>
        /// Initialize sound manager
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.random = WaveServices.Random;
            this.soundPlayer = WaveServices.SoundPlayer;

            this.bank = new SoundBank(this.Assets) { MaxConcurrentSounds = 40 };
            this.soundPlayer.RegisterSoundBank(this.bank);

            this.LoadSounds();
        }

        /// <summary>
        /// Load all sound used in this demo
        /// </summary>
        private void LoadSounds()
        {
            this.soundDictionary = new Dictionary<SoundType, SoundInfo[]>();
            this.soundDictionary[SoundType.Engines_1] = new SoundInfo[] { new SoundInfo("Content/Sounds/engines_1.wav") };
            this.soundDictionary[SoundType.Engines_2] = new SoundInfo[] { new SoundInfo("Content/Sounds/engines_2.wav") };
            this.soundDictionary[SoundType.Explosion] = new SoundInfo[] { new SoundInfo("Content/Sounds/explosion.wav") };
            this.soundDictionary[SoundType.Shoot] = new SoundInfo[] 
            { 
                new SoundInfo("Content/Sounds/shoot_1.wav"), 
                new SoundInfo("Content/Sounds/shoot_2.wav"), 
                new SoundInfo("Content/Sounds/shoot_3.wav") 
            };

            foreach (var entry in this.soundDictionary)
            {
                foreach (var soundInfo in entry.Value)
                {
                    this.bank.Add(soundInfo);
                }
            }
        }

        /// <summary>
        /// Play a sound and return its instance
        /// </summary>
        /// <param name="sound">The sound type</param>
        /// <param name="loop">Indicate if the sound must be looped</param>
        /// <returns>The associated sound instance</returns>
        public SoundInstance Play(SoundType sound, bool loop = false)
        {
            var list = this.soundDictionary[sound];
            int id = this.random.NextInt() % list.Length;
            var soundInfo = list[id];

            return this.soundPlayer.Play(soundInfo, 0, loop);
        }
    }
}
