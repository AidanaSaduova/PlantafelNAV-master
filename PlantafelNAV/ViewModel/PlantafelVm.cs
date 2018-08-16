using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PlantafelNAV.ws_production_service_2;
using PlantafelNAV.ws_arbeitsplatzservice;
using PlantafelNAV.ws_aufarbservice;
using System.Windows;
using PlantafelNAV.TimelineNAV;
using PlantafelNAV.ViewModel.Helpers;

namespace PlantafelNAV.ViewModel
{

    public class PlantafelVm : ViewModelBase
    {

        #region Services
        WS_Production_Service ws_productionservice = new WS_Production_Service();
        WS_Arbeitzplatz_Service ws_serviceap = new WS_Arbeitzplatz_Service();
        WS_Auf_Arb_Nav_Service ws_auftragnavservice = new WS_Auf_Arb_Nav_Service();
        #endregion

        #region Collections
        ObservableCollection<WS_Production> _production = new ObservableCollection<WS_Production>();
        ObservableCollection<WS_Arbeitzplatz> _arbeitsplaetze = new ObservableCollection<WS_Arbeitzplatz>();
        ObservableCollection<WS_Auf_Arb_Nav> _auftraegenav = new ObservableCollection<WS_Auf_Arb_Nav>();
        #endregion

        WS_Production _tmp_production;

        #region Properties

        private string messageBoxEntry;
        private DateTime prod_date;

        public string MessageBoxEntry { get { return messageBoxEntry; } set { messageBoxEntry = value; RaisePropertyChanged(); } }

        public DateTime Prod_date { get { return prod_date; } set { prod_date = value; Debug.WriteLine(prod_date.ToLongDateString()); } }

        public ObservableCollection<WS_Production> Production { get => _production; set => _production = value; }
        public ObservableCollection<WS_Arbeitzplatz> Arbeitsplaetze { get => _arbeitsplaetze; set => _arbeitsplaetze = value; }
        public WS_Production Tmp_production { get => _tmp_production; set => _tmp_production = value; }
        public ObservableCollection<WS_Auf_Arb_Nav> Auftraegenav { get => _auftraegenav; set => _auftraegenav = value; }

        public TMElement test = new TMElement();

        #endregion


        public PlantafelVm()
        {
            ws_productionservice.UseDefaultCredentials = true;
            ws_serviceap.UseDefaultCredentials = true;
            ws_auftragnavservice.UseDefaultCredentials = true;

            Prod_date = DateTime.Now;
            loadProduction();
            loadArbeitsplatz();

        }

        public void createTimeLine()
        {



        }

        //die komplette DLFZeit pro Arbeitsplatz
        public decimal calculateRuntime(int arbeitsplatz, decimal amount)
        {
            decimal[] runtime = new decimal[4];
            int i = 0;
            foreach (WS_Arbeitzplatz x in Arbeitsplaetze)
            {
                runtime[i] = (x.Run_Time * amount) + x.Setup_Time;
                i++;
            }

            switch (arbeitsplatz)
            {
                case 1:
                    return runtime[0];

                case 2:
                    return runtime[1];

                case 3:
                    return runtime[2];

                case 4:
                    return runtime[3];

            }
            return 0;
        }

        public void loadProduction()
        {
            Production.Clear();

            WS_Production[] list = ws_productionservice.ReadMultiple(null, null, 100);



            foreach (WS_Production x in list)
            {
                WS_Production tmp = new WS_Production();
                tmp.No = x.No;
                tmp.Description = x.Description;
                tmp.Quantity = x.Quantity;
                tmp.Starting_Date = x.Starting_Date;
                tmp.Starting_Time = x.Starting_Time;
                //MessageBox.Show("Startdatum: " + tmp.Starting_Date + "; Startzeit: " + tmp.Starting_Time);
                Production.Add(tmp);
            }

        }

        //Auftrag in Nav aktualisieren
        public void doUpdateProdNav(string nr, DateTime date)
        {
            WS_Auf_Arb_Nav x = ws_auftragnavservice.Read(nr);
            ws_auftragnavservice.Update(ref x);
        }

