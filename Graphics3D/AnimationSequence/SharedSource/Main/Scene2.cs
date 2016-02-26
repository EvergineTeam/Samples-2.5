using AnimationSequence.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Components.GameActions;

namespace AnimationSequence
{
    public class Scene2 : Scene
    {
        IGameAction animationSequence;
        Entity cube1, cube2;
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.Scene2);

            AnimationSlot animationSlot0_1 = new AnimationSlot()
            {
                TransformationType = AnimationSlot.TransformationTypes.Rotation,
                TotalTime = TimeSpan.FromSeconds(1.5f),
                StartRotation = new Vector3(0, 0, 0),
                EndRotation = new Vector3(0, MathHelper.ToRadians(180), 0),
            };

            AnimationSlot animationSlot1_1 = new AnimationSlot()
            {
                TransformationType = AnimationSlot.TransformationTypes.Rotation,
                TotalTime = TimeSpan.FromSeconds(1.5f),
                StartRotation = new Vector3(0, 0, 0),
                EndRotation = new Vector3(0, 0, MathHelper.ToRadians(16)),
            };

            AnimationSlot animationSlot2_1 = new AnimationSlot()
            {
                TransformationType = AnimationSlot.TransformationTypes.Rotation,
                TotalTime = TimeSpan.FromSeconds(1.5f),
                StartRotation = new Vector3(0, 0, MathHelper.ToRadians(-20)),
                EndRotation = new Vector3(0, 0, MathHelper.ToRadians(2)),
            };

            AnimationSlot animationSlot3_1 = new AnimationSlot()
            {
                TransformationType = AnimationSlot.TransformationTypes.Rotation,
                TotalTime = TimeSpan.FromSeconds(1.5f),
                StartRotation = new Vector3(0, 0, 0),
                EndRotation = new Vector3(0, 0, MathHelper.ToRadians(-20)),
            };

            AnimationSlot animationSlot0_2 = new AnimationSlot()
            {
                TransformationType = AnimationSlot.TransformationTypes.Rotation,
                TotalTime = TimeSpan.FromSeconds(1.5f),                
                StartRotation = new Vector3(0, MathHelper.ToRadians(180), 0),
                EndRotation = new Vector3(0, MathHelper.ToRadians(53), 0),
            };

            AnimationSlot animationSlot1_2 = new AnimationSlot()
            {
                TransformationType = AnimationSlot.TransformationTypes.Rotation,
                TotalTime = TimeSpan.FromSeconds(1.5f),                
                StartRotation = new Vector3(0, 0, MathHelper.ToRadians(16)),
                EndRotation = new Vector3(0, 0, MathHelper.ToRadians(-75)),
            };

            AnimationSlot animationSlot2_2 = new AnimationSlot()
            {
                TransformationType = AnimationSlot.TransformationTypes.Rotation,
                TotalTime = TimeSpan.FromSeconds(1.5f),                
                StartRotation = new Vector3(0, 0, MathHelper.ToRadians(2)),
                EndRotation = new Vector3(0, 0, MathHelper.ToRadians(50)),
            };

            AnimationSlot animationSlot3_2 = new AnimationSlot()
            {
                TransformationType = AnimationSlot.TransformationTypes.Rotation,
                TotalTime = TimeSpan.FromSeconds(1.5f),                
                StartRotation = new Vector3(0, 0, MathHelper.ToRadians(-20)),
                EndRotation = new Vector3(0, 0, MathHelper.ToRadians(-65)),
            };

            this.cube1 = this.EntityManager.Find("cube1");
            this.cube2 = this.EntityManager.Find("base.zone0.zone1.zone2.zone3.cube2");
            Animation3DBehavior animationBehavior0 = this.EntityManager.Find("base.zone0").FindComponent<Animation3DBehavior>();
            Animation3DBehavior animationBehavior1 = this.EntityManager.Find("base.zone0.zone1").FindComponent<Animation3DBehavior>();
            Animation3DBehavior animationBehavior2 = this.EntityManager.Find("base.zone0.zone1.zone2").FindComponent<Animation3DBehavior>();
            Animation3DBehavior animationBehavior3 = this.EntityManager.Find("base.zone0.zone1.zone2.zone3").FindComponent<Animation3DBehavior>();            

            this.animationSequence = this.CreateGameAction(new Animation3DGameAction(animationSlot0_1, animationBehavior0))
                                                           .CreateParallelGameActions(new List<IGameAction>() { new Animation3DGameAction(animationSlot2_1, animationBehavior2),
                                                                                                                 new Animation3DGameAction(animationSlot1_1, animationBehavior1),
                                                                                                                 new Animation3DGameAction(animationSlot3_1, animationBehavior3)
                                                                                                              }).WaitAll()
                                                           .ContinueWithAction(() =>
                                                           {
                                                               this.cube1.IsVisible = false;
                                                               this.cube2.IsVisible = true;
                                                           })
                                                           .CreateParallelGameActions(new List<IGameAction>() { new Animation3DGameAction(animationSlot0_2, animationBehavior0),
                                                                                                                 new Animation3DGameAction(animationSlot1_2, animationBehavior1),
                                                                                                                 new Animation3DGameAction(animationSlot2_2, animationBehavior2),
                                                                                                                 new Animation3DGameAction(animationSlot3_2, animationBehavior3)
                                                                                                              }).WaitAll();
        }
        
        protected override void Start()
        {
            base.Start();

            WaveServices.TimerFactory.CreateTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.animationSequence.Run();
            }, false);
        }
    }
}
