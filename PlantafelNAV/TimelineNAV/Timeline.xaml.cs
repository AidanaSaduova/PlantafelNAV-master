using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlantafelNAV.TimelineNAV
{
    /// <summary>
    /// Interaction logic for Timeline.xaml
    /// </summary>
    public partial class Timeline : UserControl
    {
        List<TimelineMark> TMarks = new List<TimelineMark>();
        List<TimelineElement> TElements = new List<TimelineElement>();
        int width;
        int height;
        int innerHeight;
        int elementTop;
        int spacing;
        int startSeconds;
        int endSeconds;
        internal int pixelDistance;
        public int identify;

        public List<TimelineElement> TElements1 { get => TElements; set => TElements = value; }




        /// <summary>
        /// Creates a new Timeline control of the specified height and width
        /// </summary>
        /// <param name="w">Set the height</param>
        /// <param name="h">Set the width</param>
        public Timeline(int w, int h, int x)
        {
            InitializeComponent();

            width = w;
            height = h;
            border.Width = w;
            border.Height = h;
            identify = x;
        }

        /// <summary>
        /// Creates a new TimelineElement at the desired location (in seconds)
        /// </summary>
        /// <param name="seconds">Position of the element in seconds</param>
        public void AddElement(int start_seconds, int duration, string id, int ap)
        {

            TimelineElement te = new TimelineElement(this, innerHeight, start_seconds, duration, id, ap);
            TElements1.Add(te);
            mainCanvas.Children.Add(te);
            
            Canvas.SetTop(te, elementTop);
            Canvas.SetLeft(te, (pixelDistance * (start_seconds - startSeconds) / (endSeconds - startSeconds)) - 2);
            te.savePositions();
        }

        public void clearCanvas()
        {
            
            foreach (TimelineElement x in TElements1)
            {
                mainCanvas.Children.Remove(x);
            }
        }

        /// <summary>
        /// Called by a TimelineElement once it has finished being dragged to a new location
        /// </summary>
        /// <param name="te">The TimelineElement in question</param>
        public void RefreshElement(TimelineElement te)
        {
            te.SetSeconds((int)((Canvas.GetLeft(te) + 2) * (endSeconds - startSeconds) / pixelDistance) + startSeconds);

            int seconds = (int)((Canvas.GetLeft(te) + 2) * (endSeconds - startSeconds) / pixelDistance) + startSeconds;
            //Text für die Infobox aktualisieren
            string begin = getTimeFromSeconds(seconds);
            //string end = getTimeFromSeconds(Duration + seconds);
            string end = getTimeFromSeconds(te.Duration + seconds);
            string fin = "Begin: " + begin + " Ende: " + end;
            changeTextInInfobox(fin, te.Id);

        }

        /// <summary>
        /// Sets up the timeline using seconds
        /// </summary>
        /// <param name="startSeconds">How many seconds should be assigned to the left-most side</param>
        /// <param name="endSeconds">How many seconds should be assigned to the right-most side</param>
        /// <param name="intervalSeconds">How many seconds should there be between the visibly marked intervals</param>
        /// <param name="spacing">Distance between intervals in pixels</param>
        public void Setup(int startSeconds, int endSeconds, int intervalSeconds, int spacing)
        {
            this.startSeconds = startSeconds;
            this.endSeconds = endSeconds;
            this.spacing = spacing;

            // Create first mark
            TimelineMark tmStart = new TimelineMark(startSeconds);
            TMarks.Add(tmStart);
            mainCanvas.Children.Add(tmStart);

            // Create middle marks
            int intervalCount = ((endSeconds - startSeconds) / intervalSeconds) - 1;
            for (int i = 1; i <= intervalCount; i++)
            {
                TimelineMark tm = new TimelineMark(startSeconds + (intervalSeconds * i));
                TMarks.Add(tm);
                mainCanvas.Children.Add(tm);
            }

            // Create last mark
            TimelineMark tmEnd = new TimelineMark(endSeconds);
            TMarks.Add(tmEnd);
            mainCanvas.Children.Add(tmEnd);

            // Setup spacing
            for (int k = 0; k < TMarks.Count; k++)
            {
                Canvas.SetLeft(TMarks[k], spacing * k);
                Canvas.SetTop(TMarks[k], 1);
            }

            // Size & place the controls
            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Arrange(new Rect(0, 0, width, height));

            Border border = new Border();
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(12, 12, 12));
            border.Background = new SolidColorBrush(Color.FromRgb(248, 248, 248));
            mainCanvas.Children.Add(border);
            Canvas.SetTop(border, 1 + tmStart.ActualHeight);
            elementTop = 1 + (int)tmStart.ActualHeight + 1; // Canvas.Top value for TimelineElements
            border.Width = 1 + Canvas.GetLeft(tmEnd);
            border.Height = height - 46; // To account for TimelineMark height & scrollbar height. This value assumes the height of the Aero-style scrollbar.
            innerHeight = height - 46 - 2; // Height of region inside the border

            //Infobox einrichten
            Infobox.Width = width - 38;
            changeTextInInfobox("Beginn: - Ende: -", "#");
            

            pixelDistance = (int)border.Width - 1; // Region of the border aka the timeline's length in pixels

            mainCanvas.Width = (spacing * (TMarks.Count - 1)) + (int)tmEnd.ActualWidth; // Set the canvas's width so the ScrollViewer knows how big it is
        }

        public void changeTextInInfobox(string text, string id)
        {
            if (id == "#") { Infobox.Text = text; }
            else { Infobox.Text = text + " (" + id + ")";
                }

        }

        public string getTimeFromSeconds(int seconds)
        {
            TimeSpan timespan = TimeSpan.FromSeconds(seconds);
            int hour = timespan.Hours;
            int min = timespan.Minutes;
            int sec = timespan.Seconds;
            string hours; string minutes;
            if (hour < 10) { hours = "0" + hour.ToString(); } else { hours = hour.ToString(); }
            if (min < 10) { minutes = "0" + min.ToString(); } else { minutes = min.ToString(); }
            string x = hours + ":" + minutes;
            return x;
        }

 

 
    }
}
