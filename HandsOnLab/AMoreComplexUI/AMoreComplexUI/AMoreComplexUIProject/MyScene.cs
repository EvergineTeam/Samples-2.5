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
using WaveEngine.Materials;
#endregion

namespace AMoreComplexUIProject
{
    public class MyScene : Scene
    {
        private Color backgroundColor = new Color(163 / 255f, 178 / 255f, 97 / 255f);
        private Color darkColor = new Color(120 / 255f, 39 / 255f, 72 / 255f);
        private Color lightColor = new Color(157 / 255f, 73 / 255f, 133 / 255f);

        private Transform3D cubeTransform;
        private BasicMaterial cubeMaterial;

        private Slider sliderRotX;
        private Slider sliderRotY;

        protected override void CreateScene()
        {
            //Create a 3D camera
            var camera3D = new FixedCamera("Camera3D", new Vector3(2f, 0f, 2.8f), new Vector3(.5f, 0, 0)) { BackgroundColor = backgroundColor };
            EntityManager.Add(camera3D);

            this.CreateCube3D();

            // Create a 2D camera
            var camera2D = new FixedCamera2D("Camera2D") { ClearFlags = ClearFlags.DepthAndStencil }; // Transparent background need this clearFlags.
            EntityManager.Add(camera2D);

            this.CreateSliders();

            this.CreateGrid();

            this.CreateDebugMode();
        }

        private void CreateDebugMode()
        {
            ToggleSwitch debugMode = new ToggleSwitch()
            {
                OnText = "Debug On",
                OffText = "Debug Off",
                Margin = new Thickness(5),
                Width = 200,
                Foreground = darkColor,
                Background = lightColor,
            };

            debugMode.Toggled += (s, o) =>
            {
                RenderManager.DebugLines = debugMode.IsOn;
            };

            EntityManager.Add(debugMode.Entity);
        }

