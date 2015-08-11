#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Common.Media;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace Music
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/MyScene.wscene");

            MusicInfo musicInfo = new MusicInfo(@"Content/Assets/ByeByeBrain.mp3");
            WaveServices.MusicPlayer.Play(musicInfo);
        }
    }
}
