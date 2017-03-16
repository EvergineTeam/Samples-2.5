using Moq;
using NUnit.Framework;
using TestingWaveBehaviors;

namespace TestLibrary
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void IfThereAreBulletsAShootSucceed()
        {
            Mock<IFire> fireControllerMock = new Mock<IFire>();

            GunController controller = new GunController();
            controller.fireController = fireControllerMock.Object;

            controller.ApplyFire();

            Assert.AreEqual(4, controller.bulletsLeft);

            fireControllerMock.Verify(c => c.Fire());
        }

        [Test]
        public void IfThereAreNoBullets_ShootFail()
        {
            Mock<IFire> fireControllerMock = new Mock<IFire>();

            GunController controller = new GunController();
            controller.bulletsLeft = 0;
            controller.fireController = fireControllerMock.Object;

            controller.ApplyFire();

            fireControllerMock.Verify(c => c.Fire(), Times.Never);            
        }
    }
}
