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
using GalaSoft.MvvmLight.Messaging;

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
        private string auftrNrDetailbox;
        private DateTime prod_date;

        public string MessageBoxEntry { get { return messageBoxEntry; } set { messageBoxEntry = value; RaisePropertyChanged(); } }

        public DateTime Prod_date { get { return prod_date; } set { prod_date = value; Debug.WriteLine(prod_date.ToLongDateString()); } }

        public ObservableCollection<WS_Production> Production { get => _production; set => _production = value; }
        public ObservableCollection<WS_Arbeitzplatz> Arbeitsplaetze { get => _arbeitsplaetze; set => _arbeitsplaetze = value; }
        public WS_Production Tmp_production { get => _tmp_production; set => _tmp_production = value; }
        public ObservableCollection<WS_Auf_Arb_Nav> Auftraegenav { get => _auftraegenav; set => _auftraegenav = value; }
        public string AuftrNrDetailbox { get { return auftrNrDetailbox; } set { auftrNrDetailbox = value; RaisePropertyChanged();   } }

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

        //Auftragsdetailbox aktualisieren
        public void createDetails(string auftragNr)
        {
            AuftrNrDetailbox = auftragNr;
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

        //Aufträge in Tabelle mit Nav abgleichen
        public void cleanProductions()
        {
            WS_Auf_Arb_Nav[] list = ws_auftragnavservice.ReadMultiple(null, null, 100);
            foreach(WS_Auf_Arb_Nav x in list)
            {
                WS_Production[] list2 = ws_productionservice.ReadMultiple(null, null, 100);
                int check = 0;
                foreach (WS_Production y in list2)
                {
                    
                    if (y.No == x.Auftragsnr) { check = 1; }
                }
                if(check == 0)
                {
                    ws_auftragnavservice.Delete(x.Key);
                }
            }
        }

        //Auftrag in Nav aktualisieren
        public void doUpdateProdNav(string nr, DateTime date)
        {
            WS_Production[] list = ws_productionservice.ReadMultiple(null, null, 100);
            foreach(WS_Production x in list)
            {
                if(x.No == nr)
                {
                    WS_Production y = new WS_Production();
                    y.Key = x.Key;
                    y.Ending_Date = date;
                    y.Ending_Time = date;
                    y.Quantity = 3;
                    ws_productionservice.Update(ref y);

                    ws_productionservice.UpdateAsync(y);
                 

                    Debug.WriteLine(y.No + "  " + y.Ending_Date);
                }

            }
            
           
        }

        //neuen Auftrag in Tabelle erstellem
         public void newProdToTable(string Auftragsnummer, DateTime start, DateTime end, DateTime ApStart1, DateTime ApStart2, DateTime ApStart3, DateTime ApStart4, DateTime ApEnd1, DateTime ApEnd2, DateTime ApEnd3, DateTime ApEnd4)
      
        {
            WS_Auf_Arb_Nav x = new WS_Auf_Arb_Nav();
            x.AP1_Startdatum = ApStart1.ToString();
            x.AP2_Startdatum = ApStart2.ToString();
            x.AP3_Startdatum = ApStart3.ToString();
            x.AP4_Startdatum = ApStart4.ToString();
            x.AP1_Enddatum = ApEnd1.ToString();
            x.AP2_Enddatum = ApEnd2.ToString();
            x.AP3_Enddatum = ApEnd3.ToString();
            x.AP4_Enddatum = ApEnd4.ToString();
            x.Start = start.ToString();
            x.Ende = end.ToString();
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

                if (convertStringToDate(x.AP1_Startdatum).Date == date.Date) { TMElement y = new TMElement(); y.Id = x.Auftragsnr; y.StartDate = convertStringToDate(x.AP1_Startdatum); y.EndDate = convertStringToDate(x.AP1_Enddatum); y.TimeLineNumber = 1; prods.Add(y); }
                if (convertStringToDate(x.AP2_Startdatum).Date == date.Date) { TMElement y = new TMElement(); y.Id = x.Auftragsnr; y.StartDate = convertStringToDate(x.AP2_Startdatum); y.EndDate = convertStringToDate(x.AP2_Enddatum); y.TimeLineNumber = 2; prods.Add(y); }
                if (convertStringToDate(x.AP3_Startdatum).Date == date.Date) { TMElement y = new TMElement(); y.Id = x.Auftragsnr; y.StartDate = convertStringToDate(x.AP3_Startdatum); y.EndDate = convertStringToDate(x.AP3_Enddatum); y.TimeLineNumber = 3; prods.Add(y); }
                if (convertStringToDate(x.AP4_Startdatum).Date == date.Date) { TMElement y = new TMElement(); y.Id = x.Auftragsnr; y.StartDate = convertStringToDate(x.AP4_Startdatum); y.EndDate = convertStringToDate(x.AP4_Enddatum); y.TimeLineNumber = 4; prods.Add(y); }
            }
            return prods;

        }

        //da es nicht möglich ist ein datetime in die tabelle zu schreiben (grund auch nach intensiver suche unbekannt), wird jetzt die toStringFunktion benutzt, die jedoch wieder in datetimes umgewanfdelt werden müssen 
        public DateTime convertStringToDate(string date)
        {
            if (date != null)
            {
                return DateTime.Parse(date);
            }
            else { return new DateTime(); }
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
