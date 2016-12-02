#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Framework.Services;
#endregion

namespace NavigationFlow.Navigation
{
    public interface IAnimatedNavigationScene
    {
        IGameAction CreateAppearGameAction();
        IGameAction CreateDiappearGameAction();
    }

}
