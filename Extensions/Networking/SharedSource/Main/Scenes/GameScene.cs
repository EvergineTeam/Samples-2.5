using Networking.Behaviors;
using Networking.Components;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Networking;

namespace Networking.Scenes
{
    /// <summary>
    /// This scene contains the network entities that uses the SyncComponents.
    /// </summary>
    public class GameScene : Scene
    {
        private const string GameSceneIdentifier = "NetworkingSample.Game.Scene";

        private readonly NetworkService networkService;
        private readonly NetworkManager networkManager;

        private int playerSpriteIndex;
        private Entity playerEntity;

        public GameScene(int playerSpriteIndex)
        {
            this.playerSpriteIndex = playerSpriteIndex;

            this.networkService = WaveServices.GetService<NetworkService>();
            //Register the scene to use the synchronization components. This scene sync the entities in the scenes with the same sceneId in other clients.
            this.networkManager = this.networkService.RegisterScene(this, GameSceneIdentifier);
        }

        protected override void CreateScene()
        {
            var camera2D = new FixedCamera2D("Camera2D")
            {
                BackgroundColor = Color.CornflowerBlue
            };
            this.EntityManager.Add(camera2D);

            //Create my entity with random start position.
            var random = WaveServices.Random;
            var sprite = string.Format("Content/Assets/c{0}", this.playerSpriteIndex);
            this.playerEntity = new Entity("Player_" + this.playerSpriteIndex)
                .AddComponent(new Transform2D
                {
                    Position = new Vector2(random.Next(10, 800), random.Next(10, 200)),
                    DrawOrder = 1.0f / this.playerSpriteIndex
                })
                .AddComponent(new Sprite(sprite))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new NetworkBehavior())
                .AddComponent(new SyncPositionComponent())
                .AddComponent(new MovementBehavior());

            EntityManager.Add(this.playerEntity);
        }

        protected override void Start()
        {
            //When the scene start add the payer entity to NetworkManager to start to sync with other clients.
            this.networkManager.AddEntity(this.playerEntity);
        }
    }
}
