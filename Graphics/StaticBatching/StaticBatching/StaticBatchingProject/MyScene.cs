#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace StaticBatchingProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            WaveServices.ScreenContextManager.SetDiagnosticsActive(true);                        

            //Create the camera
            FreeCamera camera = new FreeCamera("freeCamera", new Vector3(42.60071f, 10.3373f, -45.83459f), new Vector3(41.77419f, 10.37616f, -45.27303f))
            {
                Speed = 10,
                BackgroundColor = Color.CornflowerBlue,
            };

            //Add some light!
            PointLight light = new PointLight("light", Vector3.Zero)
            {
                Position = new Vector3(0, 5, 0),
                Attenuation = 750,
                Color = new Color(1, 0.6f, 0.4f),
                IsVisible = true
            };

            EntityManager.Add(light);
            EntityManager.Add(camera);

            Material columnMaterialA = new NormalMappingMaterial("Content/column1.wpk", "Content/column1_normal_spec.wpk") { AmbientColor = Color.White * 0.3f };
            Material columnMaterialB = new NormalMappingMaterial("Content/column2.wpk", "Content/column2_normal_spec.wpk") { AmbientColor = Color.White * 0.3f };

            bool isStatic = true;
            float columnSpacing = 8;
            int nCols = 10;
            int nColumns = 0;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    Entity column = new Entity("column_" + nColumns++) { IsStatic = isStatic }
                        .AddComponent(new Transform3D()
                        {
                            Position = new Vector3(-j * columnSpacing, 0, (i - 1) * columnSpacing),
                            Rotation = new Vector3(0, (WaveServices.Random.NextInt() % 4) * MathHelper.PiOver4, 0)
                        })
                        .AddComponent(new ModelRenderer())
                        .AddComponent(new Model("Content/mainColumn.wpk"))
                        .AddComponent(new MaterialsMap(columnMaterialB));

                    EntityManager.Add(column);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    Entity column = new Entity("column_" + nColumns++) { IsStatic = isStatic }
                        .AddComponent(new Transform3D()
                        {
                            Position = new Vector3(-j * columnSpacing, 0, (i - 4) * columnSpacing),
                            Rotation = new Vector3(0, (WaveServices.Random.NextInt() % 4) * MathHelper.PiOver2, 0)
                        })
                        .AddComponent(new ModelRenderer())
                        .AddComponent(new Model("Content/column.wpk"))
                        .AddComponent(new MaterialsMap(columnMaterialA));

                    EntityManager.Add(column);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    Entity column = new Entity("column_" + nColumns++) { IsStatic = isStatic }
                        .AddComponent(new Transform3D() { Position = new Vector3(-j * columnSpacing, 0, (i + 1) * columnSpacing) })
                        .AddComponent(new ModelRenderer())
                        .AddComponent(new Model("Content/column.wpk"))
                        .AddComponent(new MaterialsMap(columnMaterialA));

                    EntityManager.Add(column);
                }
            }
        }        
    }
}
