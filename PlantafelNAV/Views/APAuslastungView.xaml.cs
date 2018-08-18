using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using PlantafelNAV.TimelineNAV;
using PlantafelNAV.ViewModel.Helpers;
using PlantafelNAV.ws_aufarbservice;

namespace PlantafelNAV.Views
{
    /// <summary>
    /// Interaction logic for APAuslastungView.xaml
    /// </summary>
    public partial class APAuslastungView : UserControl
    {

        public APAuslastungView()
        {
           
            InitializeComponent();
            LoadBarChartData();
        }


        private void LoadBarChartData()

        {

            ((BarSeries)mcChart.Series[0]).ItemsSource =

                new KeyValuePair<string, int>[]{

            new KeyValuePair<string, int>("Project Manager", 12),

            new KeyValuePair<string, int>("CEO", 25),

            new KeyValuePair<string, int>("Software Engg.", 5),

            new KeyValuePair<string, int>("Team Leader", 6),

            new KeyValuePair<string, int>("Project Leader", 10),

            new KeyValuePair<string, int>("Developer", 4) };

        }
    }
}
