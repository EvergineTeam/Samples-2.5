using System;

using Foundation;
using AppKit;

namespace NoesisSample
{
	public partial class MainWindowController : NSWindowController
	{
		public new MainWindow Window {
			get { return (MainWindow)base.Window; }
		}

		public MainWindowController (IntPtr handle) : base (handle)
		{
		}

		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
		}

		public MainWindowController () : base ("MainWindow")
		{
		}
	}
}
