#region Using Statements
using System;
using System.Collections.Generic;
using ParticleSystem2DProject;
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
using WaveEngine.Materials;

#endregion

namespace ParticleSystem2D
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            var textBlock = new TextBlock()
            {
                Text = "Touch the screen to control fireball",
                HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Left,
                VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Top,
                Margin = new WaveEngine.Framework.UI.Thickness(20),
                DrawOrder = 0.1f
            };
            this.EntityManager.Add(textBlock);

            var smoke = EntityManager.Find("meteor.smoke");
            smoke.AddComponent(new ParticleSystemRenderer2D());
            smoke.AddComponent(ParticleSystemFactory.CreateSmokeParticle());

            var fire = EntityManager.Find("meteor.fire");
            fire.AddComponent(new ParticleSystemRenderer2D());
            fire.AddComponent(ParticleSystemFactory.CreateFireParticle());

            var explosionParticles = EntityManager.Find("explosion.explosionParticles");
            explosionParticles.AddComponent(new ParticleSystemRenderer2D());
            explosionParticles.AddComponent(ParticleSystemFactory.CreateExplosion());

            var dinos = EntityManager.Find("explosion.dinos");
            dinos.AddComponent(new ParticleSystemRenderer2D());
            dinos.AddComponent(ParticleSystemFactory.CreateDinosaurs());
        }
    }
}
