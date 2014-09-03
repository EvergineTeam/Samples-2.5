#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace AnimatedModelProject
{
    public class MyScene : Scene
    {
        Animation3D anim;

        protected override void CreateScene()
        {            
            ViewCamera camera = new ViewCamera("MainCamera", new Vector3(2, 1, 2), new Vector3(0, 1, 0));
            camera.BackgroundColor = Color.CornflowerBlue;
            EntityManager.Add(camera.Entity);            

            Entity animatedModel = new Entity("Isis")
                .AddComponent(new Transform3D())
                .AddComponent(new BoxCollider())
                .AddComponent(new SkinnedModel("Content/isis.wpk"))
                .AddComponent(new MaterialsMap(new BasicMaterial("Content/isis-difuse.wpk")))
                .AddComponent(new Animation3D("Content/isis-animations.wpk"))
                .AddComponent(new SkinnedModelRenderer());

            anim = animatedModel.FindComponent<Animation3D>();
            EntityManager.Add(animatedModel);
        }

        protected override void Start()
        {
            base.Start();

            anim.PlayAnimation("Jog", true);
            //Walk
            //Idle
        }
    }
}
