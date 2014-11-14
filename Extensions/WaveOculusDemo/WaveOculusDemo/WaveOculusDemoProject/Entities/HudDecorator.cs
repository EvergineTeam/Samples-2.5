using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Materials;
using WaveOculusDemoProject.Components;
using WaveOculusDemoProject.Layers;

namespace WaveOculusDemoProject.Entities
{
    /// <summary>
    /// HUD entity decorator
    /// </summary>
    public class HudDecorator : BaseDecorator
    {
        /// <summary>
        /// Instantiates the HUD
        /// </summary>
        /// <param name="name">The entity name</param>
        public HudDecorator(string name)
        {
            this.entity = new Entity(name)
            .AddComponent(new Transform3D() { LocalScale = Vector3.One * 3 })
            .AddComponent(new Model("Content/Models/Hud/HudStatic.FBX"))
            .AddComponent(new ModelRenderer())
            .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Hud/Hud_1.png", typeof(AdditiveLayer))))
            ;

            Entity rotationEntity = new Entity("rotation")
            .AddComponent(new Transform3D())
            .AddComponent(new Model("Content/Models/Hud/HudRotation.FBX"))
            .AddComponent(new ModelRenderer())
            .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Hud/Hud_1.png", typeof(AdditiveLayer))))
            .AddComponent(new HudRotationBehavior(new Vector3(1, 1, 0)))
            ;

            this.entity.AddChild(rotationEntity);
            
            Entity radarEntity = new Entity("radar")
            .AddComponent(new Transform3D() { LocalPosition = new Vector3(0, -0.32f, 0.5f), LocalRotation = Vector3.UnitY * MathHelper.Pi })
            .AddComponent(new Model("Content/Models/Hud/HudRadarStatic.FBX"))
            .AddComponent(new ModelRenderer())
            .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Hud/HudRadar.png", typeof(AdditiveLayer))))
            ;

            Entity rotationRadar1 = new Entity("rotationRadar1")
            .AddComponent(new Transform3D())
            .AddComponent(new Model("Content/Models/Hud/HudRadarRotation1.FBX"))
            .AddComponent(new ModelRenderer())
            .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Hud/HudRadar.png", typeof(AdditiveLayer))))
            .AddComponent(new HudRotationBehavior(new Vector3(1, 1, 0)))
            .AddComponent(new RadarBehavior())
            ;

            radarEntity.AddChild(rotationRadar1);

            Entity rotationRadar3 = new Entity("rotationRadar3")
             .AddComponent(new Transform3D())
             .AddComponent(new Model("Content/Models/Hud/HudRadarRotation3.FBX"))
             .AddComponent(new ModelRenderer())
             .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Hud/HudRadar.png", typeof(AdditiveLayer))))
             .AddComponent(new Spinner() { IncreaseY = -10 })
             ;

            rotationRadar1.AddChild(rotationRadar3);


            Entity rotationRadar2 = new Entity("rotationRadar2")
            .AddComponent(new Transform3D())
            .AddComponent(new Model("Content/Models/Hud/HudRadarRotation2.FBX"))
            .AddComponent(new ModelRenderer())
            .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Hud/HudRadar.png", typeof(AdditiveLayer))))
            .AddComponent(new HudRotationBehavior(new Vector3(1, 1, 0)))
            ;

            radarEntity.AddChild(rotationRadar2);

            this.entity.AddChild(radarEntity);
        }
    }
}
