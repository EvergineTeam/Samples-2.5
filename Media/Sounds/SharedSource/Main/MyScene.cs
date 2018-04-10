#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
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
        SoundInfo menuSound;

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
            this.Load(WaveContent.Scenes.MyScene);

            //Register bank
            SoundBank bank = new SoundBank(Assets);
            WaveServices.SoundPlayer.RegisterSoundBank(bank);

            //Register sounds
            this.menuSound = new SoundInfo(WaveContent.Assets.Menu_wav);
            bank.Add(menuSound);

            this.pistolSound = new SoundInfo(WaveContent.Assets.Pistol_wav);
            bank.Add(pistolSound);

            this.upgradeSound = new SoundInfo(WaveContent.Assets.Upgrade_wav);
            bank.Add(upgradeSound);

            this.sellSound = new SoundInfo(WaveContent.Assets.Sell_wav);
            bank.Add(sellSound);

            StackPanel controlPanel = new StackPanel()
            {
                VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Center,
                HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Center,
                Margin = new WaveEngine.Framework.UI.Thickness(0, 0, 30, 30),
                BorderColor = Color.White,
                IsBorder = true,
            };

            this.AddButton("Play Menu", this.menuSound, controlPanel);
            this.AddButton("Play Pistol", this.pistolSound, controlPanel);
            this.AddButton("Play Upgrade", this.upgradeSound, controlPanel);
            this.AddButton("Play Sell", this.sellSound, controlPanel);

            EntityManager.Add(controlPanel);
        }

        private Button AddButton(String name, SoundInfo info, StackPanel panel)
        {
            var btn = new Button(name)
            {
                Text = name,
                Opacity = 1,
                Margin = new WaveEngine.Framework.UI.Thickness(5, 0, 5, 0),
                Width = 170,
            };

            btn.Click += (e, s) =>
            {
                WaveServices.SoundPlayer.Play(info);
            };

            panel.Add(btn);

            return btn;
        }
    }
}
