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
		WaveEngine.Adapter.GLView glView { get; set; }

		[Action ("AutoRotateSwitchChanged:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void AutoRotateSwitchChanged (UISwitch sender);

		void ReleaseDesignerOutlets ()
		{
			if (glView != null) {
				glView.Dispose ();
				glView = null;
			}
		}
	}
}
