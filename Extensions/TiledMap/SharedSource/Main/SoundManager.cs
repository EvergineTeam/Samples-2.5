using System.Collections.Generic;
using System.Runtime.Serialization;
using TiledMap;
using WaveEngine.Common.Media;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;

namespace TiledMap
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

    [DataContract]
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
            sounds[SoundType.Coin] = new SoundInfo(WaveContent.Assets.Sound.coin_wav);
            sounds[SoundType.Contact] = new SoundInfo(WaveContent.Assets.Sound.contact_wav);
            sounds[SoundType.CrateDrop] = new SoundInfo(WaveContent.Assets.Sound.crateDrop_wav);
            sounds[SoundType.Crash] = new SoundInfo(WaveContent.Assets.Sound.crash_wav);
            sounds[SoundType.Jump] = new SoundInfo(WaveContent.Assets.Sound.jump_wav);
            sounds[SoundType.Victory] = new SoundInfo(WaveContent.Assets.Sound.victory_wav);

            this.bank = new SoundBank(this.Assets);
            this.soundPlayer.RegisterSoundBank(bank);
            foreach (var item in this.sounds)
            {
                this.bank.Add(item.Value);
            }
        }

        public SoundInstance PlaySound(SoundType soundType, float volume = 1)
        {
            return this.soundPlayer.Play(this.sounds[soundType], volume);
        }
    }
}
