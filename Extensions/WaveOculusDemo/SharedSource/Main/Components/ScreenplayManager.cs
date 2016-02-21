using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Media;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveOculusDemo;

namespace WaveOculusDemoProject.Components
{
    public enum FighterState
    {
        Player,     // The player fighter
        Wingman,    // The wingman figher
        Enemy       // The enemy fighter
    }

    /// <summary>
    /// Fighter setting. Used to get its transform and state
    /// </summary>
    public class FighterSetting
    {
        public FighterState State;
        public Transform3D Transform;
    }

    /// <summary>
    /// This behavior is responsible to take into account the scene time measured in frame time (specified by the path animations)
    /// </summary>
    [DataContract]
    public class ScreenplayManager : Behavior
    {
        MusicInfo music;

        public const float MusicVolume = 0.6f;

        public double CurrentFrameTime;
        
        [DataMember]
        public float Fps { get; set; }

        public int EndFrame = int.MinValue;
        public int StartFrame = int.MaxValue;

        private Input input;

        public List<FighterSetting> FighterList;

        public bool Loop;

        public bool firstUpdate;

        public List<Tuple<int, Action>> frameActions;

        private int lastFrame = -1;

        public ScreenplayManager()
        {
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.UpdateOrder = 0;
            this.Fps = 30;
            this.firstUpdate = true;
            this.Loop = true;
            this.lastFrame = -1;
            this.CurrentFrameTime = 0;

            this.FighterList = new List<FighterSetting>();
            this.frameActions = new List<Tuple<int, Action>>();
        }

        public void RegisterFighter(FighterSetting fighter)
        {
            this.FighterList.Add(fighter);
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.input = WaveServices.Input;
            this.music = new MusicInfo(WaveContent.Assets.Music.music_mp3);

            this.FrameEvent(0, this.StartMusic);
            
            
        }

        /// <summary> 
        /// Start scene music
        /// </summary>
        private void StartMusic()
        {
            if (this.Owner.Scene is MyScene)
            {
                WaveServices.MusicPlayer.Play(this.music);
                WaveServices.MusicPlayer.Volume = 1f;
            }
        }

        /// <summary>
        /// Update the screenplay frame time
        /// </summary>
        /// <param name="gameTime">The current frame time.</param>
        protected override void Update(TimeSpan gameTime)
        {
            if (this.firstUpdate)
            {

                this.StartMusic();
                this.firstUpdate = false;
                return;
            }

            float fps = this.Fps;

            if (this.input.KeyboardState.IsConnected)
            {
                fps = this.HandleKeys();
            }

            this.CurrentFrameTime += (gameTime.TotalSeconds * fps);

            int frame = (int)this.CurrentFrameTime;

            if (this.lastFrame != frame)
            {
                foreach (var frameAction in this.frameActions)
                {
                    if (frame >= frameAction.Item1 && this.lastFrame < frameAction.Item1)
                    {
                        frameAction.Item2();
                    }
                }

                this.lastFrame = frame;
            }

            if (this.CurrentFrameTime < this.StartFrame)
            {
                this.CurrentFrameTime = this.StartFrame;
            }
            else if (this.CurrentFrameTime > this.EndFrame)
            {
                if (this.Loop)
                {
                    this.CurrentFrameTime = 0;
                }
                else
                {
                    this.CurrentFrameTime = this.EndFrame;
                }
            }
        }

        /// <summary>
        /// Handle keys to change the animation speed
        /// </summary>
        /// <returns>The animation speed, measured in FPS</returns>
        private float HandleKeys()
        {
            float fps = this.Fps;

            float nTouches = this.input.TouchPanelState.Count;

            if (nTouches == 1 || this.input.KeyboardState.I == WaveEngine.Common.Input.ButtonState.Pressed)
            {
                fps *= -1;
            }

            if (nTouches == 2 || this.input.KeyboardState.O == WaveEngine.Common.Input.ButtonState.Pressed)
            {
                fps *= 0.1f;
            }

            if (nTouches == 3 || this.input.KeyboardState.P == WaveEngine.Common.Input.ButtonState.Pressed)
            {
                fps *= 5;
            }

            return fps;
        }

        /// <summary>
        /// Register a frame event
        /// </summary>
        /// <param name="frame">The frame</param>
        /// <param name="action">The action associated to the frame</param>
        public void FrameEvent(int frame, Action action)
        {
            this.frameActions.Add(new Tuple<int, Action>(frame, action));
        }
    }
}
