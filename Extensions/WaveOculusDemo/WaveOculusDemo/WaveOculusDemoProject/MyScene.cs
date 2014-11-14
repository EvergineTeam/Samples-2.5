#region Using Statements
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveOculusDemoProject.Components;
using WaveOculusDemoProject.Entities;
using WaveOculusDemoProject.Layers;
using WaveOculusDemoProject.Audio;
#endregion

namespace WaveOculusDemoProject
{
    public class MyScene : Scene
    {
        private float fps = 30;
        private int startFrame = 0;

        protected override void CreateScene()
        {
            this.CreateScriptManager();
            this.CreateSounds();
            this.CreateLayers();
            this.CreateWingman();
            this.CreateEnemyFighter();
            this.CreateLaunchBase();
            this.CreateEnvironment();
            this.CreateCockpit();
        }

        /// <summary>
        /// Create the script manager
        /// </summary>
        private void CreateScriptManager()
        {
            // Script Manager
            this.EntityManager.Add(new ScreenplayManagerDecorator("ScreenplayManager")
            {
                Fps = this.fps,
                CurrentFrameTime = this.startFrame
            }.Entity);
        }

        /// <summary>
        /// Load sounds used in the demo
        /// </summary>
        private void CreateSounds()
        {
            this.EntityManager.Add(new SoundManagerDecorator("soundManager"));
        }

        /// <summary>
        /// Create the cockpit entity, that contains:
        /// - The stereoscopic 3D camera,
        /// - The player controlled fighter,
        /// - The HUD
        /// </summary>
        private void CreateCockpit()
        {
            // Materials
            Dictionary<string, Material> materials = new Dictionary<string, Material>()
            {
                {
                    "Seat",
                    new BasicMaterial("Content/Textures/Cockpit.png", typeof(CockpitLayer))
                },
                {
                    "Trail",
                    new BasicMaterial("Content/Textures/Trail.wpk", DefaultLayers.Additive)
                    {
                        SamplerMode = AddressMode.LinearClamp
                    }
                }
            };

            // Player fighter entity
            var playerFighter = new Entity("Player")
            .AddComponent(new Transform3D())
            .AddComponent(new Sound3DEmitter())
            .AddComponent(new FollowPathBehavior("Content/Paths/Fighter_1.txt"))
            .AddComponent(new TrailManager())
            .AddComponent(new TrailsRenderer())
            .AddComponent(new MaterialsMap(materials))
            .AddComponent(new AnimatedParamBehavior("Content/Paths/Fighter1_Lasers.txt"))
            .AddComponent(new FighterController(FighterState.Player, new List<Vector3>() { new Vector3(-3, -1.6f, -9f), new Vector3(3f, -1.6f, -9f) }, SoundType.Engines_1, SoundType.Shoot))
            ;

            // Cockpit model
            Entity cockpitEntity = new Entity()
            .AddComponent(new Transform3D() { LocalScale = Vector3.One * 5, LocalPosition = Vector3.UnitZ * -0.8f })
            .AddComponent(new MaterialsMap(materials))
            .AddComponent(new Model("Content/Models/Cockpit.FBX"))
            .AddComponent(new ModelRenderer())
            ;

            playerFighter.AddChild(cockpitEntity);

            // Hud Entity
            var hud = new HudDecorator("hud");
            playerFighter.AddChild(hud.Entity);

            // Stereoscopic camera
            var stereoscopicCamera = new StereoscopicCameraDecorator("stereoCamera");
            playerFighter.AddChild(stereoscopicCamera.Entity);

            // Guns
            Entity projectileEmitter = new Entity() { Tag = "Gun" }
                .AddComponent(new Transform3D() { LocalPosition = new Vector3(3.376f, -2.689f, -3.499f), Orientation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.Pi) })
                .AddComponent(new ProjectileEmitter(800, 7f, 1, 0));
            playerFighter.AddChild(projectileEmitter);

            projectileEmitter = new Entity() { Tag = "Gun" }
                .AddComponent(new Transform3D() { LocalPosition = new Vector3(-3.376f, -2.689f, -3.499f), Orientation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.Pi) })
                .AddComponent(new ProjectileEmitter(800, 7f, 1, 0.5f));
            playerFighter.AddChild(projectileEmitter);

#if WINDOWS
            // In Windows platform, you must set the Oculus Rift  rendertarget and view proyections for each eye.
            var OVRService = WaveServices.GetService<WaveEngine.OculusRift.OVRService>();
            if (OVRService != null)
            {
                stereoscopicCamera.SetRenderTarget(OVRService.RenderTarget, OVRService.GetProjectionMatrix(WaveEngine.OculusRift.EyeType.Left), OVRService.GetProjectionMatrix(WaveEngine.OculusRift.EyeType.Right));
            }
#endif

