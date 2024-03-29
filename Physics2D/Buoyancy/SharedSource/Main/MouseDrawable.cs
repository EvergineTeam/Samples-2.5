﻿#region Using Statements
using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
#endregion

namespace Buoyancy
{
    /// <summary>
    /// Mouse drawable class
    /// </summary>
    [DataContract]
    public class MouseDrawable : Drawable2D
    {
        [RequiredComponent]
        private MouseBehavior behavior = null;

        /// <summary>
        /// Resolve dependencies method
        /// </summary>
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();                
        }

        /// <summary>
        /// Draw method
        /// </summary>
        /// <param name="gameTime">game time</param>
        public override void Draw(TimeSpan gameTime)
        {           
            if (behavior != null && this.layer != null)
            {
                if (this.behavior.mouseJoint != null)
                {
                    Vector2 startPosition = this.behavior.TouchPosition;
                    Vector2 endPosition = this.behavior.ConnectedEntity.FindComponent<Transform2D>().Position;
                    
                    layer.LineBatch2D.DrawLine(startPosition, endPosition, Color.Blue, -5);
                }                
            }            
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {            
        }
    }
}
