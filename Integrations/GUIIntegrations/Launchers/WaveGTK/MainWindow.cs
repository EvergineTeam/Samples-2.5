#region Using Statements
using System;
using Gtk;
using WaveGTK; 
#endregion

public partial class MainWindow: Gtk.Window
{
    private WaveWidget waveWidget;    

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow" /> class.
    /// </summary>
    public MainWindow()
        : base(Gtk.WindowType.Toplevel)
    {
        Build();

        VBox panel = new VBox();

        this.waveWidget = new WaveWidget();
        panel.Add(this.waveWidget);

        Button button = new Button("I am a button");
        button.WidthRequest = 100;
        button.HeightRequest = 100;
        panel.Add(button);

        this.Add(panel);        
    }

    /// <summary>
    /// Called when [window state event].
    /// </summary>
    /// <param name="evnt">The evnt.</param>
    /// <returns></returns>
    protected override bool OnWindowStateEvent(Gdk.EventWindowState evnt)
    {
        bool result = base.OnWindowStateEvent(evnt);

        if (evnt.NewWindowState == Gdk.WindowState.Withdrawn || 
            evnt.NewWindowState == Gdk.WindowState.Iconified)
        {
            this.waveWidget.isMinimized = true;
        }
        else
        {
            this.waveWidget.isMinimized = false;
        }

        return result;
    }

    /// <summary>
    /// Called when [delete event].
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="a">The <see cref="DeleteEventArgs" /> instance containing the event data.</param>
    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {        
        this.waveWidget.Destroy();

        Application.Quit();
        a.RetVal = true;
    }      
}
