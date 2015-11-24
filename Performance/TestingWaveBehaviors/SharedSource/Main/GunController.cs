using System;
using System.Collections.Generic;
using System.Text;

namespace TestingWaveBehaviors
{
    public class GunController
    {
        public int bulletsLeft = 5;
        private TimeSpan shootRate;

        public IFire fireController;

        public void ApplyFire()
        {
            if (bulletsLeft > 0 && shootRate <= TimeSpan.Zero)
            {
                shootRate = TimeSpan.FromMilliseconds(250);
                bulletsLeft--;
                fireController.Fire();
            }
        }

        public void Reload()
        {
            bulletsLeft = 5;
        }

        internal void CountShooRate(TimeSpan gameTime)
        {
            if (shootRate > TimeSpan.Zero)
            {
                shootRate -= gameTime;
            }
        }
    }
}