        //neuen Auftrag in Tabelle erstellem
        public void newProdToTable(string Auftragsnummer, DateTime start, DateTime end, DateTime ApStart1, DateTime ApStart2, DateTime ApStart3, DateTime ApStart4, DateTime ApEnd1, DateTime ApEnd2, DateTime ApEnd3, DateTime ApEnd4)
        {
            WS_Auf_Arb_Nav x = new WS_Auf_Arb_Nav();
            x.AP1_Startdatum = ApStart1;
            x.AP2_Startdatum = ApStart2;
            x.AP3_Startdatum = ApStart3;
            x.AP4_Startdatum = ApStart4;
            x.AP1_Enddatum = ApEnd1;
            x.AP2_Enddatum = ApEnd2;
            x.AP3_Enddatum = ApEnd3;
            x.AP4_Enddatum = ApEnd4;
            x.Start = start;
            x.Ende = end;
            x.Auftragsnr = Auftragsnummer;

            ws_auftragnavservice.Create(ref x);
        }

        //alle aufträge eines bestimmten Datums holen
        public ObservableCollection<TMElement> getProdOfSpecDate(DateTime date)
        {

            //der filter funktioniert leider nicht so wie ich das will -> also schleife
            /* List<WS_Auf_Arb_Nav_Filter> filterArray = new List<WS_Auf_Arb_Nav_Filter>();
             WS_Auf_Arb_Nav_Filter dateFilter = new WS_Auf_Arb_Nav_Filter();
             dateFilter.Field = WS_Auf_Arb_Nav_Fields.AP1_Startdatum;
             dateFilter.Criteria = date;
             filterArray.Add(nameFilter);*/

            WS_Auf_Arb_Nav[] list = ws_auftragnavservice.ReadMultiple(null, null, 100);
            ObservableCollection<TMElement> prods = new ObservableCollection<TMElement>();
            foreach (WS_Auf_Arb_Nav x in list)
            {
                TMElement y = new TMElement();
                if (x.AP1_Startdatum.Date == date) { y.Id = x.Auftragsnr; y.StartDate = x.AP1_Startdatum; y.EndDate = x.AP1_Enddatum; y.TimeLineNumber = 1; prods.Add(y); }
                else if (x.AP2_Startdatum.Date == date) { y.Id = x.Auftragsnr; y.StartDate = x.AP2_Startdatum; y.EndDate = x.AP2_Enddatum; y.TimeLineNumber = 2; prods.Add(y); }
                else if (x.AP3_Startdatum.Date == date) { y.Id = x.Auftragsnr; y.StartDate = x.AP3_Startdatum; y.EndDate = x.AP3_Enddatum; y.TimeLineNumber = 3; prods.Add(y); }
                else if (x.AP4_Startdatum.Date == date) { y.Id = x.Auftragsnr; y.StartDate = x.AP4_Startdatum; y.EndDate = x.AP4_Enddatum; y.TimeLineNumber = 4; prods.Add(y); }
            }
            return prods;

        }

        //überprüfen ob Auftrag schon in neuer Tabelle
        public WS_Auf_Arb_Nav getSpecAuftrag(string Auftragsnummer)
        {
            WS_Auf_Arb_Nav x = ws_auftragnavservice.Read(Auftragsnummer);
            if (x != null)
            {
                return x;
            }
            else { return null; }
        }

        public void loadArbeitsplatz()
        {
            Arbeitsplaetze.Clear();
            WS_Arbeitzplatz[] list = ws_serviceap.ReadMultiple(null, null, 100);

            foreach (WS_Arbeitzplatz x in list)
            {
                WS_Arbeitzplatz tmp = new WS_Arbeitzplatz();
                tmp.No = x.No;
                tmp.Description = x.Description;
                tmp.Setup_Time = x.Setup_Time;
                tmp.Wait_Time = x.Wait_Time;
                tmp.Run_Time = x.Run_Time;
                tmp.Move_Time = x.Move_Time;
                tmp.Key = x.Key;
                Arbeitsplaetze.Add(tmp);
            }
        }
    }
}
