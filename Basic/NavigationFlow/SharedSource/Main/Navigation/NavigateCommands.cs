#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
#endregion

namespace NavigationFlow.Navigation
{
    public enum NavigateCommands
    {
        Back = 0,
        DefaultForward,
        Quit,
        Play,
        Pause,
        ChooseLevel,
        NextLevel,
        ReturnMainMenu,
    }
}
