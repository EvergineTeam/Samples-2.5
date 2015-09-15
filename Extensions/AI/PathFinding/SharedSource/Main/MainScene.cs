#region Using Statements
using PathFindingProject.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using WaveEngine.AI.PathFinding;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.TiledMap;
#endregion

namespace PathFindingSource
{
    public class MainScene : Scene
    {
        private TiledMap tiledMap;

        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/MainScene.wscene");           
          
            var map = EntityManager.Find("Map");
               
                map.AddComponent(this.tiledMap = new TiledMap("Content/Assets/hexagonalMap.tmx")
                {
                    MinLayerDrawOrder = -10,
                    MaxLayerDrawOrder = -0
                })
                .AddComponent(new PlayerControllerBehavior())
                .RefreshDependencies();



                var player = EntityManager.Find("Player");

            map.FindComponent<PlayerControllerBehavior>().TileSelected += (s, tile) =>
                {
                    var pathFollower = player.FindComponent<PathFollowerBehavior>();
                    pathFollower.MoveToTile(tile);
                };

#if DEBUG
            this.AddSceneBehavior(new DebugSceneBehavior(), SceneBehavior.Order.PostUpdate);
#endif
        }

        protected override void Start()
        {
            base.Start();

            var player = EntityManager.Find("Player");
            
            player.AddComponent(new AStar<LayerTile>())
                .AddComponent(new PathFinder<LayerTile>())
            .AddComponent(new PathFollowerBehavior(this.tiledMap))
            .RefreshDependencies();
        }
    }
}
