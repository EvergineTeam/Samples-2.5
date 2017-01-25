using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingWaveBehaviors;
using Moq;

namespace TestProject
{
    [TestClass]
    public class TestClass
    {
        [TestMethod]
        public void IfThereAreBulletsAShootSucceed()
        {
            var fireControllerMock = new Mock<IFire>();

            GunController controller = new GunController();
            controller.fireController = fireControllerMock.Object;

            controller.ApplyFire();

            Assert.AreEqual(4, controller.bulletsLeft);

            fireControllerMock.Verify(c => c.Fire());
        }

        [TestMethod]
        public void IfThereAreNoBullets_ShootFail()
        {
            var fireControllerMock = new Mock<IFire>();

            GunController controller = new GunController();
            controller.bulletsLeft = 0;
            controller.fireController = fireControllerMock.Object;

            controller.ApplyFire();

            fireControllerMock.Verify(c => c.Fire(), Times.Never);
        }
    }
}
