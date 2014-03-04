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

using System;
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components;
using WaveEngine.Components.Animation;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
using WaveEngine.Materials;
using WaveEngine.Components.Gestures;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework.Physics2D;

namespace KeyEventsAnimationProject
{
    public class MainScene : Scene
    {
        Animation3D animation;

        protected override void CreateScene()
        {
            #region Scene creation
            // Create the camera
            ViewCamera camera = new ViewCamera("MainCamera", new Vector3(2, 1, 2), new Vector3(0, 1, 0));
            EntityManager.Add(camera.Entity);

            RenderManager.SetActiveCamera(camera.Entity);

            // Create the model. Note of we add the Animation3D component.
            Entity animatedModel = new Entity("Isis")
                .AddComponent(new Transform3D())
                .AddComponent(new BoxCollider())
                .AddComponent(new SkinnedModel("Content/isis.wpk"))
                .AddComponent(new MaterialsMap(new BasicMaterial("Content/isis-difuse.wpk")))
                .AddComponent(new Animation3D("Content/isis-animations.wpk"))
                .AddComponent(new SkinnedModelRenderer())
                .AddComponent(new IsisBehavior());

            // Create the sound bank
            SoundBank spankerSlamSounds = new SoundBank();
            spankerSlamSounds.Add(SoundsManager.FootStep1);
            spankerSlamSounds.Add(SoundsManager.FootStep2);
            WaveServices.SoundPlayer.RegisterSoundBank(spankerSlamSounds);

            RenderManager.BackgroundColor = Color.CornflowerBlue;
            #endregion

            #region Key Events
            // Add the key frames. The first parameter is the name of the animation, the second the number of frames and the third the name of the event. As you can see, we raise two events when
            // the animation is "Attack" ( see the Animation3D example for further information ). The first event is raised on frame 10 and the second on frame 25. See the SpankerBehavior class
            animation = animatedModel.FindComponent<Animation3D>()
                .AddKeyFrameEvent("Jog", 1, "DoFootstep")
                .AddKeyFrameEvent("Jog", 14, "DoFootstep")
                .AddKeyFrameEvent("Jog", 26, "DoFootstep")
                .AddKeyFrameEvent("Jog", 39, "DoFootstep");
            EntityManager.Add(animatedModel);
            #endregion
        }
    }
}
