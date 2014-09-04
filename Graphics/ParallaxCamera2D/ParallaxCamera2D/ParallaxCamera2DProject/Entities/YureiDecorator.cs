using ParallaxCamera2DProject.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Spine;

namespace ParallaxCamera2DProject.Entities
{
    public class YureiDecorator : BaseDecorator
    {
        public WaveEngine.Spine.SkeletalAnimation SkeletalAnimation
        {
            get
            {
                return this.entity.FindComponent<WaveEngine.Spine.SkeletalAnimation>();
            }
        }
        public YureiDecorator(string name, Vector3 position, Vector3 scale, Entity dustEntity, Type layer)
        {
            Transform2D transform = new Transform2D();
            transform.Transform3D.Position = position;
            transform.Transform3D.Scale = scale;

            this.entity = new Entity(name)
            .AddComponent(transform)
            .AddComponent(new YureiBehavior(dustEntity))
            .AddComponent(new SkeletalData("Content/Yurei/yurei.spine"))
            .AddComponent(new SkeletalAnimation("Content/Yurei/yurei.json"))
            .AddComponent(new SkeletalRenderer(layer) { ActualDebugMode = WaveEngine.Spine.SkeletalRenderer.DebugMode.Quads });
            ;
        }
    }
}
