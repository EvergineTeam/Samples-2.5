using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapProject.Entities;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;

namespace TiledMapProject
{
    public class JoystickScene : Scene
    {
        public Joystick Joystick { get; private set; }
        public JumpButton JumpButton { get; private set; }


        protected override void CreateScene()
        {
            FixedCamera2D camera = new FixedCamera2D("camera")
            {
                ClearFlags = WaveEngine.Common.Graphics.ClearFlags.DepthAndStencil
            };

            this.EntityManager.Add(camera);

            ViewportManager vm = WaveServices.ViewportManager;
            float width = vm.RightEdge - vm.LeftEdge;
            float halfWidth = width / 2;
            float height = vm.BottomEdge - vm.TopEdge;

            this.Joystick = new Joystick("joystick", new RectangleF(vm.LeftEdge, vm.TopEdge, halfWidth, height));
            this.EntityManager.Add(this.Joystick);

            this.JumpButton = new JumpButton("jumpButton", new RectangleF(vm.LeftEdge + (halfWidth), vm.TopEdge, halfWidth, height));
            this.EntityManager.Add(this.JumpButton);
        }        
    }
}
