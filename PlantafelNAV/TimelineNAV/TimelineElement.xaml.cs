using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PlantafelNAV.ViewModel;

namespace PlantafelNAV.TimelineNAV
{
    /// <summary>
    /// Interaction logic for TimelineElement.xaml
    /// </summary>
    public partial class TimelineElement : UserControl
    {
        Timeline parent;
        int seconds;
        string display = "";
        double canvasLeft;
        double mouseXInitial;
        bool primed = false;
        double _elementWidth;
        double _pixProSec;
        string _id;
        int _duration;
        bool _checkIfSelected;
        double _distanceRight;
        double _startposition;
        double _originstartposition;
        double _originendposition;
        double _endposition;



        public double ElementWidth { get => _elementWidth; set => _elementWidth = value; }
        public double PixProSec { get => _pixProSec; set => _pixProSec = value; }
        public string Id { get => _id; set => _id = value; }
        public int Duration { get => _duration; set => _duration = value; }
        public bool CheckIfSelected { get => _checkIfSelected; set => _checkIfSelected = value; }
        public double Startposition { get => _startposition; set => _startposition = value; }
        public double Endposition { get => _endposition; set => _endposition = value; }
        public double Originstartposition { get => _originstartposition; set => _originstartposition = value; }
        public double Originendposition { get => _originendposition; set => _originendposition = value; }
        public double DistanceRight { get => _distanceRight; set => _distanceRight = value; }

        /// <summary>
        /// Creates the visual TimelineElement
        /// </summary>
        /// <param name="height">Height of the element (typically height of the Timeline's inner 'border' field)</param>
        /// <param name="seconds">Position on the timeline in seconds</param>
        public TimelineElement(Timeline parent, int height, int seconds, int duration, string id)
        {
            InitializeComponent();

            rectOuter.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            CheckIfSelected = false;
            Duration = duration;
            this.seconds = seconds;
            this.parent = parent;
            Id = id;
            rectOuter.Height = height;
            //die Dauer in Pixel umrechnen; 1000 (- 37 ... Breite von TLMark) Pixel = 8 Stunden
            PixProSec = 963.0 / (8 * 60 * 60.0);

            ElementWidth = PixProSec * Duration;
            rectOuter.Width = ElementWidth;

            //rectInner.Height = height;

            // Create friendly time for tooltip text
            setupTooltip();

            // Setup for draggability
            this.MouseEnter += TimelineElement_MouseEnter;
            this.MouseLeave += TimelineElement_MouseLeave;
            this.MouseLeftButtonDown += TimelineElement_MouseLeftButtonDown;


        }

        // Creates tooltip from seconds value
        private void setupTooltip()
        {
            //wird für die Plantafel angepasst
            /*
            string m = (seconds / 60).ToString();
            if (m.Length < 2)
                m = "0" + m;
            string s = (seconds % 60).ToString();
            if (s.Length < 2)
                s = "0" + s;
            display = m + ":" + s;
            mainCanvas.ToolTip = display;*/
            display = parent.getTimeFromSeconds(seconds);
            mainCanvas.ToolTip = display;
        }

        // Called by the parent to give it updated seconds based on its position
        public void SetSeconds(int seconds)
        {
            this.seconds = seconds;
            setupTooltip();
        }

        // Listeners
        private void TimelineElement_MouseEnter(object sender, MouseEventArgs e)
        {
            // Respond visually
            rectOuter.Opacity = 0.5;

            // Prime
            primed = true;
        }
        private void TimelineElement_MouseLeave(object sender, MouseEventArgs e)
        {
            // Respond visually
            if (CheckIfSelected == false)
            {
                rectOuter.Opacity = 1;
            }

            // Deprime
            primed = false;
        }
        private void TimelineElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (primed)
            {

                //Messagebox leeren
                //parent.Messagebox.Text = "";
                sendTextToMessagebox("");

                // Respond visually
                rectOuter.Opacity = 0.6;

                //jetzige position merken
                Originstartposition = Canvas.GetLeft(this);
                Originendposition = Originstartposition + ElementWidth;
                Debug.WriteLine("Setze Origstartposition: " + Originstartposition + "Setze Origenposition: " + Originendposition);

                //als ausgewählt markieren
                CheckIfSelected = true;

                rectOuter.Stroke = new SolidColorBrush(Color.FromRgb(255, 140, 0));
                //alle anderen als nicht ausgewählt markieren
                foreach (TimelineElement x in parent.TElements1)
                {
                    if (x.Id != this.Id) { x.CheckIfSelected = false; x.rectOuter.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0)); x.rectOuter.Opacity = 1; }
                }

