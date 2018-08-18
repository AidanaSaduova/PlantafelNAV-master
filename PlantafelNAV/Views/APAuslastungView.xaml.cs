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
        WS_Auf_Arb_Nav_Service ws_arbeitsplan = new WS_Auf_Arb_Nav_Service();

        DateTime _datum = new DateTime();
        private Int16 ap1Duration;
        private Int16 ap2Duration;
        private Int16 ap3Duration;
        private Int16 ap4Duration;

        Collection<AP_Auslastung> _itemsAP = new Collection<AP_Auslastung>();
        private int duration;

        public DateTime Datum { get { return _datum; } set { _datum = value; doUpdate(); } }

        public short Ap1Duration { get => ap1Duration; set => ap1Duration = value; }
        public short Ap2Duration { get => ap2Duration; set => ap2Duration = value; }
        public short Ap3Duration { get => ap3Duration; set => ap3Duration = value; }
        public short Ap4Duration { get => ap4Duration; set => ap4Duration = value; }
        public Collection<AP_Auslastung> ItemsAP { get => _itemsAP; set => _itemsAP = value; }

        public APAuslastungView()
        {

            InitializeComponent();

            ws_arbeitsplan.UseDefaultCredentials = true;
            Datum = DateTime.Now;
            doUpdate();
            LoadBarChartData();
        }

        private void doUpdate()
        {
            readData();
            showData();
        }

        private void showData()
        {
            string[] tmp = new string[2];
            tmp[0] = "Id";
            Messenger.Default.Send<string[], Views.APAuslastungView>(tmp);

            ItemsAP.Add(new AP_Auslastung(Ap1Duration, "Schmelzofen"));
            ItemsAP.Add(new AP_Auslastung(Ap2Duration, "CNC Drehmaschine"));
            ItemsAP.Add(new AP_Auslastung(Ap3Duration, "Ausrichtestation"));
            ItemsAP.Add(new AP_Auslastung(Ap4Duration, "Schwingungssensor"));


        }

        private void readData()
        {
            WS_Auf_Arb_Nav[] list = ws_arbeitsplan.ReadMultiple(null, null, 1000);
            foreach (WS_Auf_Arb_Nav item in list)
            {
                if (DateTime.Parse(item.AP1_Startdatum).Date == Datum.Date) { Ap1Duration = generateDuration(item.AP1_Startdatum, item.AP1_Enddatum); }
                if (DateTime.Parse(item.AP2_Startdatum).Date == Datum.Date) { Ap2Duration = generateDuration(item.AP2_Startdatum, item.AP2_Enddatum); }
                if (DateTime.Parse(item.AP3_Startdatum).Date == Datum.Date) { Ap3Duration = generateDuration(item.AP3_Startdatum, item.AP3_Enddatum); }
                if (DateTime.Parse(item.AP4_Startdatum).Date == Datum.Date) { Ap4Duration = generateDuration(item.AP4_Startdatum, item.AP4_Enddatum); }
            }

        }

        private Int16 generateDuration(string aP1_Startdatum, string aP1_Enddatum)
        {
            TimeSpan diff = DateTime.Parse(aP1_Enddatum) - DateTime.Parse(aP1_Startdatum);
            Int16 minutes = (Int16)diff.TotalMinutes;
            return minutes;
        }

        private void LoadBarChartData()
        {

            ((BarSeries)mcChart.Series[0]).ItemsSource =

                new KeyValuePair<string, int>[]{

            new KeyValuePair<string, int>("Project Manager", ap1Duration),

            new KeyValuePair<string, int>("CEO", 25),

            new KeyValuePair<string, int>("Software Engg.", 5),

            new KeyValuePair<string, int>("Team Leader", 6),

            new KeyValuePair<string, int>("Project Leader", 10),

            new KeyValuePair<string, int>("Developer", 4) };

        }
    }
}
