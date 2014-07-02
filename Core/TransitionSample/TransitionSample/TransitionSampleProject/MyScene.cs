#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.Transitions;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
#endregion

namespace TransitionSampleProject
{
    /// <summary>
    /// 
    /// </summary>
    public class MyScene : Scene
    {
        /// <summary>
        /// The transitiontime
        /// </summary>
        private static readonly TimeSpan TRANSITIONTIME = new TimeSpan(0, 0, 0, 1, 200);

        /// <summary>
        /// The scenes
        /// </summary>
        private static IList<ScreenContext> scenes = new List<ScreenContext>();

        /// <summary>
        /// The transition
        /// </summary>
        private ScreenTransition transition;

        /// <summary>
        /// The scene index
        /// </summary>
        private static int sceneIndex = 0;

        /// <summary>
        /// The ease function
        /// </summary>
        private static EasingFunctionBase easeFunction = new CubicEase() { EasingMode = EasingMode.EaseInOut }; 

        /// <summary>
        /// The index
        /// </summary>
        private int index;

        /// <summary>
        /// Creates the scene.
        /// </summary>
        /// <remarks>
        /// This method is called before all <see cref="T:WaveEngine.Framework.Entity" /> instances in this instance are initialized.
        /// </remarks>
        protected override void CreateScene()
        {
            string name = string.Empty;

            ViewCamera camera = new ViewCamera("MainCamera", new Vector3(0, 0, 3), Vector3.Zero);
            EntityManager.Add(camera.Entity);
            RenderManager.SetActiveCamera(camera.Entity);

            Entity primitive = new Entity("Primitive")
                .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(1f, 2f, 1f) })
                .AddComponent(new Transform3D())
                .AddComponent(new BoxCollider())
                .AddComponent(new MaterialsMap())
                .AddComponent(new ModelRenderer());

            switch (this.index)
            {
                case (0):
                    name = "Push";
                    primitive.AddComponent(Model.CreateTeapot());
                    RenderManager.BackgroundColor = Color.Black;
                    break;
                case (1):
                    name = "Color Fade";
                    primitive.AddComponent(Model.CreateCapsule());
                    RenderManager.BackgroundColor = Color.DarkBlue;
                    break;
                case (2):
                    name = "Doorway";
                    primitive.AddComponent(Model.CreateSphere());
                    RenderManager.BackgroundColor = Color.DarkCyan;
                    break;
                case (3):
                    name = "Comb";
                    primitive.AddComponent(Model.CreateTorus());
                    RenderManager.BackgroundColor = Color.DarkGoldenrod;
                    break;
                case (4):
                    name = "Cover";
                    primitive.AddComponent(Model.CreateCapsule());
                    RenderManager.BackgroundColor = Color.DarkGray;
                    break;
                case (5):
                    name = "Curtains";
                    primitive.AddComponent(Model.CreatePyramid());
                    RenderManager.BackgroundColor = Color.DarkGreen;
                    break;
                case (6):
                    name = "Falling Lines";
                    primitive.AddComponent(Model.CreatePlane());
                    RenderManager.BackgroundColor = Color.DarkKhaki;
                    break;
                case (7):
                    name = "Fan";
                    primitive.AddComponent(Model.CreateCone());
                    RenderManager.BackgroundColor = Color.DarkMagenta;
                    break;
                case (8):
                    name = "Rotate";
                    primitive.AddComponent(Model.CreateCube());
                    RenderManager.BackgroundColor = Color.DarkOliveGreen;
                    break;
                case (9):
                    name = "Scale";
                    primitive.AddComponent(Model.CreateTeapot());
                    RenderManager.BackgroundColor = Color.DarkOrange;
                    break;
                case (10):
                    name = "Shrink and Spin";
                    primitive.AddComponent(Model.CreateCapsule());
                    RenderManager.BackgroundColor = Color.DarkOrchid;
                    break;
                case (11):
                    name = "Spinning Squares";
                    primitive.AddComponent(Model.CreateSphere());
                    RenderManager.BackgroundColor = Color.DarkRed;
                    break;
                case (12):
                    name = "Uncover";
                    primitive.AddComponent(Model.CreateTorus());
                    RenderManager.BackgroundColor = Color.DarkSalmon;
                    break;
                case (13):
                    name = "Zoom";
                    primitive.AddComponent(Model.CreatePyramid());
                    RenderManager.BackgroundColor = Color.DarkSeaGreen;
                    break;
                case (14):
                    name = "Chequered";
                    primitive.AddComponent(Model.CreatePlane());
                    RenderManager.BackgroundColor = Color.DarkSlateBlue;
                    break;
                case (15):
                    name = "Cross Fade";
                    primitive.AddComponent(Model.CreateCube());
                    RenderManager.BackgroundColor = Color.DarkSlateGray;
                    break;
                default:
                    break;
            }

