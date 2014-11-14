using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Services;
using WaveOculusDemoProject.Entities;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// This class controls each asteriod field sector
    /// </summary>
    public class AsteroidFieldController : Behavior
    {
        private int Border = 2;
        private const int DefaultMaxLod = 2;

        private static int asteroidCounter = 0;

        private Vector3 sectorSize;
        private bool needRefresh;
        private int sectorX, sectorY;
        private int maxLodLevel;

        // Asteroid sector pool lists
        private List<AsteroidSectorDecorator> asteroidSectorList;
        private List<AsteroidSectorDecorator>[,] busyAsteroidSectorList;
        private List<AsteroidSectorDecorator>[,] freeAsteroidSectorList;

        public int MaxLodLevel
        {
            get
            {
                return this.maxLodLevel;
            }

            set
            {
                this.maxLodLevel = Math.Min(Math.Max(0, value), DefaultMaxLod);
            }
        }

        /// <summary>
        /// Instantiates a new Asteroid field controller, with the size of one asteroid sector.
        /// </summary>
        /// <param name="sectorSize">The size of one asteroid sector</param>
        public AsteroidFieldController(Vector3 sectorSize)
        {
            this.sectorSize = sectorSize;
            this.needRefresh = true;

            Platform platform = WaveServices.Platform;
            
            
            if (platform.PlatformType == PlatformType.Windows 
                || platform.PlatformType == PlatformType.Linux 
                || platform.PlatformType == PlatformType.MacOS)
            {
                // In case of desktop platforms (Windows, Mac & Linux), disable LOD and increase the number of asteroids 
                this.MaxLodLevel = 0;
                this.Border = 5;
            }
            else
            {
                // In Mobile platforms, enable LOD and decrease the number of asteroid sectors.
                this.MaxLodLevel = 2;
                this.Border = 2;
            }

            this.UpdateOrder = 0;
        }

        /// <summary>
        /// Initializes the asteroid field controller
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            this.asteroidSectorList = new List<AsteroidSectorDecorator>();
            this.busyAsteroidSectorList = new List<AsteroidSectorDecorator>[3, 3];
            this.freeAsteroidSectorList = new List<AsteroidSectorDecorator>[3, 3];

            for (int i = 0; i < this.busyAsteroidSectorList.GetLength(0); i++)
            {
                for (int j = 0; j < this.busyAsteroidSectorList.GetLength(1); j++)
                {
                    this.busyAsteroidSectorList[i, j] = new List<AsteroidSectorDecorator>();
                    this.freeAsteroidSectorList[i, j] = new List<AsteroidSectorDecorator>();
                }
            }
        }

        /// <summary>
        /// Update the asteroid field. If the player camera moves to a different sector, this controller
        /// must place each asteroids sectors in its correct place.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {
            Vector3 position = this.RenderManager.ActiveCamera3D.Position;

            int newSectorX, newSectorY;

            this.GetSector(position, out newSectorX, out newSectorY);

            if (this.sectorX != newSectorX || this.sectorY != newSectorY)
            {
                this.sectorX = newSectorX;
                this.sectorY = newSectorY;
                this.needRefresh = true;
            }

            if (this.needRefresh)
            {
                this.Refresh();
                this.needRefresh = false;
            }
        }

        /// <summary>
        /// Refresh the asteroid field.
        /// </summary>
        private void Refresh()
        {
            this.CleanSectorList();

            for (int i = -Border; i <= Border; i++)
            {
                for (int j = -Border; j <= Border; j++)
                {
                    int asteroidSectorX = this.sectorX + i;
                    int asteroidSectorY = this.sectorY + j;
                    int sectorType = this.GetSectorTypeByCoords(asteroidSectorX, asteroidSectorY);
                    Vector3 position = this.GetPositionByCoordinates(asteroidSectorX, asteroidSectorY);

                    // Fix a wrong asteroid field position in 3DS MAX
                    if(position.X == -2000)
                    {
                        position.X = -1933.599f;
                    }

                    AsteroidSectorDecorator asteroidSector = this.GetFreeSteroidSector(sectorType, Math.Max(Math.Abs(i), Math.Abs(j)));

                    asteroidSector.Transform.Position = position;
                }
            }
        }

        /// <summary>
        /// Get a free asteroid sector.
        /// </summary>
        /// <param name="sectorType">The sector type</param>
        /// <param name="lod">The sector LOD (level of detail)</param>
        /// <returns>A free asteroid sector</returns>
        private AsteroidSectorDecorator GetFreeSteroidSector(int sectorType, int lod)
        {
            lod = Math.Min(lod, this.MaxLodLevel);

            var freeList = this.freeAsteroidSectorList[sectorType, lod];
            var busyList = this.busyAsteroidSectorList[sectorType, lod];
            AsteroidSectorDecorator result;

            if (freeList.Count == 0)
            {
                result = new AsteroidSectorDecorator("asteroidCounter_" + (asteroidCounter++), sectorType, lod);

                this.Owner.AddChild(result.Entity);
                this.asteroidSectorList.Add(result);
            }
            else
            {
                result = freeList[0];
                freeList.RemoveAt(0);
            }

            busyList.Add(result);
            result.Entity.Enabled = true;

            return result;
        }

        /// <summary>
        /// Clean the asteroid sector list.
        /// </summary>
        private void CleanSectorList()
        {
            for (int i = 0; i < this.busyAsteroidSectorList.GetLength(0); i++)
            {
                for (int j = 0; j < this.busyAsteroidSectorList.GetLength(1); j++)
                {
                    var busyList = this.busyAsteroidSectorList[i, j];
                    var freeList = this.freeAsteroidSectorList[i, j];

                    foreach (var asteroidSector in busyList)
                    {
                        asteroidSector.Entity.Enabled = false;
                    }

                    freeList.AddRange(busyList);
                    busyList.Clear();
                }
            }
        }

        /// <summary>
        /// Gets sector type by its coordinates
        /// </summary>
        /// <param name="sectorX">X coord.</param>
        /// <param name="sectorY">Y coord.</param>
        /// <returns>Sector type</returns>
        public int GetSectorTypeByCoords(int sectorX, int sectorY)
        {
            int lengt = this.busyAsteroidSectorList.GetLength(0);
            return ((sectorX - sectorY) % lengt + lengt) % lengt;
        }

        /// <summary>
        /// Gets sector position by its coordinates
        /// </summary>
        /// <param name="sectorX">X coord.</param>
        /// <param name="sectorY">Y coord.</param>
        /// <returns>World position of the asteroid sector</returns>
        public Vector3 GetPositionByCoordinates(int sectorX, int sectorY)
        {
            return new Vector3(
                (sectorX) * this.sectorSize.X,
                0,
                (sectorY) * this.sectorSize.Y
                );
        }

        /// <summary>
        /// Gets asteroid coordinates by a world position
        /// </summary>
        /// <param name="position">Position in world space</param>
        /// <param name="sectorX">X coord.</param>
        /// <param name="sectorY">Y coord.</param>
        public void GetSector(Vector3 position, out int sectorX, out int sectorY)
        {
            Vector2 positionSectorSpace = new Vector2(
                position.X / sectorSize.X + 0.5f,
                position.Z / sectorSize.Y + 0.5f
                );

            sectorX = positionSectorSpace.X >= 0 ? (int)positionSectorSpace.X : (int)positionSectorSpace.X - 1;
            sectorY = positionSectorSpace.Y >= 0 ? (int)positionSectorSpace.Y : (int)positionSectorSpace.Y - 1;
        }
    }
}
