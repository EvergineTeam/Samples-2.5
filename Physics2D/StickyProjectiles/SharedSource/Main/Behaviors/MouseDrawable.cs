#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics; 
#endregion

namespace StickyProjectiles.Behaviors
{
    [DataContract]
    public class MouseDrawable : Drawable2D
    {
        [RequiredComponent]
        private MouseBehavior behavior = null;

        private Layer layer;

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            this.layer = this.RenderManager.FindLayer(DefaultLayers.Debug) as DebugLayer;                 
        }

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

        protected override void Dispose(bool disposing)
        {            
        }
    }
}
