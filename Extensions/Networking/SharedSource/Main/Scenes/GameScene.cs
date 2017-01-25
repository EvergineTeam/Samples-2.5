using Networking.Behaviors;
using Networking.Components;
using System.Linq;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Networking;
using System;

namespace Networking.Scenes
{
    /// <summary>
    /// This scene contains the network entities that uses the SyncComponents.
    /// </summary>
    public class GameScene : Scene
    {
        private const string GameSceneIdentifier = "NetworkingSample.Game.Scene";
        private const string PlayerFactoryIdentifier = "PlayerFactory";

        private readonly NetworkService networkService;
        private readonly NetworkManager networkManager;

        private int playerSpriteIndex;

        public GameScene(int playerSpriteIndex)
        {
            this.playerSpriteIndex = playerSpriteIndex;

            this.networkService = WaveServices.GetService<NetworkService>();
            //Register the scene to use the synchronization components. This scene sync the entities in the scenes with the same sceneId in other clients.
            this.networkManager = this.networkService.RegisterScene(this, GameSceneIdentifier);
            this.networkManager.AddFactory(PlayerFactoryIdentifier, this.CreatePlayer);
        }

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.GameScene);
        }

        protected override void Start()
        {
            //When the scene start add the payer entity to NetworkManager to start to sync with other clients.
            this.networkManager.AddEntity(PlayerFactoryIdentifier);
        }

        private Entity CreatePlayer(string clientId, string behaviorId)
        {
            var playerEntity = this.EntityManager.Instantiate(WaveContent.Prefabs.PlayerEntity);
            playerEntity.Name += clientId;

            if (this.networkService.ClientIdentifier == clientId)
            {
                var random = WaveServices.Random;

                var transform = playerEntity.FindComponent<Transform2D>();
                transform.Position = new Vector2(random.Next(10, 800), random.Next(10, 200));
                transform.DrawOrder = 1.0f / this.playerSpriteIndex;

                var spriteAtlas = playerEntity.FindComponent<SpriteAtlas>();
                spriteAtlas.TextureName = string.Format("c{0}", this.playerSpriteIndex);

                playerEntity.AddComponent(new MovementBehavior());
            }

            return playerEntity;
        }
    }
}