            this.EntityManager.Add(playerFighter);
        }

        /// <summary>
        /// Create specific render layers
        /// </summary>
        private void CreateLayers()
        {
            this.RenderManager.RegisterLayerBefore(new StarfieldLayer(this.RenderManager), DefaultLayers.Alpha);
            this.RenderManager.RegisterLayerBefore(new PlanetLayer(this.RenderManager), DefaultLayers.Alpha);

            this.RenderManager.RegisterLayerAfter(new CockpitLayer(this.RenderManager), DefaultLayers.Additive);
            this.RenderManager.RegisterLayerAfter(new CockpitAdditiveLayer(this.RenderManager), typeof(CockpitLayer));
        }

        /// <summary>
        /// Create scene environment:
        /// - Projectiles, Explosions
        /// - Asteroid field,
        /// - Starfield
        /// </summary>
        private void CreateEnvironment()
        {
            // Projectiles
            Entity blueProjectiles = new Entity("Projectiles")
                .AddComponent(new ProjectileManager())
                .AddComponent(new ProjectilesRenderer())
                .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Blaster.png", DefaultLayers.Additive)));
            this.EntityManager.Add(blueProjectiles);

            // Explosion
            ExplosionDecorator explosion = new ExplosionDecorator("explosion", 2556);
            explosion.Transform3D.Position = new Vector3(-1109.993f, 0.437f, -3785.457f);
            this.EntityManager.Add(explosion.Entity);

            // Add asteroidfield
            var asteroidField = new AsteroidFieldDecorator("asteroidField", new Vector3(2000, 2000, 0));
            this.EntityManager.Add(asteroidField);

            // Add starfield
            var starField = new StarfieldDecorator("starfield");
            this.EntityManager.Add(starField);

            Dictionary<string, Material> materials = new Dictionary<string, Material>()
            {
                {
                    "Planet",
                    new BasicMaterial("Content/Textures/planet.png", typeof(PlanetLayer))
                },
                {
                    "BackSun",
                    new BasicMaterial("Content/Textures/StarShine.png", typeof(StarfieldLayer)){DiffuseColor = Color.White * 0.8f}
                },
                {
                    "FrontSun",
                    new BasicMaterial("Content/Textures/StarShine.png", DefaultLayers.Additive){DiffuseColor = Color.White * 0.4f}
                }
            };

            // Add background planet
            var planet = new Entity("planet")
            .AddComponent(new Transform3D())
            .AddComponent(new Model("Content/Models/Planet.FBX"))
            .AddComponent(new ModelRenderer())
            .AddComponent(new MaterialsMap(materials))
            .AddComponent(new FollowCameraBehavior())
            ;
            this.EntityManager.Add(planet);

            PointLight light = new PointLight("light", new Vector3(-30000, 17000, -15000))
            {
                Attenuation = 1900000,
                IsVisible = true,
                Color = Color.White
            };

            this.EntityManager.Add(light);
        }

        /// <summary>
        /// Create wingman fighter
        /// </summary>
        private void CreateWingman()
        {
            Dictionary<string, Material> materials = new Dictionary<string, Material>()
            {
                {
                    "Fighter",
                    new NormalMappingMaterial(
                    "Content/Textures/fighter_diffuse.png",
                    "Content/Textures/fighter_normal_spec.png")
                    {
                        AmbientColor = GameResources.AmbientColor
                    }
                },
                {
                    "Glow",
                    new BasicMaterial("Content/Textures/fighter_diffuse.png")
                    {
                        DiffuseColor = GameResources.AmbientColor
                    }
                },
                {
                    "Trail",
                    new BasicMaterial("Content/Textures/Trail.wpk", DefaultLayers.Additive)
                    {
                        SamplerMode = AddressMode.LinearClamp
                    }
                },
                {
                    "Thrust",
                    new BasicMaterial("Content/Textures/Thrust.wpk", DefaultLayers.Additive)
                    {
                        SamplerMode = AddressMode.LinearClamp
                    }                   
                }
            };

            Entity fighter = new Entity("Wingman")
                .AddComponent(new Transform3D())
                .AddComponent(new Sound3DEmitter())
                .AddComponent(new FollowPathBehavior("Content/Paths/Fighter_2.txt", Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.Pi)))
                .AddComponent(new AnimatedParamBehavior("Content/Paths/Fighter2_Lasers.txt"))
                .AddComponent(new Model("Content/Models/Fighter.FBX"))
                .AddComponent(new MaterialsMap(materials))
                .AddComponent(new ModelRenderer())
                .AddComponent(new TrailManager())
                .AddComponent(new TrailsRenderer())
                .AddComponent(new FighterController(FighterState.Wingman, new List<Vector3>() { new Vector3(-3, 0, 6.8f), new Vector3(3f, 0, 6.8f) }, SoundType.Engines_2, SoundType.Shoot))
            ;

            EntityManager.Add(fighter);

            // Wingman guns
            Entity projectileEmitter = new Entity() { Tag = "Gun" }
                .AddComponent(new Transform3D() { LocalPosition = new Vector3(3.376f, -0.689f, -3.499f) })
                .AddComponent(new ProjectileEmitter(800, 7f, 1, 0));
            fighter.AddChild(projectileEmitter);

            projectileEmitter = new Entity() { Tag = "Gun" }
                .AddComponent(new Transform3D() { LocalPosition = new Vector3(-3.376f, -0.689f, -3.499f) })
                .AddComponent(new ProjectileEmitter(800, 7f, 1, 0.5f));
            fighter.AddChild(projectileEmitter);
        }

        /// <summary>
        /// Create the enemy fighter entity
        /// </summary>
        private void CreateEnemyFighter()
        {
            Dictionary<string, Material> materials = new Dictionary<string, Material>()
            {
                {
                    "EnemyFighter",
                    new NormalMappingMaterial(
                    "Content/Textures/enemyfighter_diffuse.png",
                    "Content/Textures/enemyfighter_normal_spec.png")
                    {
                        AmbientColor = GameResources.AmbientColor
                    }
                },
                {
                    "EnemyGlow",
                    new BasicMaterial("Content/Textures/enemyfighter_diffuse.png")
                    {
                        DiffuseColor = GameResources.AmbientColor
                    }
                },
                {
                    "Trail",
                    new BasicMaterial("Content/Textures/EnemyTrail.wpk", DefaultLayers.Additive)
                    {
                        SamplerMode = AddressMode.LinearClamp
                    }
                },
                {
                    "EnemyThrust",
                    new BasicMaterial("Content/Textures/EnemyThrust.wpk", DefaultLayers.Additive)
                    {
                        SamplerMode = AddressMode.LinearClamp
                    }                   
                }
            };

            Entity fighter = new Entity()
                .AddComponent(new Transform3D())
                .AddComponent(new Sound3DEmitter())
                .AddComponent(new FollowPathBehavior("Content/Paths/EnemyFighter.txt", Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.Pi)))
                .AddComponent(new AnimatedParamBehavior("Content/Paths/Enemy_Lasers.txt"))
                .AddComponent(new Model("Content/Models/EnemyFighter.FBX"))
                .AddComponent(new MaterialsMap(materials))
                .AddComponent(new ModelRenderer())
                .AddComponent(new TrailManager())
                .AddComponent(new TrailsRenderer())
                .AddComponent(new FighterController(FighterState.Enemy, new List<Vector3>() 
                { 
                    new Vector3(-4.5f, -3.7f, 9),
                    new Vector3(4.5f , -3.7f, 9),
                    new Vector3(-4.5f, 3.7f, 9),
                    new Vector3(4.5f , 3.7f, 9),
                }, SoundType.Engines_2, SoundType.Shoot))
                ;

            EntityManager.Add(fighter);

            // Gun entities
            Entity projectileEmitter = new Entity() { Tag = "Gun" }
                .AddComponent(new Transform3D() { LocalPosition = new Vector3(9.197f, 1.99f, -13.959f) })
                .AddComponent(new ProjectileEmitter(800, 5f, 1, 0));
            fighter.AddChild(projectileEmitter);

            projectileEmitter = new Entity() { Tag = "Gun" }
                .AddComponent(new Transform3D() { LocalPosition = new Vector3(-9.197f, 1.99f, -13.959f) })
                .AddComponent(new ProjectileEmitter(800, 5f, 1, 0.75f));
            fighter.AddChild(projectileEmitter);

            projectileEmitter = new Entity() { Tag = "Gun" }
                .AddComponent(new Transform3D() { LocalPosition = new Vector3(9.197f, -1.99f, -13.959f) })
                .AddComponent(new ProjectileEmitter(800, 5f, 1, 0.5f));
            fighter.AddChild(projectileEmitter);

            projectileEmitter = new Entity() { Tag = "Gun" }
                .AddComponent(new Transform3D() { LocalPosition = new Vector3(-9.197f, -1.99f, -13.959f) })
                .AddComponent(new ProjectileEmitter(800, 5f, 1, 0.25f));
            fighter.AddChild(projectileEmitter);
        }

        /// <summary>
        /// Launch base entity
        /// </summary>
        private void CreateLaunchBase()
        {
            Dictionary<string, Material> materials = new Dictionary<string, Material>()
            {
                {
                    "SpaceStation",
                    new NormalMappingMaterial(
                    "Content/Textures/spacestation_diffuse.jpg",
                    "Content/Textures/spacestation_normal_spec.png")
                    {
                        AmbientColor = GameResources.AmbientColor
                    }
                },
                {
                    "Glow",
                    new BasicMaterial("Content/Textures/spacestation_glow.png", DefaultLayers.Additive)
                }   
            };

            Entity launchBase = new Entity()
                .AddComponent(new Transform3D())
                .AddComponent(new Model("Content/Models/LaunchBase.FBX"))
                .AddComponent(new BoxCollider())
                .AddComponent(new MaterialsMap(materials))
                .AddComponent(new ModelRenderer())
                ;

            EntityManager.Add(launchBase);
        }
    }
}