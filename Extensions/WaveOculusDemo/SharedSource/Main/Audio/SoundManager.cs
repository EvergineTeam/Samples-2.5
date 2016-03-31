using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Media;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
using WaveOculusDemo;

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

    [DataContract]
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
            this.soundDictionary[SoundType.Engines_1] = new SoundInfo[] { new SoundInfo(WaveContent.Assets.Sounds.engines_1_wav) };
            this.soundDictionary[SoundType.Engines_2] = new SoundInfo[] { new SoundInfo(WaveContent.Assets.Sounds.engines_2_wav) };
            this.soundDictionary[SoundType.Explosion] = new SoundInfo[] { new SoundInfo(WaveContent.Assets.Sounds.explosion_wav) };
            this.soundDictionary[SoundType.Shoot] = new SoundInfo[] 
            { 
                new SoundInfo(WaveContent.Assets.Sounds.shoot_1_wav), 
                new SoundInfo(WaveContent.Assets.Sounds.shoot_2_wav), 
                new SoundInfo(WaveContent.Assets.Sounds.shoot_3_wav) 
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
        /// Gets sound path by its sound type
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        public string GetSoundPath(SoundType sound)
        {
            var list = this.soundDictionary[sound];
            int id = this.random.NextInt() % list.Length;
            var soundInfo = list[id];

            return soundInfo.Path;
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
