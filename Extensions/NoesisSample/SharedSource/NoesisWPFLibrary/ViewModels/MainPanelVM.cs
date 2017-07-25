using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace NoesisWPFLibrary
{
    public class MainPanelVM
    {
        public ICommand MGOCommand { get; private set; }

        public ICommand CuriosityCommand { get; private set; }

        public ICommand SpiritCommand { get; private set; }

        public MainPanelVM()
        {
            this.MGOCommand = new MissionCommand(this.ToMGO);
            this.CuriosityCommand = new MissionCommand(this.ToCuriosity);
            this.SpiritCommand = new MissionCommand(this.ToSpirit);
        }

        private void ToMGO(object obj)
        {
            StateManager.Instance.BeginState(MissionEnum.MarsGlobarSurveyor);
        }

        private void ToCuriosity(object obj)
        {
            StateManager.Instance.BeginState(MissionEnum.Curiosity);
        }

        private void ToSpirit(object obj)
        {
            StateManager.Instance.BeginState(MissionEnum.Spirit);
        }
    }
}
