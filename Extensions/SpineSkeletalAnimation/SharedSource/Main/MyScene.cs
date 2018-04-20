#region Using Statements
using System.Linq;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WaveEngine.Spine;
#endregion

namespace SpineSkeletalAnimation
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            var skeletalAnimations = this.EntityManager.FindComponentsOfType<SkeletalAnimation>();

            #region UI
            var speedSlider = new Slider()
            {
                Margin = new Thickness(10, 40, 0, 0),
                Width = 500,
                Minimum = -25,
                Maximum = 25,
                Value = 10
            };

            speedSlider.RealTimeValueChanged += (s, e) =>
            {
                foreach (var anim in skeletalAnimations)
                {
                    anim.Speed = e.NewValue / 10f;
                }
            };
            this.EntityManager.Add(speedSlider);

            var debugMode = new ToggleSwitch()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 10, 0),
                OnText = "Debug On",
                OffText = "Debug Off",
                Width = 200
            };

            debugMode.Toggled += (s, o) =>
            {
                var isOn = ((ToggleSwitch)s).IsOn;

                WaveServices.ScreenContextManager.SetDiagnosticsActive(isOn);
                RenderManager.DebugLines = isOn;
            };

            this.EntityManager.Add(debugMode);

            var debugBones = new CheckBox()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 60, 10, 0),
                IsChecked = true,
                Text = "Bones",
                Width = 150,
            };

            debugBones.Checked += (s, o) =>
            {
                if (o.Value)
                {
                    this.RenderManager.DebugFlags |= WaveEngine.Framework.Managers.DebugLinesFlags.Bones;
                }
                else
                {
                    this.RenderManager.DebugFlags &= ~WaveEngine.Framework.Managers.DebugLinesFlags.Bones;
                }
            };

            this.EntityManager.Add(debugBones);
            #endregion
        }
    }
}
