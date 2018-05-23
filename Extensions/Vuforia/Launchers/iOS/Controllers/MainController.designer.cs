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

namespace Vuforia
{
	[Register ("MainController")]
	partial class MainController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		GLView glView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (glView != null) {
				glView.Dispose ();
				glView = null;
			}
		}
	}
}
