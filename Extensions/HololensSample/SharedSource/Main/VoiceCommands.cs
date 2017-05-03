using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.VR;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
using WaveEngine.Hololens;
using WaveEngine.Hololens.Interaction;
using WaveEngine.Hololens.Speech;

namespace HololensSample
{
    [DataContract]
    public class VoiceCommands : Component
    {
        private const string EngineTag = "Engine";
        private const string TurnEnginesOn = "Turn Engines On";
        private const string TurnEnginesOff = "Turn Engines Off";
        private const string Spin = "Begin Rotation";
        private const string StopSpin = "End Rotation";

        [RequiredComponent]
        private Spinner spinner;
        
        private KeywordRecognizerService keywordService;
                
        protected override void DefaultValues()
        {
            base.DefaultValues();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.keywordService = WaveServices.GetService<KeywordRecognizerService>();
            
            if (this.keywordService != null)
            {
                this.keywordService.Keywords = new string[] { TurnEnginesOn, TurnEnginesOff, Spin, StopSpin };
                this.keywordService.Start();
                this.keywordService.OnKeywordRecognized += OnKeywordRecognized;
            }
        }

        private void OnKeywordRecognized
            (KeywordRecognizerResult result)
        {
            switch (result.Text)
            {
                case TurnEnginesOff:
                    this.TurnEngines(false);
                    break;
                case TurnEnginesOn:
                    this.TurnEngines(true);
                    break;
                case Spin:
                    this.spinner.IsActive = true;
                    break;
                case StopSpin:
                    this.spinner.IsActive = false;
                    break;
            }
        }
        
        private void TurnEngines(bool state)
        {
            foreach (Entity soundEntity in this.Owner.FindChildrenByTag(EngineTag))
            {
                var emitter = soundEntity.FindComponent<SoundEmitter3D>();
                if (state)
                {
                    emitter.Play();
                    emitter.PlayAutomatically = true;
                }
                else
                {
                    emitter.PlayAutomatically = false;
                    WaveServices.SoundPlayer.StopAllSounds();
                }
            }
        }
    }
}