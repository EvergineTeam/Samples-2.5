using System.Runtime.Serialization;
using WaveEngine.Framework;
using WaveEngine.NoesisGUI;

namespace NoesisSample.Components
{
    /// <summary>
    /// Camera that adds a Mission view model to a Noesis Panel
    /// </summary>
    [DataContract]
    public class MissionComponent : Component
    {
        [RequiredComponent]
        protected NoesisPanel panel;

        [DataMember]
        private MissionEnum mission;

        private MissionPanel missionPanel;

        private bool isShowed = false;

        public MissionEnum Mission
        {
            get
            {
                return this.mission;
            }

            set
            {
                this.mission = value;
                this.UpdatePanel();
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.panel.OnViewCreated += (e, a) =>
            {
                this.missionPanel = this.panel.Content as MissionPanel;
                this.UpdatePanel();

                this.isShowed = StateManager.Instance.State == this.mission;
                if (!isShowed)
                {
                    this.missionPanel.HidePanel();
                }
            };

            StateManager.Instance.StateChanging -= OnStateChanging;
            StateManager.Instance.StateChanging += OnStateChanging;

            StateManager.Instance.StateChanged -= OnStateChanged;
            StateManager.Instance.StateChanged += OnStateChanged;
        }

        private void UpdatePanel()
        {
            if ((this.panel.Content != null) && (this.panel.Content.IsInitialized))
            {
                this.panel.Content.DataContext = MissionFactory.GetMissionVM(this.mission);
            }
        }

        private void OnStateChanging(object sender, MissionEnum e)
        {
            if ((e != this.mission) && (this.isShowed))
            {
                this.isShowed = false;
                this.missionPanel.HidePanel();
            }
        }

        private void OnStateChanged(object sender, MissionEnum e)
        {
            if (e == this.mission)
            {
                this.isShowed = true;
                this.missionPanel.ShowPanel();
            }
        }
    }
}
