using System;
using System.Collections.Generic;
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
using PlantafelNAV.TimelineNAV;
using PlantafelNAV.ws_arbeitsplatzservice;
using PlantafelNAV.ws_mbplantafel_service;

namespace PlantafelNAV.Views
{
    /// <summary>
    /// Interaction logic for APAuslastungView.xaml
    /// </summary>
    public partial class APAuslastungView : UserControl
    {
        //Timelines erstellen - in diesem Fall werden diese dann als Auslastungsbalken dienen
        //Tag = 8h -> 480 min -> 1% des Arbeitstages = 4,8 min 
        Timeline t1 = new Timeline(1000, 80, 1);
        Timeline t2 = new Timeline(1000, 80, 1);
        Timeline t3 = new Timeline(1000, 80, 1);
        Timeline t4 = new Timeline(1000, 80, 1);

        public APAuslastungView()
        {
            
            InitializeComponent();
            //erzeugen der Timelines für die max größe 
            t1.Setup(28800, 57600, 3600, 120);
            t2.Setup(28800, 57600, 3600, 120);
            t3.Setup(28800, 57600, 3600, 120);
            t4.Setup(28800, 57600, 3600, 120);

            host1.Children.Add(t1);
            host2.Children.Add(t2);
            host3.Children.Add(t3);
            host4.Children.Add(t4);

        }
    }
}
