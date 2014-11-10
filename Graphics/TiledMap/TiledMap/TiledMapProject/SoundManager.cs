using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Media;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;

namespace TiledMapProject
{
    public enum SoundType
    {
        Coin = 0,
        Contact,
        Crash,
        CrateDrop,
        Jump, 
        Victory
    }

    public class SoundManager : Component
    {
        private SoundPlayer soundPlayer;
        private SoundBank bank;
        private Dictionary<SoundType, SoundInfo> sounds;

        protected override void Initialize()
        {
            base.Initialize();
            this.soundPlayer = WaveServices.SoundPlayer;

            // fill sound info
            sounds = new Dictionary<SoundType, SoundInfo>();
            sounds[SoundType.Coin] = new SoundInfo("Content/Sound/coin.wav");
            sounds[SoundType.Contact] = new SoundInfo("Content/Sound/contact.wav");
            sounds[SoundType.CrateDrop] = new SoundInfo("Content/Sound/crateDrop.wav");
            sounds[SoundType.Crash] = new SoundInfo("Content/Sound/crash.wav");
            sounds[SoundType.Jump] = new SoundInfo("Content/Sound/jump.wav");
            sounds[SoundType.Victory] = new SoundInfo("Content/Sound/victory.wav");

            this.bank = new SoundBank(this.Assets);            
            this.soundPlayer.RegisterSoundBank(bank);
            foreach(var item in this.sounds)
            {
                this.bank.Add(item.Value);
            }
        }

        public SoundInstance PlaySound(SoundType soundType)
        {
            return this.soundPlayer.Play(this.sounds[soundType]);
        }
    }
}