                // Enter dragging
                canvasLeft = Canvas.GetLeft(this);
                mouseXInitial = Mouse.GetPosition(parent).X;
                parent.MouseMove += Parent_MouseMove;
                parent.MouseLeftButtonUp += Parent_MouseLeftButtonUp;
                this.ToolTip = "";
            }
        }

        // Dragging handler
        private void Parent_MouseMove(object sender, MouseEventArgs e)
        {
            //anzahl pixel des zurückgelegten weges
            double diff = mouseXInitial - Mouse.GetPosition(parent).X;

            Canvas.SetLeft(this, canvasLeft - diff);
            //Einstellen, dass Element die Timeline "nicht verlassen kann"
            if (Canvas.GetLeft(this) > parent.pixelDistance - 2)
                Canvas.SetLeft(this, parent.pixelDistance - 2);
            if (Canvas.GetLeft(this) < -2)
                Canvas.SetLeft(this, -2);
            DistanceRight = Canvas.GetLeft(this) + ElementWidth;

            if (DistanceRight > parent.pixelDistance - 2)
            {

                Canvas.SetLeft(this, parent.pixelDistance - ElementWidth);
            }

            //Aktuelle Position (Zeit) in Infobox anzeigen

            string curPosAsTime = parent.getTimeFromSeconds((int)((Canvas.GetLeft(this) + 2) / PixProSec) + 8 * 60 * 60);
            parent.changeTextInInfobox(curPosAsTime, Id);
        }
        private void Parent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Respond visually
            if (CheckIfSelected == false)
            {
                rectOuter.Opacity = 1;
            }

            Endposition = Canvas.GetLeft(this) + ElementWidth;
            Startposition = Canvas.GetLeft(this);
            Debug.WriteLine("Setze Startposition: " + Startposition + "Setze Enposition: " + Endposition);

            //überprüfen ob sich Elemente überschneiden und gff warnen bzw Aktion zurücksetzen
            foreach (TimelineElement x in parent.TElements1)
            {

                if (x.Id != this.Id)
                {

                    if (this.Startposition >= x.Startposition & this.Startposition <= x.Endposition)
                    {
                        //parent.Messagebox.Text = "Fehler: Produktionsschritte dürfen sich nicht überschneiden!";
                        sendTextToMessagebox("Fehler: Produktionsschritte dürfen sich nicht überschneiden!");
                        revertMove();
                        Debug.WriteLine(Id + " - 1: Start: " + this.Startposition + " xStart: " + x.Startposition + " Start: " + this.Startposition + " x.End: " + x.Endposition);
                    }
                    if (this.Endposition >= x.Startposition & this.Endposition <= x.Endposition)
                    {
                        //parent.Messagebox.Text = "Fehler: Produktionsschritte dürfen sich nicht überschneiden!";
                        sendTextToMessagebox("Fehler: Produktionsschritte dürfen sich nicht überschneiden!");
                        revertMove();
                        Debug.WriteLine(Id + " - 2: End: " + this.Endposition + " xStart: " + x.Startposition + " End: " + this.Endposition + " x.End: " + x.Endposition);
                    }
                }
            }

            // Reset to default 'stance'
            primed = false;
            parent.MouseMove -= Parent_MouseMove;
            parent.MouseLeftButtonUp -= Parent_MouseLeftButtonUp;
            parent.RefreshElement(this); // Notify parent to give this element new data about its position in the timeline in seconds

            //als BindingParameter fürs Mainviewmodel wird die ID des Elements mitgegeben
            foreach (InputBinding item in rectOuter.InputBindings)
            {
                item.CommandParameter = Id;
            }

        }

        private void revertMove()
        {
            Canvas.SetLeft(this, Originstartposition);

        }

        public void savePositions()
        {
            Originstartposition = Canvas.GetLeft(this);
            Originendposition = Originstartposition + ElementWidth;
            Startposition = Originstartposition;
            Endposition = Originendposition;
            Debug.WriteLine("" + Startposition + Endposition);
        }

        public void sendTextToMessagebox(string s)
        {

           //PlantafelVm.Instance.MessageBoxEntry = s;

        }
    }
}