            Button button = new Button()
            {
                Text = string.Format("Next scene with {0} transition", name),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = 400,
                Margin = new Thickness(10),
            };

            button.Click += this.OnClick;

            this.EntityManager.Add(button);
            this.EntityManager.Add(primitive);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyScene" /> class.
        /// </summary>
        /// <param name="index">The index.</param>
        public MyScene(int index)
        {
            this.index = index;

            switch (this.index)
            {
                case (0):
                    this.transition = DefaultTransitions.CrossFade(TRANSITIONTIME);
                    break;
                case (1):
                    this.transition = DefaultTransitions.Push(TRANSITIONTIME, PushTransition.EffectOptions.FromLeft);
                    break;
                case (2):
                    this.transition = DefaultTransitions.ColorFade(Color.White, TRANSITIONTIME);
                    break;
                case (3):
                    this.transition = DefaultTransitions.Doorway(TRANSITIONTIME);
                    break;
                case (4):
                    this.transition = DefaultTransitions.Comb(TRANSITIONTIME, CombTransition.EffectOptions.Horizontal);
                    break;
                case (5):
                    this.transition = DefaultTransitions.Cover(TRANSITIONTIME, CoverTransition.EffectOptions.FromLeft);
                    break;
                case (6):
                    this.transition = DefaultTransitions.Curtains(TRANSITIONTIME);
                    break;
                case (7):
                    this.transition = DefaultTransitions.FallingLines(TRANSITIONTIME);
                    break;
                case (8):
                    this.transition = DefaultTransitions.Fan(TRANSITIONTIME);
                    break;
                case (9):
                    this.transition = DefaultTransitions.Rotate(TRANSITIONTIME);
                    break;
                case (10):
                    this.transition = DefaultTransitions.Scale(TRANSITIONTIME);
                    break;
                case (11):
                    this.transition = DefaultTransitions.ShrinkAndSpin(TRANSITIONTIME);
                    break;
                case (12):
                    this.transition = DefaultTransitions.SpinningSquares(TRANSITIONTIME);
                    break;
                case (13):
                    this.transition = DefaultTransitions.Uncover(TRANSITIONTIME, UncoverTransition.EffectOptions.FromLeft);
                    break;
                case (14):
                    this.transition = DefaultTransitions.Zoom(TRANSITIONTIME);
                    break;
                case (15):
                    this.transition = DefaultTransitions.ChequeredAppear(TRANSITIONTIME);
                    break;
                default:
                    break;
            }

            this.transition.EaseFunction = easeFunction;
        }

        /// <summary>
        /// Called when the user clicks the button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnClick(object sender, EventArgs e)
        {
            NextScene();
        }

        /// <summary>
        /// Goes to the next scene.
        /// </summary>
        public static void NextScene()
        {
            sceneIndex = (sceneIndex + 1) % 16;

            var context = new ScreenContext(new MyScene(sceneIndex));
            WaveServices.ScreenContextManager.To(context, (context[0] as MyScene).transition);
        }
    }
}
