// Copyright (C) 2012-2013 Weekend Game Studio
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

#region File Description
//-----------------------------------------------------------------------------
// WaterAnimationBehavir
//
// Copyright © 2012 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Framework;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Materials;
using WaveEngine.Common.Math;
#endregion

namespace DualTextureProject
{
    public class WaterAnimationBehavior : Behavior
    {
        [RequiredComponent]
        public MaterialsMap Maps;

        #region Properties
        #endregion

        #region Methods
        public WaterAnimationBehavior()
            : base("WaterAnimationBehavior")
        {
        }

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();
        }

        protected override void Update(TimeSpan gameTime)
        {
            DualTextureMaterial waterMaterial = Maps.Materials["RiverMesh"] as DualTextureMaterial;

            if (waterMaterial != null)
            {
                Vector2 newWaterCoords = waterMaterial.PrimaryTexcoordOffset;
                Vector2 newLeafCoords = waterMaterial.SecondaryTexcoordOffset;

                newWaterCoords.Y = newWaterCoords.Y - (float)gameTime.TotalSeconds * 0.1f;
                newLeafCoords.Y = newLeafCoords.Y - (float)gameTime.TotalSeconds * 0.08f;

                waterMaterial.PrimaryTexcoordOffset = newWaterCoords;
                waterMaterial.SecondaryTexcoordOffset = newLeafCoords;
            }
        }
        #endregion
    }
}
