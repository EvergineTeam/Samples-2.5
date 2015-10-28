#region Using Statements
using Leap;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.LeapMotion;
using WaveEngine.LeapMotion.Behaviors;
using WaveEngine.Materials;
#endregion

namespace LeapMotionSample
{
    /// <summary>
    /// Leap motion drawable
    /// </summary>
    [DataContract]
    public class LeapMotionDrawable : Drawable3D
    {
        private LeapMotionService leapService;

        private Dictionary<string, Entity> entities;

        /// <summary>
        /// Hand types (Right or Left)
        /// </summary>
        private enum HandType
        {
            R,
            L,
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.leapService = WaveServices.GetService<LeapMotionService>();

            if (this.leapService == null)
            {
                Console.WriteLine("You need to register the LeapMotion service.");
                return;
            }

            if (!this.leapService.IsReady)
            {
                Console.WriteLine("You need to call to start method before.");
                return;
            }

            Color rightColor = Color.LightGreen;
            Color leftColor = Color.LightBlue;
            float defaultScale = 0.018f;
            float palmScale = 0.025f;

            this.entities = new Dictionary<string, Entity>();

            // Hands
            foreach (HandType handType in Enum.GetValues(typeof(HandType)))
            {
                string handName = handType.ToString();
                Color color = (handType == HandType.R) ? rightColor : leftColor;

                // Palm
                this.entities.Add(string.Format("{0}Palm", handName), this.CreateNode(handName, color, palmScale));
                this.entities.Add(string.Format("{0}Palm1", handName), this.CreateNode(handName, color, defaultScale));
                string edgePalm1 = string.Format("{0}{1}{2}_To_{0}Palm1", handName, Finger.FingerType.TYPE_THUMB, Finger.FingerJoint.JOINT_MCP);
                this.entities.Add(edgePalm1, this.CreateEdge(handName));
                string edgePalm2 = string.Format("{0}Palm1_To_{0}{1}{2}", handName, Finger.FingerType.TYPE_PINKY, Finger.FingerJoint.JOINT_MCP);
                this.entities.Add(edgePalm2, this.CreateEdge(handName));

                // Fingers
                string beforeMCPFinger = string.Empty;

                foreach (Finger.FingerType fingerType in Enum.GetValues(typeof(Finger.FingerType)))
                {
                    string beforeNodeName = string.Empty;

                    foreach (Finger.FingerJoint jointType in Enum.GetValues(typeof(Finger.FingerJoint)))
                    {
                        string nodeName = string.Format("{0}_{1}_{2}", handName, fingerType, jointType).ToLowerInvariant();
                        this.entities.Add(nodeName, this.CreateNode(handName, color, defaultScale));

                        if (!string.IsNullOrEmpty(beforeNodeName))
                        {
                            string edgeName = string.Format("{0}_To_{1}", beforeNodeName, nodeName).ToLowerInvariant();
                            this.entities.Add(edgeName, this.CreateEdge(handName));
                        }

                        if (jointType == Finger.FingerJoint.JOINT_MCP && !string.IsNullOrEmpty(beforeMCPFinger))
                        {
                            string edgeName = string.Format("{0}_To_{1}", beforeMCPFinger, nodeName).ToLowerInvariant();
                            this.entities.Add(edgeName, this.CreateEdge(handName));
                        }

                        beforeNodeName = nodeName;

                        if (jointType == Finger.FingerJoint.JOINT_MCP)
                        {
                            beforeMCPFinger = nodeName;
                        }
                    }
                }
            }

            foreach (Entity e in this.entities.Values)
            {
                this.Owner.AddChild(e);
            }
        }

        /// <summary>
        /// Create sphere
        /// </summary>
        /// <param name="color">Color.</param>
        /// <returns>Entity.</returns>
        private Entity CreateNode(string handName, Color color, float scale)
        {
            return new Entity() { IsVisible = true , Tag = handName}
                            .AddComponent(new Transform3D()
                            {
                                Scale = new Vector3(scale),
                            })                            
                            .AddComponent(new MaterialsMap(new StandardMaterial(color, DefaultLayers.Opaque) { AmbientColor = Color.Gray }))
                            .AddComponent(Model.CreateSphere())
                            .AddComponent(new ModelRenderer());
        }

        /// <summary>
        /// Create cylinder
        /// </summary>
        /// <returns></returns>
        private Entity CreateEdge(string handName)
        {
            return new Entity() { IsVisible = true, Tag = handName }
                            .AddComponent(new Transform3D()
                            {
                                Scale = new Vector3(0.01f)
                            })                            
                            .AddComponent(new MaterialsMap(new StandardMaterial(Color.LightGray, DefaultLayers.Opaque) { AmbientColor = Color.Gray }))
                            .AddComponent(Model.CreateCube())
                            .AddComponent(new ModelRenderer());
        }

