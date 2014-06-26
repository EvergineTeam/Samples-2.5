using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace PiedraPapelOTijeraProject.Entities
{
    class CpuMoveHandEntity : BaseDecorator
    {
        public GameplayHandMovesEnum CurrentHandMove { get; set; }

        public CpuMoveHandEntity()
            : base()
        {
            this.entity = new Entity("CPU move hand sprite")
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new Transform2D
                {
                    Origin = Vector2.UnitX / 2,
                    X = WaveServices.ViewportManager.VirtualWidth / 2,
                    Y = 40
                });

            this.UpdateGameplay();
        }

        internal void UpdateGameplay()
        {
            this.SetNewRandomHandMove();

            var spritePath = this.GetCorrespondingSpritePathToCurrentHandMove();

            this.UpdateSpriteComponent(spritePath);
        }

        private void UpdateSpriteComponent(string spritePath)
        {
            this.entity.RemoveComponent<Sprite>();
            var newSpriteComponent = new Sprite(spritePath);
            this.entity.AddComponent(newSpriteComponent);
            this.entity.RefreshDependencies();
        }

        private string GetCorrespondingSpritePathToCurrentHandMove()
        {
            string spriteName;

            switch (this.CurrentHandMove)
            {
                case GameplayHandMovesEnum.Stone:
                    spriteName = "_0011_piedra_grande";

                    break;
                case GameplayHandMovesEnum.Paper:
                    spriteName = "_0012_papel_grande";

                    break;
                case GameplayHandMovesEnum.Scissors:
                    spriteName = "_0010_tijeras_grande";

                    break;
                default:
                    throw new InvalidOperationException();
            }

            var spritePath = string.Format("Content/{0}.wpk", spriteName);
            
            return spritePath;
        }

        private void SetNewRandomHandMove()
        {
            var everyHandMoveArray = Enum.GetValues(typeof(GameplayHandMovesEnum)) as GameplayHandMovesEnum[];

            // MAYBE: The 1st is always the same :-/
            var randomHandMoveIndex = WaveServices.Random.Next(everyHandMoveArray.Length);
            var randomHandMove = everyHandMoveArray[randomHandMoveIndex];

            this.CurrentHandMove = randomHandMove;
        }
    }
}