        private void CreateGrid()
        {
            Grid grid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Height = WaveServices.Platform.ScreenHeight,
            };

            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Proportional) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4, GridUnitType.Proportional) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Proportional) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Proportional) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Proportional) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Proportional) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Proportional) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200, GridUnitType.Pixel) });

            EntityManager.Add(grid);

            #region Color UI
            TextBlock t_colors = new TextBlock()
                {
                    Text = "Colors",
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(10),
                };

            t_colors.SetValue(GridControl.RowProperty, 0);
            t_colors.SetValue(GridControl.ColumnProperty, 0);

            grid.Add(t_colors);

            StackPanel stackPanel = new StackPanel()
            {
                Margin = new Thickness(30, 0, 0, 0),
            };

            stackPanel.SetValue(GridControl.RowProperty, 1);
            stackPanel.SetValue(GridControl.ColumnProperty, 0);

            grid.Add(stackPanel);

            RadioButton radio1 = new RadioButton()
            {
                Text = "Red",
                GroupName = "colors",
                Foreground = Color.Red,
            };

            radio1.Checked += (s, o) =>
            {
                this.cubeMaterial.DiffuseColor = Color.Red;
            };

            stackPanel.Add(radio1);

            RadioButton radio2 = new RadioButton()
            {
                Text = "Green",
                GroupName = "colors",
                Foreground = Color.Green,
            };

            radio2.Checked += (s, o) =>
            {
                this.cubeMaterial.DiffuseColor = Color.Green;
            };

            stackPanel.Add(radio2);

            RadioButton radio3 = new RadioButton()
            {
                Text = "Blue",
                GroupName = "colors",
                Foreground = Color.Blue,
            };

            radio3.Checked += (s, o) =>
            {
                this.cubeMaterial.DiffuseColor = Color.Blue;
            };

            stackPanel.Add(radio3);

            RadioButton radio4 = new RadioButton()
            {
                Text = "White",
                GroupName = "colors",
                Foreground = Color.White,
                IsChecked = true,
            };

            radio4.Checked += (s, o) =>
            {
                this.cubeMaterial.DiffuseColor = Color.White;
            };

            stackPanel.Add(radio4); 
            #endregion

            #region Texture UI
            TextBlock t_texture = new TextBlock()
                {
                    Text = "Textures",
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(10),
                };

            t_texture.SetValue(GridControl.RowProperty, 2);
            t_texture.SetValue(GridControl.ColumnProperty, 0);

            grid.Add(t_texture);

            ToggleSwitch toggleTexture = new ToggleSwitch()
            {
                Margin = new Thickness(30, 0, 0, 0),
                Foreground = darkColor,
                Background = lightColor,
                IsOn = true,
            };

            toggleTexture.Toggled += (s, o) =>
            {
                this.cubeMaterial.TextureEnabled = toggleTexture.IsOn;
            };

            toggleTexture.SetValue(GridControl.RowProperty, 3);
            toggleTexture.SetValue(GridControl.ColumnProperty, 0);

            grid.Add(toggleTexture); 
            #endregion

            #region Scale UI
            TextBlock t_scale = new TextBlock()
                {
                    Text = "Scale",
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(10),
                };

            t_scale.SetValue(GridControl.RowProperty, 4);
            t_scale.SetValue(GridControl.ColumnProperty, 0);

            grid.Add(t_scale);

            Slider sliderScale = new Slider()
            {
                Width = 150,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(30, 0, 0, 0),
                Foreground = darkColor,
                Background = lightColor,
                Value = 50,
            };

            sliderScale.RealTimeValueChanged += (s, o) =>
            {
                this.cubeTransform.Scale = Vector3.One / 2 + (Vector3.One * (o.NewValue / 100f));
            };

            sliderScale.SetValue(GridControl.RowProperty, 5);
            sliderScale.SetValue(GridControl.ColumnProperty, 0);

            grid.Add(sliderScale); 
            #endregion

            #region Reset UI
            Button b_reset = new Button()
                {
                    Text = "Reset",
                    Margin = new Thickness(10, 0, 0, 20),
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Foreground = Color.White,
                    BackgroundColor = lightColor,
                };

            b_reset.Click += (s, o) =>
            {
                radio4.IsChecked = true;
                toggleTexture.IsOn = true;
                sliderScale.Value = 50;
                this.sliderRotX.Value = 0;
                this.sliderRotY.Value = 0;
            };

            b_reset.SetValue(GridControl.RowProperty, 6);
            b_reset.SetValue(GridControl.ColumnProperty, 0);

            grid.Add(b_reset); 
            #endregion
        }

        private void CreateSliders()
        {
            TextBlock t_rotX = new TextBlock()
            {
                Text = "Rot X",
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(15, 0, 0, 60),
            };

            EntityManager.Add(t_rotX);

            this.sliderRotX = new Slider()
            {
                Margin = new Thickness(50, 0, 0, 0),
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Center,
                Height = 360,
                Width = 40,
                Minimum = 0,
                Maximum = 360,
                Foreground = darkColor,
                Background = lightColor,
            };
            this.sliderRotX.RealTimeValueChanged += this.sliderRot_RealTimeValueChanged;

            EntityManager.Add(this.sliderRotX);

            TextBlock t_rotY = new TextBlock()
            {
                Text = "Rot Y",
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(40, 0, 0, 40),
            };

            EntityManager.Add(t_rotY);

            this.sliderRotY = new Slider()
            {
                Margin = new Thickness(100, 20, 0, 32),
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = 360,
                Height = 40,
                Minimum = 0,
                Maximum = 360,
                Foreground = darkColor,
                Background = lightColor,
            };
            this.sliderRotY.RealTimeValueChanged += this.sliderRot_RealTimeValueChanged;

            EntityManager.Add(this.sliderRotY);
        }

        private void sliderRot_RealTimeValueChanged(object sender, ChangedEventArgs e)
        {
            this.cubeTransform.LocalRotation = new Vector3(MathHelper.ToRadians(this.sliderRotX.Value), MathHelper.ToRadians(this.sliderRotY.Value), 0);
        }

        private void CreateCube3D()
        {
            Entity cube = new Entity("Cube")
                 .AddComponent(new Transform3D())
                 .AddComponent(Model.CreateCube())
                 .AddComponent(new MaterialsMap(new BasicMaterial("Content/crate.jpg")))
                 .AddComponent(new ModelRenderer());


            this.cubeTransform = cube.FindComponent<Transform3D>();
            this.cubeMaterial = (BasicMaterial)cube.FindComponent<MaterialsMap>().DefaultMaterial;

            EntityManager.Add(cube);
        }
    }
}
