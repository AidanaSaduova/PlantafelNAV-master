using System;
using System.Windows.Controls;

namespace PlantafelNAV.TimelineNAV
{
    /// <summary>
    /// Interaction logic for TimelineMark.xaml
    /// </summary>
    public partial class TimelineMark : UserControl
    {
        public TimelineMark(int seconds)
        {
            InitializeComponent();
            //Da wir nur mit Minuten arbeiten, wurde hier die Methode entsprechend angepasst
            /*
            string m = (seconds / 60).ToString();
            if (m.Length < 2)
                m = "0" + m;
            string s = (seconds % 60).ToString();
            if(s.Length < 2)
                s = "0" + s;
            this.text.Text = m + ":" + s;*/

            TimeSpan timespan = TimeSpan.FromSeconds(seconds);
            int hour = timespan.Hours;
            int min = timespan.Minutes;
            int sec = timespan.Seconds;
            string hours; string minutes;
            if (hour < 10) { hours = "0" + hour.ToString(); } else { hours = hour.ToString(); }
            if (min < 10) { minutes = "0" + min.ToString(); } else { minutes = min.ToString(); }
            this.text.Text = hours + ":" + minutes;
        }
    }
}
