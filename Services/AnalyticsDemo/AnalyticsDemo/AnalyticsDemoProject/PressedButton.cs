#region File Description
//-----------------------------------------------------------------------------
// PressedButton
//
// Copyright © 2010 - 2013 Wave Coorporation. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Gestures;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.UI;
#endregion

namespace SaveThePrincessProject.Commons
{
    /// <summary>
    /// Button decorate class
    /// </summary>
    public class PressedButton : UIBase
    {
        /// <summary>
        /// The instances
        /// </summary>
        private static int instances;

        #region Constants
        /// <summary>
        /// The default margin
        /// </summary>
        private readonly Thickness defaultMargin = new Thickness(5);

        /// <summary>
        /// The default width
        /// </summary>
        private const int DefaultWidth = 100;

        /// <summary>
        /// The default height
        /// </summary>
        private const int DefaultHeight = 40;
        #endregion

        /// <summary>
        /// Path to the background image showed when the button is being pressed.
        /// </summary>
        private string pressedBackgroundImage;

        /// <summary>
        /// Indicates whether back to render the default background image on the released event.
        /// </summary>
        private bool backToBackgroundImage;

        /// <summary>
        /// Path to the default background image.
        /// </summary>
        private string backgroundImage;

        /// <summary>
        /// Occurs when [click].
        /// </summary>
        public event EventHandler Click;

