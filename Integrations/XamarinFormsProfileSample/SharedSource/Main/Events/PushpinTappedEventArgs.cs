using System;

namespace XamarinFormsProfileSample.Events
{
    public class PushpinTappedEventArgs : EventArgs
    {
        public PushpinTappedEventArgs(string pushpin)
        {
            Pushpin = pushpin;
        }

        public string Pushpin { get; }
    }
}