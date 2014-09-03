#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
#endregion

namespace SoundsProject
{
    public class MyScene : Scene
    {
        SoundInfo sound1;
        SoundInfo sound2;
        SoundInfo sound3;
        SoundInfo sound4;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Black;

            //Register bank
            SoundBank bank = new SoundBank(Assets);
            WaveServices.SoundPlayer.RegisterSoundBank(bank);

            //Register sounds
            sound1 = new SoundInfo("Content/Menu.wpk");
            bank.Add(sound1);

            sound2 = new SoundInfo("Content/Pistol.wpk");
            bank.Add(sound2);

            sound3 = new SoundInfo("Content/Upgrade.wpk");
            bank.Add(sound3);

            sound4 = new SoundInfo("Content/Sell.wpk");
            bank.Add(sound4);
        }

        protected override void Start()
        {
            base.Start();

            // Play Sound
            WaveServices.SoundPlayer.Play(sound1);
            WaveServices.SoundPlayer.Play(sound2);

            WaveServices.TimerFactory.CreateTimer("Timer1", TimeSpan.FromSeconds(4),
            () =>
            {
                WaveServices.SoundPlayer.Play(sound3);
            });

            WaveServices.TimerFactory.CreateTimer("Timer2", TimeSpan.FromSeconds(2),
            () =>
            {
                WaveServices.SoundPlayer.Play(sound4);
            },
            false);
        }
    }
}
