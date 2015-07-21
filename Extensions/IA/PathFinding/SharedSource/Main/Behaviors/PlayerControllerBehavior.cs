#region Using Statements
using PathFindingProject.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.TiledMap;
#endregion

namespace PathFindingProject.Behaviors
{
    [DataContract]
    public class PlayerControllerBehavior : Behavior
    {
        private readonly Color overTileColor = Color.Red;

        [RequiredComponent]
        protected TiledMap tiledMap;

        protected Entity mouseOverTileEntity;

        protected TiledMapLayer groundLayer;

        private bool waitingTouchRelease;

        public event EventHandler<LayerTile> TileSelected;

        public PlayerControllerBehavior()
            : base()
        { }

        protected override void Initialize()
        {
            base.Initialize();

            this.groundLayer = this.tiledMap.TileLayers["Ground"];

            this.mouseOverTileEntity = this.CreateMouseOverEntity(this.overTileColor, 17);
            this.Owner.AddChild(this.mouseOverTileEntity);
        }

        protected override void Update(TimeSpan gameTime)
        {
            var mouseState = WaveServices.Input.MouseState;

            var activeCamera = this.RenderManager.ActiveCamera2D;

            var touchPressed = WaveServices.Input.TouchPanelState.Count > 0;

            var mousePosition = Vector2.Zero;
            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;

            if (touchPressed)
            {
                mousePosition = WaveServices.Input.TouchPanelState.First().Position;
            }

            WaveServices.ViewportManager.TranslatePosition(ref mousePosition);

            var worldLocationV3 = mousePosition.ToVector3(0);
            worldLocationV3 = activeCamera.Unproject(ref worldLocationV3);

            var tile = this.groundLayer.GetLayerTileByWorldPosition(worldLocationV3.ToVector2());

            var tileCoordsStr = tile == null ? string.Empty : tile.X + ", " + tile.Y;

            Labels.Add("Dest Coords", tileCoordsStr);

            this.UpdateTile(tile, this.mouseOverTileEntity);

            if (mouseState.LeftButton == ButtonState.Pressed || touchPressed)
            {
                this.waitingTouchRelease = true;
            }
            else if (this.waitingTouchRelease)
            {
                this.waitingTouchRelease = false;

                if (this.TileSelected != null)
                {
                    this.TileSelected(this, tile);
                }
            }
        }

        private Entity CreateMouseOverEntity(Color color, int tileId)
        {
            var sceneTileSet = this.tiledMap.Tilesets.First(ts => ts.Name == "Default");
            var tileRectangle = TiledMapUtils.GetRectangleTileByID(sceneTileSet, tileId);

            return new Entity()
                {
                    IsVisible = false
                }
                .AddComponent(new Transform2D()
                {
                    LocalDrawOrder = -1f,
                    Opacity = 0.5f
                })
                .AddComponent(new Sprite(sceneTileSet.Image.AssetPath)
                {
                    SourceRectangle = tileRectangle,
                    TintColor = color
                })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha, AddressMode.PointWrap));
        }

        private void UpdateTile(LayerTile layerTile, Entity tileEntity)
        {
            tileEntity.IsVisible = layerTile != null;

            if (layerTile != null)
            {
                tileEntity.FindComponent<Transform2D>().LocalPosition = layerTile.LocalPosition;
            }
        }
    }
}
