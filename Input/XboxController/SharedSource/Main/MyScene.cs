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
using WaveEngine.Framework.UI;
using XboxControllerProject;

#endregion

namespace XBoxController
{
    public class MyScene : Scene
    {
        public TextBlock leftStickText, rightStickText, leftTriggerText, rightTriggerText;
        public Entity leftJoystick, rightJoystick, buttonA, buttonB, buttonX, buttonY, buttonBack, buttonStart, dpadUp, dpadDown, dpadLeft, dpadRight, leftShoulder, rightShoulder, leftTrigger, rightTrigger;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.leftJoystick = EntityManager.Find("leftJoystickParent").FindChild("leftJoystick");
            this.rightJoystick = EntityManager.Find("rightJoystickParent").FindChild("rightJoystick");
            this.buttonA = EntityManager.Find("buttonA");
            this.buttonB = EntityManager.Find("buttonB");
            this.buttonX = EntityManager.Find("buttonX");
            this.buttonY = EntityManager.Find("buttonY");
            this.buttonBack = EntityManager.Find("buttonBack");
            this.buttonStart = EntityManager.Find("buttonStart");
            this.dpadUp = EntityManager.Find("dpadUp");
            this.dpadDown = EntityManager.Find("dpadDown");
            this.dpadLeft = EntityManager.Find("dpadLeft");
            this.dpadRight = EntityManager.Find("dpadRight");
            this.leftShoulder = EntityManager.Find("leftShoulder");
            this.rightShoulder = EntityManager.Find("rightShoulder");
            this.leftTrigger = EntityManager.Find("leftTrigger");
            this.rightTrigger = EntityManager.Find("rightTrigger");

            // Texts
            this.leftStickText = new TextBlock()
            {
                Text = "LeftStick: " + Vector2.Zero,
                Foreground = Color.Gray,
                Margin = new Thickness(263, 627, 0, 0),
            };
            EntityManager.Add(this.leftStickText);

            this.rightStickText = new TextBlock()
            {
                Text = "RightStick: " + Vector2.Zero,
                Foreground = Color.Gray,
                Margin = new Thickness(263, 657, 0, 0),
            };
            EntityManager.Add(this.rightStickText);

            this.leftTriggerText = new TextBlock()
            {
                Text = "LeftTrigger: 0",
                Foreground = Color.Gray,
                Margin = new Thickness(856, 627, 0, 0),
            };
            EntityManager.Add(this.leftTriggerText);

            this.rightTriggerText = new TextBlock()
            {
                Text = "RightTrigger: 0",
                Foreground = Color.Gray,
                Margin = new Thickness(856, 657, 0, 0),
            };
            EntityManager.Add(this.rightTriggerText);

            this.AddSceneBehavior(new GamePadSceneBehavior(), SceneBehavior.Order.PostUpdate);

        }
    }
}
