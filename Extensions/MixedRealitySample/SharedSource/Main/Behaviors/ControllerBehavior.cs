#region Using Statements
using MixedRealitySample.Components;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Common.Physics3D;
using WaveEngine.Common.VR;
using WaveEngine.Components.VR;
using WaveEngine.Framework;
using WaveEngine.Framework.Physics3D;
#endregion

namespace MixedRealitySample.Behaviors
{
    [DataContract]
    public class ControllerBehavior : BaseControllerBehavior
    {
        // Cached
        private VRCameraRig vrCameraRig;
        private ButtonComponentBase lastButton;
        //private NoesisMouseSyncComponent lastNoesisPanel;
        private List<HitResult3D> collisions;
        private HitResult3D resultZero;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.collisions = new List<HitResult3D>();
            this.resultZero = new HitResult3D();

        }

        /// <summary>
        /// Resolve dependencies method
        /// </summary>
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            this.vrCameraRig = this.EntityManager.FindComponentFromEntityPath<VRCameraRig>("CameraRig");
        }

        protected override void UpdateState()
        {
            VRGenericControllerState currentstate;
            if (this.Type == ControllerType.Left)
            {
                currentstate = this.vrCameraRig.LeftController.State;
            }
            else
            {
                currentstate = this.vrCameraRig.RightController.State;
            }

            this.ThumbstickValue = currentstate.ThumbStick;
            this.TriggerValue = currentstate.Trigger;
            this.GrabValue = (currentstate.Grip == WaveEngine.Common.Input.ButtonState.Pressed) ? true : false;
            this.MenuValue = (currentstate.Menu == WaveEngine.Common.Input.ButtonState.Pressed) ? true : false;
            this.TouchpadValue = currentstate.Touchpad;
        }

        protected override void CalculateGazeProperties(ref Ray ray, out Vector3 gazePosition, out Vector3 gazeNormal)
        {
            gazePosition = ray.Position + ray.Direction * 50;
            gazeNormal = -ray.Direction;

            if (this.Owner.Scene.PhysicsManager.Simulation3D.InternalWorld == null)
            {
                return;
            }

            this.collisions.Clear();
            this.Owner.Scene.PhysicsManager.Simulation3D.RayCastAll(ref ray, this.collisions);
            if (this.collisions != null && this.collisions.Count > 0)
            {
                HitResult3D result = this.NearestCollider(ref ray.Position, this.collisions);
                if (result.Succeeded)
                {
                    if (!this.gaze.IsVisible)
                    {
                        this.gaze.IsVisible = true;
                    }

                    gazePosition = result.Point + (result.Normal * 0.002f);
                    gazeNormal = result.Normal;
                }

                // Noesis Panel 
                //this.CheckNoesisInteraction(ref result, ref ray);

                // 3D Buttons
                this.CheckButtonInteraction(ref result);
            }
            else
            {
                if (gaze.IsVisible)
                {
                    this.gaze.IsVisible = false;
                }
            }
        }

        private void CheckButtonInteraction(ref HitResult3D result)
        {
            ButtonComponentBase currentButton = null;
            if (result.Succeeded)
            {
                Entity entityHiting = (result.PhysicBody.UserData as PhysicBody3D).Owner;
                currentButton = entityHiting?.Parent?.FindComponent<ButtonComponentBase>(false);
                if (currentButton != null)
                {
                    if (!currentButton.Hover)
                    {
                        currentButton.Hover = true;
                    }
                    if (this.TriggerValue > 0 && this.lastTriggerValue == 0)
                    {
                        currentButton?.Touch();
                    }

                    if (currentButton != this.lastButton && this.lastButton != null)
                    {
                        this.lastButton.Hover = false;
                    }
                }
                else
                {
                    if (this.lastButton != null)
                    {
                        this.lastButton.Hover = false;
                    }
                }
            }
            else
            {
                if (this.lastButton != null)
                {
                    this.lastButton.Hover = false;
                }
            }

            this.lastButton = currentButton;
        }

        //private void CheckNoesisInteraction(ref HitResult3D result, ref Ray ray)
        //{
        //    NoesisMouseSyncComponent noesisSyncComponent = null;
        //    if (result.Succeeded)
        //    {
        //        Entity noesisEntity = (result.PhysicBody.UserData as PhysicBody3D).Owner;
        //        var noesisPanel = noesisEntity?.FindComponent<NoesisPanel>();
        //        noesisSyncComponent = noesisEntity?.FindComponent<NoesisMouseSyncComponent>();

        //        if (noesisPanel != null && noesisSyncComponent != null)
        //        {
        //            int projectedX;
        //            int projectedY;
        //            float? rayDistance;
        //            if (noesisPanel != null && noesisPanel.View != null && noesisPanel.ProjectRay(ray, out projectedX, out projectedY, out rayDistance))
        //            {
        //                if (this.lastTriggerValue == 0 && this.TriggerValue > 0)
        //                {
        //                    noesisSyncComponent.MouseDown(projectedX, projectedY, Noesis.MouseButton.Left);
        //                }
        //                else if (this.lastTriggerValue > 0 && this.TriggerValue == 0)
        //                {
        //                    noesisSyncComponent.MouseUp(projectedX, projectedY, Noesis.MouseButton.Left);
        //                }
        //                else
        //                {
        //                    noesisSyncComponent.MouseMove(projectedX, projectedY);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            this.lastNoesisPanel?.MouseMove(-1, -1);
        //            noesisSyncComponent = null;
        //        }
        //    }
        //    else
        //    {
        //        this.lastNoesisPanel?.MouseMove(-1, -1);
        //        noesisSyncComponent = null;
        //    }

        //    this.lastNoesisPanel = noesisSyncComponent;
        //}

        private HitResult3D NearestCollider(ref Vector3 startPosition, IList<HitResult3D> results)
        {
            float minDistance = float.MaxValue;
            HitResult3D nearest = this.resultZero;

            foreach (HitResult3D r in results)
            {
                if (!r.Succeeded)
                {
                    continue;
                }

                if (minDistance > r.HitFraction)
                {
                    nearest = r;
                    minDistance = r.HitFraction;
                }
            }

            return nearest;
        }
    }
}
