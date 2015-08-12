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
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace Entity3DCollisionGroups
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/MyScene.wscene");

            var groupA = new Physic3DCollisionGroup();
            var groupB = new Physic3DCollisionGroup();

            // GroupA RED will Ignore Self Group Collision
            groupA.IgnoreCollisionWith(groupA);

            // GroupA RED will Ignore GroupB BLUE Collision
            groupA.IgnoreCollisionWith(groupB);

            // Creates Red-Red enviroment
            this.AddEntityToCollisionGroup("sphereA01", groupA);
            this.AddEntityToCollisionGroup("sphereA02", groupA);
            // Creates Blue-Red enviroment
            this.AddEntityToCollisionGroup("sphereA03", groupA);
            this.AddEntityToCollisionGroup("sphereB01", groupB);
            // Creates Blue-Blue enviroment
            this.AddEntityToCollisionGroup("sphereB02", groupB);
            this.AddEntityToCollisionGroup("sphereB03", groupB);
        }

        private void AddEntityToCollisionGroup(string entityName, Physic3DCollisionGroup group)
        {
            var entity = this.EntityManager.Find(entityName);
            var rigidBody = entity.FindComponent<RigidBody3D>();
            rigidBody.CollisionGroup = group;
        }
    }
}
