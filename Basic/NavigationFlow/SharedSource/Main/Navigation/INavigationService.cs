#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NavigationFlow.Navigation
{
    public interface INavigationService<TCommands> where TCommands : struct
    {
        bool IsPerformingNavigation { get; }

        void StartNavigation();
        void Navigate(TCommands command);
        bool CanNavigate(TCommands command);
    }

}
