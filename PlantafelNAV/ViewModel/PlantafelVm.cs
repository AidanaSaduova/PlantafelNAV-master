using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PlantafelNAV.ws_production_service_2;
using System.Windows;

namespace PlantafelNAV.ViewModel
{

    public class PlantafelVm:ViewModelBase
    {
        // WS_Production_Service ws_prod_service = new WS_Production_Service();
        WS_Production_Service ws_productionservice = new WS_Production_Service();

        /*private static readonly PlantafelVm instance = new PlantafelVm();
        public static PlantafelVm Instance
        {
            get { return instance; }
        }*/

        ObservableCollection<WS_Production> _production = new ObservableCollection<WS_Production>();
        private string messageBoxEntry;
        private DateTime prod_date;

        public string MessageBoxEntry { get { return messageBoxEntry; } set { messageBoxEntry = value; RaisePropertyChanged(); } }



        public DateTime Prod_date { get { return prod_date; } set { prod_date = value; Debug.WriteLine(prod_date.ToLongDateString()); } }

        public ObservableCollection<WS_Production> Production { get => _production; set => _production = value; }

        public PlantafelVm()
        {
            ws_productionservice.UseDefaultCredentials = true;

            Prod_date = DateTime.Now;
            loadProduction();

        }


        public void loadProduction()
        {
            Production.Clear();

            WS_Production[] list = ws_productionservice.ReadMultiple(null, null, 100);

            foreach (WS_Production x in list)
            {
                WS_Production tmp = new WS_Production();
                tmp.No = x.No;
                tmp.Starting_Date = x.Starting_Date;
                tmp.Starting_Time = x.Starting_Time;
                MessageBox.Show("Startdatum: " + tmp.Starting_Date + "; Startzeit: " + tmp.Starting_Time);
                Production.Add(tmp);
            }


        }
    }
}
