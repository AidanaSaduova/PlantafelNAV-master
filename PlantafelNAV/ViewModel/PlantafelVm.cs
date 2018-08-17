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

        public DateTime Prod_date { get { return prod_date; } set { prod_date = value; Debug.WriteLine("Date set: " + prod_date.ToLongDateString()); } }

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

        //return quantity 
        public decimal returnQuantity(string auftragnr)
        {
            WS_Production x = ws_productionservice.Read(auftragnr);
            return x.Quantity;
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

        public void neuberechnungAuftrag(string auftrgnr, string startzeit, int ap)
        {
            WS_Auf_Arb_Nav x = getSpecAuftrag(auftrgnr);

            DateTime datum = DateTime.Parse(x.AP1_Startdatum);

            DateTime start = DateTime.ParseExact(startzeit, "H:mm", null, System.Globalization.DateTimeStyles.None);
            //bei start noch das datum richtigstellen
            start = new DateTime(datum.Year, datum.Month, datum.Day, start.Hour, start.Minute, start.Millisecond);
            Debug.WriteLine("Betdatum: " + start);
            decimal quantity = returnQuantity(auftrgnr);

            //dlfz für jeden schritt
            decimal ApDLFZ1 = calculateRuntime(1, quantity);
            decimal ApDLFZ2 = calculateRuntime(2, quantity);
            decimal ApDLFZ3 = calculateRuntime(3, quantity);
            decimal ApDLFZ4 = calculateRuntime(4, quantity);

            //Anfangszeiten berechnen
            DateTime ApStart1;
            DateTime ApEnd1;
            DateTime ApStart2;
            DateTime ApEnd2;
            DateTime ApStart3;
            DateTime ApEnd3;
            DateTime ApStart4;
            DateTime ApEnd4;
            DateTime tmp;
            DateTime tmp1;

            //je nachdem in welcher timeline sich das elementbefindet werden nur die elemente die in späteren Timelines kommen neu berechnet
            if (ap == 1)
            {
                ApStart1 = start;
                ApEnd1 = start.AddMinutes((double)ApDLFZ1);
                ApStart1 = start;
                ApEnd1 = start.AddMinutes((double)ApDLFZ1);
                //Überprüfen ob sich der zweite noch ausgeht//
                tmp = ApEnd1.AddMinutes((double)ApDLFZ2);
                tmp1 = new DateTime(datum.Year, datum.Month, datum.Day, 16, 00, 00);
                //geht sich noch an diesem Tag aus?
                if (tmp.CompareTo(tmp1) != 1)
                {
                    ApStart2 = ApEnd1;
                    ApEnd2 = ApStart2.AddMinutes((double)ApDLFZ2);
                }
                else
                {
                    DateTime nextDay = new DateTime(ApEnd1.Year, ApEnd1.Month, ApEnd1.Day, 08, 00, 00);
                    nextDay = nextDay.AddDays(1);
                    ApStart2 = nextDay;
                    ApEnd2 = nextDay.AddMinutes((double)ApDLFZ2);
                }
                //Überprüfen ob sich der dritte noch ausgeht//
                tmp = ApEnd2.AddMinutes((double)ApDLFZ3);
                tmp1 = new DateTime(ApEnd2.Year, ApEnd2.Month, ApEnd2.Day, 16, 00, 00);
                //geht sich noch an diesem Tag aus?
                if (tmp.CompareTo(tmp1) != 1)
                {
                    ApStart3 = ApEnd2;
                    ApEnd3 = ApStart3.AddMinutes((double)ApDLFZ3);
                }
                else
                {
                    DateTime nextDay = new DateTime(ApEnd2.Year, ApEnd2.Month, ApEnd2.Day, 08, 00, 00);
                    nextDay = nextDay.AddDays(1);
                    ApStart3 = nextDay;
                    ApEnd3 = nextDay.AddMinutes((double)ApDLFZ3);
                }
                //Überprüfen ob sich der vierte noch ausgeht//
                tmp = ApEnd3.AddMinutes((double)ApDLFZ4);
                tmp1 = new DateTime(ApEnd3.Year, ApEnd3.Month, ApEnd3.Day, 16, 00, 00);
                //geht sich noch an diesem Tag aus?
                if (tmp.CompareTo(tmp1) != 1)
                {
                    ApStart4 = ApEnd3;
                    ApEnd4 = ApStart4.AddMinutes((double)ApDLFZ4);
                }
                else
                {
                    DateTime nextDay = new DateTime(ApEnd3.Year, ApEnd3.Month, ApEnd3.Day, 08, 00, 00);
                    nextDay = nextDay.AddDays(1);
                    ApStart4 = nextDay;
                    ApEnd4 = nextDay.AddMinutes((double)ApDLFZ4);
                }
                updateAuftrag(auftrgnr, ApStart1, ApStart2, ApStart3, ApStart4, ApEnd1, ApEnd2, ApEnd3, ApEnd4);
            }
            if(ap == 2)
            {
                //Überprüfen ob sich der zweite noch ausgeht//
                tmp = DateTime.Parse(x.AP1_Enddatum).AddMinutes((double)ApDLFZ2);
                tmp1 = new DateTime(datum.Year, datum.Month, datum.Day, 16, 00, 00);
                //geht sich noch an diesem Tag aus?
                if (tmp.CompareTo(tmp1) != 1)
                {
                    ApStart2 = DateTime.Parse(x.AP1_Enddatum);
                    ApEnd2 = ApStart2.AddMinutes((double)ApDLFZ2);
                }
                else
                {
                    DateTime nextDay = new DateTime(DateTime.Parse(x.AP1_Enddatum).Year, DateTime.Parse(x.AP1_Enddatum).Month, DateTime.Parse(x.AP1_Enddatum).Day, 08, 00, 00);
                    nextDay = nextDay.AddDays(1);
                    ApStart2 = nextDay;
                    ApEnd2 = nextDay.AddMinutes((double)ApDLFZ2);
                }
                //Überprüfen ob sich der dritte noch ausgeht//
                tmp = ApEnd2.AddMinutes((double)ApDLFZ3);
                tmp1 = new DateTime(ApEnd2.Year, ApEnd2.Month, ApEnd2.Day, 16, 00, 00);
                //geht sich noch an diesem Tag aus?
                if (tmp.CompareTo(tmp1) != 1)
                {
                    ApStart3 = ApEnd2;
                    ApEnd3 = ApStart3.AddMinutes((double)ApDLFZ3);
                }
                else
                {
                    DateTime nextDay = new DateTime(ApEnd2.Year, ApEnd2.Month, ApEnd2.Day, 08, 00, 00);
                    nextDay = nextDay.AddDays(1);
                    ApStart3 = nextDay;
                    ApEnd3 = nextDay.AddMinutes((double)ApDLFZ3);
                }
                //Überprüfen ob sich der vierte noch ausgeht//
                tmp = ApEnd3.AddMinutes((double)ApDLFZ4);
                tmp1 = new DateTime(ApEnd3.Year, ApEnd3.Month, ApEnd3.Day, 16, 00, 00);
                //geht sich noch an diesem Tag aus?
                if (tmp.CompareTo(tmp1) != 1)
                {
                    ApStart4 = ApEnd3;
                    ApEnd4 = ApStart4.AddMinutes((double)ApDLFZ4);
                }
                else
                {
                    DateTime nextDay = new DateTime(ApEnd3.Year, ApEnd3.Month, ApEnd3.Day, 08, 00, 00);
                    nextDay = nextDay.AddDays(1);
                    ApStart4 = nextDay;
                    ApEnd4 = nextDay.AddMinutes((double)ApDLFZ4);
                }
                updateAuftrag(auftrgnr, DateTime.Parse(x.AP1_Startdatum), ApStart2, ApStart3, ApStart4, DateTime.Parse(x.AP1_Enddatum), ApEnd2, ApEnd3, ApEnd4);
            }
            if(ap == 3)
            {
                
                //Überprüfen ob sich der dritte noch ausgeht//
                tmp = DateTime.Parse(x.AP2_Enddatum).AddMinutes((double)ApDLFZ3);
                tmp1 = new DateTime(DateTime.Parse(x.AP2_Enddatum).Year, DateTime.Parse(x.AP2_Enddatum).Month, DateTime.Parse(x.AP2_Enddatum).Day, 16, 00, 00);
                //geht sich noch an diesem Tag aus?
                if (tmp.CompareTo(tmp1) != 1)
                {
                    ApStart3 = DateTime.Parse(x.AP2_Enddatum);
                    ApEnd3 = ApStart3.AddMinutes((double)ApDLFZ3);
                }
                else
                {
                    DateTime nextDay = new DateTime(DateTime.Parse(x.AP2_Enddatum).Year, DateTime.Parse(x.AP2_Enddatum).Month, DateTime.Parse(x.AP2_Enddatum).Day, 08, 00, 00);
                    nextDay = nextDay.AddDays(1);
                    ApStart3 = nextDay;
                    ApEnd3 = nextDay.AddMinutes((double)ApDLFZ3);
                }
                //Überprüfen ob sich der vierte noch ausgeht//
                tmp = ApEnd3.AddMinutes((double)ApDLFZ4);
                tmp1 = new DateTime(ApEnd3.Year, ApEnd3.Month, ApEnd3.Day, 16, 00, 00);
                //geht sich noch an diesem Tag aus?
                if (tmp.CompareTo(tmp1) != 1)
                {
                    ApStart4 = ApEnd3;
                    ApEnd4 = ApStart4.AddMinutes((double)ApDLFZ4);
                }
                else
                {
                    DateTime nextDay = new DateTime(ApEnd3.Year, ApEnd3.Month, ApEnd3.Day, 08, 00, 00);
                    nextDay = nextDay.AddDays(1);
                    ApStart4 = nextDay;
                    ApEnd4 = nextDay.AddMinutes((double)ApDLFZ4);
                }
                updateAuftrag(auftrgnr, DateTime.Parse(x.AP1_Startdatum), DateTime.Parse(x.AP2_Startdatum), ApStart3, ApStart4, DateTime.Parse(x.AP1_Enddatum), DateTime.Parse(x.AP2_Enddatum), ApEnd3, ApEnd4);
            }
            if(ap == 4)
            {
                //Überprüfen ob sich der vierte noch ausgeht//
                tmp = DateTime.Parse(x.AP3_Enddatum).AddMinutes((double)ApDLFZ4);
                tmp1 = new DateTime(DateTime.Parse(x.AP3_Enddatum).Year, DateTime.Parse(x.AP3_Enddatum).Month, DateTime.Parse(x.AP3_Enddatum).Day, 16, 00, 00);
                //geht sich noch an diesem Tag aus?
                if (tmp.CompareTo(tmp1) != 1)
                {
                    ApStart4 = DateTime.Parse(x.AP3_Enddatum);
                    ApEnd4 = ApStart4.AddMinutes((double)ApDLFZ4);
                }
                else
                {
                    DateTime nextDay = new DateTime(DateTime.Parse(x.AP3_Enddatum).Year, DateTime.Parse(x.AP3_Enddatum).Month, DateTime.Parse(x.AP3_Enddatum).Day, 08, 00, 00);
                    nextDay = nextDay.AddDays(1);
                    ApStart4 = nextDay;
                    ApEnd4 = nextDay.AddMinutes((double)ApDLFZ4);
                }
                updateAuftrag(auftrgnr, DateTime.Parse(x.AP1_Startdatum), DateTime.Parse(x.AP2_Startdatum), DateTime.Parse(x.AP4_Startdatum), ApStart4, DateTime.Parse(x.AP1_Enddatum), DateTime.Parse(x.AP2_Enddatum), DateTime.Parse(x.AP3_Enddatum), ApEnd4);
            }
            

            //Debug.WriteLine("NR: " + auftrgnr + " S1: " + ApStart1 + " E1: " + ApEnd1 + " S2: " + ApStart2 + " E2: " + ApEnd2 + " S3: " + ApStart3 + " E3: " + ApEnd3 + " S4: " + ApStart4 + " E4: " + ApEnd4);
        }

        private void updateAuftrag(string AuftrNr, DateTime ApStart1, DateTime ApStart2, DateTime ApStart3, DateTime ApStart4, DateTime ApEnd1, DateTime ApEnd2, DateTime ApEnd3, DateTime ApEnd4)
        {
  
            WS_Auf_Arb_Nav x = getSpecAuftrag(AuftrNr);
            x.AP1_Startdatum = ApStart1.ToString();
            x.AP2_Startdatum = ApStart2.ToString();
            x.AP3_Startdatum = ApStart3.ToString();
            x.AP4_Startdatum = ApStart4.ToString();
            x.AP1_Enddatum = ApEnd1.ToString();
            x.AP2_Enddatum = ApEnd2.ToString();
            x.AP3_Enddatum = ApEnd3.ToString();
            x.AP4_Enddatum = ApEnd4.ToString();
            x.Start = ApStart1.ToString();
            x.Ende = ApEnd4.ToString();
            ws_auftragnavservice.Update(ref x);
        }
    }
}