        /// <summary>
        /// Draw the left and right hands capture with leap motion
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(TimeSpan gameTime)
        {
            if (this.leapService != null && this.leapService.IsReady)
            {
                if (this.leapService.CurrentFeatures.HasFlag(LeapFeatures.Hands))
                {
                    // Hands
                    foreach (HandType handType in Enum.GetValues(typeof(HandType)))
                    {
                        string handName = handType.ToString();
                        Hand hand = (handType == HandType.R) ? this.leapService.RightHand : this.leapService.LeftHand;

                        if (hand != null)
                        {
                            this.UpdateVisibility(handName, hand.IsValid);

                            if (hand.IsValid)
                            {
                                // Palm
                                UpdatePalm(handName, hand);

                                // Fingers
                                UpdateFingers(hand);
                            }                            
                        }                        
                        else
                        {
                            this.UpdateVisibility(handName, false);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Update palm
        /// </summary>
        /// <param name="handName"></param>
        /// <param name="hand"></param>
        private void UpdatePalm(string handName, Hand hand)
        {
            this.UpdateNode(string.Format("{0}Palm", handName), hand.PalmPosition.ToPositionVector3());

            Vector3 thumbPosition = hand.Fingers[0].JointPosition(Finger.FingerJoint.JOINT_MCP).ToPositionVector3();
            Vector3 indexPosition = hand.Fingers[1].JointPosition(Finger.FingerJoint.JOINT_MCP).ToPositionVector3();
            Vector3 pinkyPosition = hand.Fingers[4].JointPosition(Finger.FingerJoint.JOINT_MCP).ToPositionVector3();
            Vector3 direction = (pinkyPosition - indexPosition);
            direction.Normalize();
            Vector3 palm1Position = thumbPosition + direction * 0.05f;
            this.UpdateNode(string.Format("{0}Palm1", handName), palm1Position);
            string edgePalm1 = string.Format("{0}{1}{2}_To_{0}Palm1", handName, Finger.FingerType.TYPE_THUMB, Finger.FingerJoint.JOINT_MCP);
            this.UpdateEdge(edgePalm1, thumbPosition, palm1Position);
            string edgePalm2 = string.Format("{0}Palm1_To_{0}{1}{2}", handName, Finger.FingerType.TYPE_PINKY, Finger.FingerJoint.JOINT_MCP);
            this.UpdateEdge(edgePalm2, palm1Position, pinkyPosition);
        }

        /// <summary>
        /// Updates the fingers.
        /// </summary>
        /// <param name="hand">The hand.</param>
        private void UpdateFingers(Hand hand)
        {
            string beforeMCPFinger = string.Empty;
            Vector3 beforeMCPPosition = Vector3.Zero;

            for (int i = 0; i < 5; i++)
            {
                Finger finger = hand.Fingers[i];

                string handName = (finger.Hand.IsRight) ? HandType.R.ToString() : HandType.L.ToString();
                string fingerName = finger.Type.ToString();

                string beforeNodeName = string.Empty;
                Vector3 beforeJointPosition = Vector3.Zero;

                foreach (Finger.FingerJoint fingerJoint in Enum.GetValues(typeof(Finger.FingerJoint)))
                {
                    string nodeName = string.Format("{0}_{1}_{2}", handName, fingerName, fingerJoint).ToLowerInvariant();
                    Vector3 jointPosition = finger.JointPosition(fingerJoint).ToPositionVector3();
                    this.UpdateNode(nodeName, jointPosition);

                    if (!string.IsNullOrEmpty(beforeNodeName))
                    {
                        string edgeName = string.Format("{0}_To_{1}", beforeNodeName, nodeName).ToLowerInvariant();
                        this.UpdateEdge(edgeName, beforeJointPosition, jointPosition);
                    }

                    if (fingerJoint == Finger.FingerJoint.JOINT_MCP && !string.IsNullOrEmpty(beforeMCPFinger))
                    {
                        string edgeName = string.Format("{0}_To_{1}", beforeMCPFinger, nodeName).ToLowerInvariant();
                        this.UpdateEdge(edgeName, beforeMCPPosition, jointPosition);
                    }

                    beforeNodeName = nodeName;
                    beforeJointPosition = jointPosition;

                    if (fingerJoint == Finger.FingerJoint.JOINT_MCP)
                    {
                        beforeMCPFinger = nodeName;
                        beforeMCPPosition = jointPosition;
                    }
                }
            }
        }


        /// <summary>
        /// Update node method
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="Vector3"></param>
        private void UpdateNode(string nodeName, Vector3 leapMotionPosition)
        {
            Entity node = this.entities[nodeName];
            var transform = node.FindComponent<Transform3D>();
            transform.Position = leapMotionPosition;            
        }

        /// <summary>
        /// Update edge method
        /// </summary>
        /// <param name="edgeName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void UpdateEdge(string edgeName, Vector3 start, Vector3 end)
        {
            Entity edge = this.entities[edgeName];
            var transform = edge.FindComponent<Transform3D>();            

            Vector3 direction = end - start;
            Vector3 position = start + (direction / 2);
            float distance = direction.Length();

            transform.Position = position;

            Vector3 scale = transform.Scale;
            scale.Z = distance;
            transform.Scale = scale;

            transform.LookAt(end, Vector3.Up);
        }

        /// <summary>
        /// Update entities visibility
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        private void UpdateVisibility(string tag, bool value)
        {
            var entities = this.Owner.FindChildrenByTag(tag);
            foreach(Entity e in entities)
            {
                e.IsVisible = value;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
        }
    }
}
