#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.AI.PathFinding;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.TiledMap;
#endregion

namespace PathFindingProject.Behaviors
{
    public class PathFollowerBehavior : Behavior
    {
        /// <summary>
        /// The offset with respect to the tile position
        /// </summary>
        private readonly Vector2 playerOffset = new Vector2(7, 11);

        /// <summary>
        /// The weights represented in a dictionary. Where the key indicates the TileId and the Value the
        /// </summary>
        private readonly Dictionary<int, int> weightsByTileId = new Dictionary<int, int>()
        {
            {1,1},
            //{4,2},
            //{12, 4},
            //{15, 5},
            //{16, 6}
        };

        [RequiredComponent]
        protected Transform2D transform2D;

        [RequiredComponent]
        protected PathFinder<LayerTile> pathFinder;

        [RequiredComponent]
        protected Animation2D animation2D;

        // TODO: NOT NECESSARY
        [RequiredComponent]
        protected AStar<LayerTile> pathFindingAlgorithm;

        private TiledMap tiledMap;
        private LayerTile currentTile;
        private TiledMapLayer groundTiledLayer;

        private LayerTile nextTile;
        private TimeSpan elapsed;
        private List<LayerTile> pathToFollow;

        #region Initialization
        public PathFollowerBehavior(TiledMap tiledMap)
        {
            this.tiledMap = tiledMap;

            this.pathToFollow = new List<LayerTile>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Generate the Adjacency Matrix based on the TiledMapLayer
            this.groundTiledLayer = this.tiledMap.TileLayers["Ground"];

            this.pathFinder.AdjacencyMatrix = this.GetAdjacencyMatrixFromTiledMapLayer(this.groundTiledLayer, this.weightsByTileId);

            // TODO: NOT NECESSARY
            this.pathFindingAlgorithm.AdjacencyMatrix = this.pathFinder.AdjacencyMatrix;

            this.currentTile = this.groundTiledLayer.GetLayerTileByMapCoordinates(2, 3);

            // Move to starting tile
            this.SetTransformPosition(this.currentTile.LocalPosition);
        } 
        #endregion

        #region Private Methods
        protected override void Update(TimeSpan gameTime)
        {
            if (this.nextTile != null)
            {
                this.elapsed += gameTime;
                var amount = Math.Min(1, this.elapsed.TotalSeconds / 0.5f);

                this.SetTransformPosition(Vector2.Lerp(this.currentTile.LocalPosition, this.nextTile.LocalPosition, (float)amount));

                if (amount >= 1)
                {
                    this.currentTile = nextTile;
                    this.nextTile = null;

                    if (this.pathToFollow.Count == 0)
                    {
                        this.UpdateCurrentAnimation();
                    }
                }
            }
            else if (this.pathToFollow.Count > 0)
            {
                this.nextTile = this.pathToFollow.First();
                this.pathToFollow.Remove(this.nextTile);
                this.elapsed = TimeSpan.Zero;

                this.UpdateCurrentAnimation();
            }
        }

        private void SetTransformPosition(Vector2 tilePosition)
        {
            this.transform2D.Position = (tilePosition + this.playerOffset) * 4;
        }

        private void UpdateCurrentAnimation()
        {
            string currentAnimation = string.Empty;

            if (this.nextTile != null)
            {
                var currentPosition = this.currentTile.LocalPosition;
                var nextPosition = this.nextTile.LocalPosition;

                currentAnimation = "Walk_";

                if (nextPosition.Y < currentPosition.Y)
                {
                    // Look Up
                    currentAnimation += "Top";
                }
                else if (nextPosition.Y > currentPosition.Y)
                {
                    // Look Down
                    currentAnimation += "Bottom";
                }

                if (nextPosition.X > currentPosition.X)
                {
                    // Look right
                    currentAnimation += "Right";
                }
                else
                {
                    // Look left
                    currentAnimation += "Left";
                }
            }
            else
            {
                currentAnimation = this.animation2D.CurrentAnimation;
                currentAnimation = currentAnimation.Substring(currentAnimation.IndexOf('_'));

                // Idle
                currentAnimation = "Idle" + currentAnimation;
            }

            this.animation2D.CurrentAnimation = currentAnimation;
            ////this.animation2D.Play();
        }

        private AdjacencyMatrix<LayerTile> GetAdjacencyMatrixFromTiledMapLayer(TiledMapLayer tiledMapLayer, IDictionary<int, int> weights)
        {
            var result = new AdjacencyMatrix<LayerTile>();

            foreach (var tile in tiledMapLayer.Tiles)
            {
                if (weights.ContainsKey(tile.Id))
                {
                    foreach (var neighbour in tiledMapLayer.GetNeighboursFromTile(tile))
                    {
                        if (neighbour != null && weights.ContainsKey(neighbour.Id))
                        {
                            result.AddAdjacent(tile, neighbour, weights[neighbour.Id]);
                        }
                    }
                }
            }

            return result;
        } 
        #endregion

        #region Public Methods
        public void MoveToTile(LayerTile tile)
        {
            this.MoveToTileCoords(tile.X, tile.Y);
        }

        public void MoveToTileCoords(int x, int y)
        {
            var startTile = this.nextTile ?? this.currentTile;
            var endTile = this.groundTiledLayer.GetLayerTileByMapCoordinates(x, y);

            this.pathToFollow = this.pathFinder.GetPath(startTile, endTile);
        } 
        #endregion
    }
}