        #region Properties

        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        /// <value>
        /// The margin.
        /// </value>
        public Thickness Margin
        {
            get
            {
                return this.entity.FindComponent<PanelControl>().Margin;
            }

            set
            {
                this.entity.FindComponent<PanelControl>().Margin = value;
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get
            {
                return this.entity.FindChild("TextEntity").FindComponent<TextControl>().Text;
            }

            set
            {
                this.entity.FindChild("TextEntity").FindComponent<TextControl>().Text = value;
            }
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public string FontPath
        {
            set
            {
                Entity textEntity = this.entity.FindChild("TextEntity");
                TextControl textBlock = textEntity.FindComponent<TextControl>();
                textEntity.RemoveComponent<TextControl>();
                textEntity.AddComponent(new TextControl(value)
                {
                    Text = textBlock.Text,
                    Foreground = textBlock.Foreground,
                    Margin = textBlock.Margin,
                    HorizontalAlignment = textBlock.HorizontalAlignment,
                    VerticalAlignment = textBlock.VerticalAlignment,
                    LineSpacing = textBlock.LineSpacing,
                    LineWidth = textBlock.LineWidth,
                    TouchMargin = textBlock.TouchMargin,
                    TextWrapping = textBlock.TextWrapping
                });

                textEntity.RefreshDependencies();
            }
        }

        /// <summary>
        /// Gets or sets the foreground.
        /// </summary>
        /// <value>
        /// The foreground.
        /// </value>
        public Color Foreground
        {
            get
            {
                return this.entity.FindChild("TextEntity").FindComponent<TextControl>().Foreground;
            }

            set
            {
                this.entity.FindChild("TextEntity").FindComponent<TextControl>().Foreground = value;
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public float Width
        {
            get
            {
                return this.entity.FindComponent<PanelControl>().Width;
            }

            set
            {
                this.entity.FindComponent<PanelControl>().Width = value;
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public float Height
        {
            get
            {
                return this.entity.FindComponent<PanelControl>().Height;
            }

            set
            {
                this.entity.FindComponent<PanelControl>().Height = value;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment.
        /// </summary>
        /// <value>
        /// The horizontal alignment.
        /// </value>
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return this.entity.FindComponent<PanelControl>().HorizontalAlignment;
            }

            set
            {
                this.entity.FindComponent<PanelControl>().HorizontalAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal text alignment.
        /// </summary>
        /// <value>
        /// The horizontal text alignment.
        /// </value>
        public HorizontalAlignment HorizontalTextAlignment
        {
            get
            {
                return this.entity.FindChild("TextEntity").FindComponent<TextControl>().HorizontalAlignment;
            }

            set
            {
                this.entity.FindChild("TextEntity").FindComponent<TextControl>().HorizontalAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment.
        /// </summary>
        /// <value>
        /// The vertical alignment.
        /// </value>
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return this.entity.FindComponent<PanelControl>().VerticalAlignment;
            }

            set
            {
                this.entity.FindComponent<PanelControl>().VerticalAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets the vertical text alignment.
        /// </summary>
        /// <value>
        /// The vertical text alignment.
        /// </value>
        public VerticalAlignment VerticalTextAlignment
        {
            get
            {
                return this.entity.FindChild("TextEntity").FindComponent<TextControl>().VerticalAlignment;
            }

            set
            {
                this.entity.FindChild("TextEntity").FindComponent<TextControl>().VerticalAlignment = value;
            }
        }

        /// <summary>
        /// Sets the background image.
        /// </summary>
        /// <value>
        /// The background image.
        /// </value>
        public string BackgroundImage
        {
            set
            {
                this.backgroundImage = value;
                this.ChangeBackgroundImage(value);
            }
        }

        /// <summary>
        /// Sets the color of the background.
        /// </summary>
        /// <value>
        /// The color of the background.
        /// </value>
        public Color BackgroundColor
        {
            set
            {
                Entity imageEntity = this.entity.FindChild("ImageEntity");

                if (imageEntity != null)
                {
                    // If imageEntity exist
                    imageEntity.FindComponent<ImageControl>().TintColor = value;
                }
                else
                {
                    // If imageEntity doesn't exist
                    this.entity.AddChild(new Entity("ImageEntity")
                                    .AddComponent(new Transform2D()
                                    {
                                        DrawOrder = 0.5f
                                    })
                                    .AddComponent(new ImageControl(value, 1, 1)
                                    {
                                        Stretch = Stretch.Fill
                                    })
                                    .AddComponent(new ImageControlRenderer()));

                    this.entity.RefreshDependencies();
                }
            }
        }

        /// <summary>
        /// Sets the pressed background image.
        /// </summary>
        /// <value>
        /// The pressed background image.
        /// </value>
        public string PressedBackgroundImage
        {
            set
            {
                this.pressedBackgroundImage = value;
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes a new instance of the <see cref="Button" /> class.
        /// </summary>
        public PressedButton()
            : this("PressedButton" + instances++)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public PressedButton(string name)
        {
            this.backToBackgroundImage = false;

            var touchGestures = new TouchGestures();
            this.entity = new Entity(name)
                           .AddComponent(new Transform2D())
                           .AddComponent(new RectangleCollider())
                           .AddComponent(touchGestures)
                           .AddComponent(new ButtonBehavior())
                           .AddComponent(new PanelControl(DefaultWidth, DefaultHeight))
                           .AddComponent(new PanelControlRenderer())
                           .AddComponent(new BorderRenderer())
                           .AddChild(new Entity("TextEntity")
                                .AddComponent(new Transform2D()
                                {
                                    DrawOrder = 0.4f
                                })
                                .AddComponent(new AnimationUI())
                                .AddComponent(new TextControl()
                                {
                                    Text = "Button",
                                    Margin = this.defaultMargin,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center
                                })
                                .AddComponent(new TextControlRenderer()));

            // Event
            touchGestures.TouchReleased += this.Button_TouchReleased;

            touchGestures.TouchPressed += touchGestures_TouchPressed;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Handles the TouchReleased event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GestureEventArgs" /> instance containing the event data.</param>
        private void Button_TouchReleased(object sender, GestureEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.pressedBackgroundImage) && this.backToBackgroundImage)
            {
                this.backToBackgroundImage = false;
                this.ChangeBackgroundImage(this.backgroundImage);
            }

            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }

        /// <summary>
        /// If a pressed background image is set, it draws this last one instead of background image, 
        /// just up to the released event is catched.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GestureEventArgs" /> instance containing the event data.</param>
        private void touchGestures_TouchPressed(object sender, GestureEventArgs e)
        {
            // Asking for !this.backToBackgroundImage avoids to execute the if when has been done once before
            if (!string.IsNullOrWhiteSpace(this.pressedBackgroundImage) && !this.backToBackgroundImage)
            {
                this.ChangeBackgroundImage(this.pressedBackgroundImage);
                this.backToBackgroundImage = true;
            }
        }

        /// <summary>
        /// Modifies the background image with the new asset path.
        /// </summary>
        /// <param name="imagePath">Path to the background image</param>
        private void ChangeBackgroundImage(string imagePath)
        {
            Entity imageEntity = this.entity.FindChild("ImageEntity");

            if (imageEntity != null)
            {
                // If imageEntity exist
                imageEntity.RemoveComponent<ImageControl>();
                imageEntity.AddComponent(new ImageControl(imagePath)
                {
                    Stretch = Stretch.Fill
                });

                imageEntity.RefreshDependencies();
            }
            else
            {
                // If imageEntity doesn't exist
                this.entity.AddChild(new Entity("ImageEntity")
                    .AddComponent(new Transform2D()
                    {
                        DrawOrder = 0.5f
                    })
                    .AddComponent(new ImageControl(imagePath)
                    {
                        Stretch = Stretch.Fill
                    })
                    .AddComponent(new ImageControlRenderer()));

                this.entity.RefreshDependencies();
            }
        }
        #endregion
    }
}
