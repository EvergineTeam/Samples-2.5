using System;
using System.Collections.Generic;
using System.Text;

namespace NoesisSample
{
    public class StateManager
    {
        private static StateManager instance;

        public static StateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StateManager();
                }

                return instance;
            }
        }

        private bool isInitializing;

        public event EventHandler<MissionEnum> StateChanging;

        public event EventHandler<MissionEnum> StateChanged;

        public MissionEnum State { get; private set; }

        public void BeginState(MissionEnum state)
        {
            if (this.isInitializing || (state == this.State))
            {
                return;
            }

            this.isInitializing = true;
            this.State = state;
            this.StateChanging?.Invoke(this, this.State);
        }

        public void StateInitialized(MissionEnum state)
        {
            this.isInitializing = false;
            this.State = state;
            this.StateChanged?.Invoke(this, this.State);
        }

        private StateManager()
        {
            this.State = MissionEnum.Curiosity;
        }
    }
}
