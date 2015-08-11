#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
#endregion

namespace Sounds
{
    public class MyScene : Scene
    {
        /// <summary>
        /// The menu sound
        /// </summary>
        SoundInfo MenuSound;

        /// <summary>
        /// The pistol sound
        /// </summary>
        SoundInfo pistolSound;

        /// <summary>
        /// The upgrade sound
        /// </summary>
        SoundInfo upgradeSound;

        /// <summary>
        /// The sell sound
        /// </summary>
        SoundInfo sellSound;

        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/MyScene.wscene");

            //Register bank
            SoundBank bank = new SoundBank(Assets);
            WaveServices.SoundPlayer.RegisterSoundBank(bank);
            
            //Register sounds
            MenuSound = new SoundInfo(@"Content/Assets/Menu.wav.wpk");
            bank.Add(MenuSound);

            pistolSound = new SoundInfo(@"Content/Assets/Pistol.wav.wpk");
            bank.Add(pistolSound);

            upgradeSound = new SoundInfo(@"Content/Assets/Upgrade.wav.wpk");
            bank.Add(upgradeSound);

            sellSound = new SoundInfo(@"Content/Assets/Sell.wav.wpk");
            bank.Add(sellSound);
        }

        protected override void Start()
        {
            base.Start();

            // Play Sound
            WaveServices.SoundPlayer.Play(MenuSound);
            WaveServices.SoundPlayer.Play(pistolSound);

            WaveServices.TimerFactory.CreateTimer("Timer1", TimeSpan.FromSeconds(4),
            () =>
            {
                WaveServices.SoundPlayer.Play(upgradeSound);
            });

            WaveServices.TimerFactory.CreateTimer("Timer2", TimeSpan.FromSeconds(2),
            () =>
            {
                WaveServices.SoundPlayer.Play(sellSound);
            },
            false);
        }
    }
}
