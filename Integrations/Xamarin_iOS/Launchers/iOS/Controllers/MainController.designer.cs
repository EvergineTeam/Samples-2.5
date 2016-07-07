// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using WaveEngine.Adapter;

namespace Xamarin_iOS
{
    [Register ("MainController")]
    partial class MainController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch AutoRotateSwitch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        WaveEngine.Adapter.GLView glView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AutoRotateSwitch != null) {
                AutoRotateSwitch.Dispose ();
                AutoRotateSwitch = null;
            }

            if (glView != null) {
                glView.Dispose ();
                glView = null;
            }
        }
    }
}