#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
#endregion

namespace XboxControllerProject
{
    public class MyScene : Scene
    {
        public Entity leftJoystick, rightJoystick, buttonA, buttonB, buttonX, buttonY, buttonBack, buttonStart, dpadUp, dpadDown, dpadLeft, dpadRight, leftShoulder, rightShoulder, leftTrigger, rightTrigger;
        public TextBlock leftStickText, rightStickText, leftTriggerText, rightTriggerText;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.White;

            // FrontController
            Entity frontController = new Entity()
                                            .AddComponent(new Transform2D()
                                            {
                                                X = 67,
                                                Y = 203,
                                                DrawOrder = 0.9f,
                                            })
                                            .AddComponent(new Sprite("Content/FrontController.wpk"))
                                            .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));
            EntityManager.Add(frontController);

            // Top Controller
            Entity topController = new Entity()
                                            .AddComponent(new Transform2D()
                                            {
                                                X = 656,
                                                Y = 249,
                                                DrawOrder = 0.9f,
                                            })
                                            .AddComponent(new Sprite("Content/TopController.wpk"))
                                            .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));
            EntityManager.Add(topController);

            // Sticks
            this.leftJoystick = new Entity()
                                        .AddComponent(new Transform2D()
                                        {
                                            Origin = Vector2.One / 2,
                                            Rectangle = new RectangleF(183, 311, 75, 75),
                                            DrawOrder = 0.5f,
                                        })
                                        .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "JoyStick"))
                                        .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(leftJoystick);

            this.rightJoystick = new Entity()
                                        .AddComponent(new Transform2D()
                                        {
                                            Origin = Vector2.One / 2,
                                            Rectangle = new RectangleF(427, 407, 75, 75),
                                            DrawOrder = 0.5f,
                                        })
                                        .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "JoyStick"))
                                        .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(rightJoystick);

            // Button A
            this.buttonA = new Entity()
                                        .AddComponent(new Transform2D()
                                        {
                                            Origin = Vector2.One / 2,
                                            X = 509,
                                            Y = 349,
                                            DrawOrder = 0.5f,
                                        })
                                        .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "circlePressed"))
                                        .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(buttonA);

            // Button B
            this.buttonB = new Entity()
                                        .AddComponent(new Transform2D()
                                        {
                                            Origin = Vector2.One / 2,
                                            X = 552,
                                            Y = 305,
                                            DrawOrder = 0.5f,
                                        })
                                        .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "circlePressed"))
                                        .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(buttonB);

            // ButtonX
            this.buttonX = new Entity()
                                       .AddComponent(new Transform2D()
                                       {
                                           Origin = Vector2.One / 2,
                                           X = 465,
                                           Y = 305,
                                           DrawOrder = 0.5f,
                                       })
                                       .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "circlePressed"))
                                       .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(buttonX);

            // ButtonY
            this.buttonY = new Entity()
                                       .AddComponent(new Transform2D()
                                       {
                                           Origin = Vector2.One / 2,
                                           X = 507,
                                           Y = 261,
                                           DrawOrder = 0.5f,
                                       })
                                       .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "circlePressed"))
                                       .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(buttonY);

            // Button Back
            this.buttonBack = new Entity()
                                       .AddComponent(new Transform2D()
                                       {
                                           Origin = Vector2.One / 2,
                                           X = 288,
                                           Y = 312,
                                           DrawOrder = 0.5f,
                                       })
                                       .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "rectangularPressed"))
                                       .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(buttonBack);

            // Start
            this.buttonStart = new Entity()
                                      .AddComponent(new Transform2D()
                                      {
                                          Origin = Vector2.One / 2,
                                          X = 406,
                                          Y = 310,
                                          DrawOrder = 0.5f,
                                      })
                                      .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "rectangularPressed"))
                                      .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(buttonStart);

            // DPad Up
            this.dpadUp = new Entity()
                                    .AddComponent(new Transform2D()
                                    {
                                        Origin = Vector2.One / 2,
                                        X = 262,
                                        Y = 381,
                                        DrawOrder = 0.5f,
                                    })
                                      .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "DPad"))
                                      .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(dpadUp);

            // DPad Down
            this.dpadDown = new Entity()
                                   .AddComponent(new Transform2D()
                                   {
                                       Origin = Vector2.One / 2,
                                       Rotation = (float)Math.PI,
                                       X = 262,
                                       Y = 435,
                                       DrawOrder = 0.5f,
                                   })
                                     .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "DPad"))
                                     .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(dpadDown);

            // DPad Left
            this.dpadLeft = new Entity()
                                      .AddComponent(new Transform2D()
                                      {
                                          Origin = Vector2.One / 2,
                                          Rotation = (float)-Math.PI / 2,
                                          X = 231,
                                          Y = 408,
                                          DrawOrder = 0.5f,
                                      })
                                     .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "DPad"))
                                     .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(dpadLeft);

            // DPad Right
            this.dpadRight = new Entity()
                                      .AddComponent(new Transform2D()
                                      {
                                          Origin = Vector2.One / 2,
                                          Rotation = (float)Math.PI / 2,
                                          X = 289,
                                          Y = 408,
                                          DrawOrder = 0.5f,
                                      })
                                     .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "DPad"))
                                     .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(dpadRight);

            // Left shoulder
            this.leftShoulder = new Entity()
                                      .AddComponent(new Transform2D()
                                      {
                                          Origin = Vector2.One / 2,
                                          X = 739,
                                          Y = 364,
                                          DrawOrder = 0.5f,
                                      })
                                     .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "LeftShoulderPressed"))
                                     .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(leftShoulder);

            // Right shoulder
            this.rightShoulder = new Entity()
                                      .AddComponent(new Transform2D()
                                      {
                                          Origin = Vector2.One / 2,
                                          X = 1130,
                                          Y = 364,
                                          DrawOrder = 0.5f,
                                      })
                                     .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "RightShoulder"))
                                     .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(rightShoulder);

            // Left trigger
            this.leftTrigger = new Entity()
                                      .AddComponent(new Transform2D()
                                      {
                                          Origin = Vector2.One / 2,
                                          X = 759,
                                          Y = 291,
                                          DrawOrder = 0.5f,
                                      })
                                     .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "LeftTrigger"))
                                     .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(leftTrigger);

            // Right trigger
            this.rightTrigger = new Entity()
                                      .AddComponent(new Transform2D()
                                      {
                                          Origin = Vector2.One / 2,
                                          X = 1112,
                                          Y = 291,
                                          DrawOrder = 0.5f,
                                      })
                                     .AddComponent(new SpriteAtlas("Content/Buttons.wpk", "RightTrigger"))
                                     .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Opaque));
            EntityManager.Add(rightTrigger);

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
