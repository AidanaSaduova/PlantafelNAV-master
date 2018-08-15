using PlantafelNAV.TimelineNAV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlantafelNAV.Views
{
    /// <summary>
    /// Interaction logic for Plantafel.xaml
    /// </summary>
    public partial class Plantafel : UserControl
    {

        private static readonly Plantafel instance = new Plantafel();
        public static Plantafel Instance
        {
            get { return instance; }
        }


        public Plantafel()
        {
            
            InitializeComponent();
            
            Timeline t1 = generateTimeline(1);
            Timeline t2 = generateTimeline(2);
            Timeline t3 = generateTimeline(3);
            Timeline t4 = generateTimeline(4);
            //in sekunden
            addTimelineEntry(t1, 8 * 60 * 60, 60 * 60 * 2, "#12345");
            addTimelineEntry(t1, 12 * 60 * 60, 60 * 60 * 2, "#98765");

            addTimelineEntry(t2, 8 * 60 * 60, 60 * 60 * 2, "#12345");
            addTimelineEntry(t2, 12 * 60 * 60, 60 * 60 * 2, "#98765");

            addTimelineEntry(t3, 8 * 60 * 60, 60 * 60 * 2, "#12345");
            addTimelineEntry(t3, 12 * 60 * 60, 60 * 60 * 2, "#98765");

            addTimelineEntry(t4, 8 * 60 * 60, 60 * 60 * 2, "#12345");
            addTimelineEntry(t4, 12 * 60 * 60, 60 * 60 * 2, "#98765");
        }
 
        private void addTimelineEntry(Timeline tl, int Starttime, int Duration, string Id)
        {

            tl.AddElement(Starttime, Duration, Id);
        }

        public Timeline generateTimeline(int hostnumber)
        {
            //ACHTUNG: Bei Änderung ist zu beachten, dass die Länge eines Eintrages von Minuten abhängt, die anhand der Länge der Timeline (1000) in Pixel umgerechnet werden; Diese Umrechnung muss dann im TimeLineElement-File geändert werden
            Timeline tl = new Timeline(1000, 80);
            //Setupaufruf mit Startzeit 08:00, Endzeit 16:00, Stundenintervallen sowie 
            tl.Setup(28800, 57600, 3600, 120);
            switch (hostnumber)
            {
                case 1:
                    host1.Children.Add(tl);
                    break;
                case 2:
                    host2.Children.Add(tl);
                    break;
                case 3:
                    host3.Children.Add(tl);
                    break;
                case 4:
                    host4.Children.Add(tl);
                    break;
            }

            return tl;

        }

        private void msgbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
