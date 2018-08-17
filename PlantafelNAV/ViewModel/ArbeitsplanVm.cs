using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PlantafelNAV.ws_mbplantafel_service;
using PlantafelNAV.ws_mitarbeiterservice;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.CommandWpf;

namespace PlantafelNAV.ViewModel
{
    public class ArbeitsplanVm:ViewModelBase
    {
        DateTime arbeitsplatzdatum;
        ws_mitarbeiter_Service ws_mbservice = new ws_mitarbeiter_Service();
        WS_MB_Plantafel_Service ws_navmbservice = new WS_MB_Plantafel_Service();

        ObservableCollection<WS_MB_Plantafel> _colnavmb = new ObservableCollection<WS_MB_Plantafel>();
        ObservableCollection<ws_mitarbeiter> _colmb = new ObservableCollection<ws_mitarbeiter>();

         public ArbeitsplanVm()
        {
            ws_mbservice.UseDefaultCredentials = true;
            ws_navmbservice.UseDefaultCredentials = true;
            Arbeitsplatzdatum = DateTime.Now;
            doUpdate();
            AddRC = new RelayCommand<string>((s) => addMB(s));
            RemoveRC = new RelayCommand<string>((s) => removeMB(s));
           
        }

        private void removeMB(string s)
        {
            if (s == "1") { WS_MB_Plantafel tmp = ws_navmbservice.Read(SelItem2.No); ws_navmbservice.Delete(SelItem2.Key); }
            if (s == "2") { WS_MB_Plantafel tmp = ws_navmbservice.Read(SelItem3.No); ws_navmbservice.Delete(SelItem3.Key); }
            if (s == "3") { WS_MB_Plantafel tmp = ws_navmbservice.Read(SelItem4.No); ws_navmbservice.Delete(SelItem4.Key); }
            if (s == "4") { WS_MB_Plantafel tmp = ws_navmbservice.Read(SelItem5.No); ws_navmbservice.Delete(SelItem5.Key); }

            doUpdate();
        }

        private void addMB(string s)
        {
            WS_MB_Plantafel tmp = new WS_MB_Plantafel();
            tmp.Arbeitsplatz = s;
            tmp.Datum = Arbeitsplatzdatum.ToLongDateString();
            tmp.Nachname = SelItem.Last_Name;
            tmp.Vorname = SelItem.First_Name;
            tmp.No = SelItem.No;
            ws_navmbservice.Create(ref tmp);
            doUpdate();
        }

        public DateTime Arbeitsplatzdatum { get { return arbeitsplatzdatum; } set { arbeitsplatzdatum = value; doUpdate(); } }

        public ObservableCollection<WS_MB_Plantafel> Colnavmb { get => _colnavmb; set => _colnavmb = value; }
        public ObservableCollection<ws_mitarbeiter> Colmb { get => _colmb; set => _colmb = value; }
        public ObservableCollection<WS_MB_Plantafel> ColAp1 { get { return _colAp1; } set { _colAp1 = value; RaisePropertyChanged("SelItem2"); } }
        public ObservableCollection<WS_MB_Plantafel> ColAp2 { get => _colAp2; set => _colAp2 = value; }
        public ObservableCollection<WS_MB_Plantafel> ColAp3 { get => _colAp3; set => _colAp3 = value; }
        public ObservableCollection<WS_MB_Plantafel> ColAp4 { get => _colAp4; set => _colAp4 = value; }
        public RelayCommand<string> AddRC { get => addRC; set => addRC = value; }
        public RelayCommand<string> RemoveRC { get => removeRC; set => removeRC = value; }
        public ws_mitarbeiter SelItem { get => _selItem; set => _selItem = value; }
        
        public WS_MB_Plantafel SelItem3 { get => _selItem3; set => _selItem3 = value; }
        public WS_MB_Plantafel SelItem4 { get => _selItem4; set => _selItem4 = value; }
        public WS_MB_Plantafel SelItem5 { get => _selItem5; set => _selItem5 = value; }
        public WS_MB_Plantafel SelItem2 { get { return _selItem2; } set { _selItem2 = value; RaisePropertyChanged("SelItem2"); } }

        ObservableCollection<WS_MB_Plantafel> _colAp1 = new ObservableCollection<WS_MB_Plantafel>();
        ObservableCollection<WS_MB_Plantafel> _colAp2 = new ObservableCollection<WS_MB_Plantafel>();
        ObservableCollection<WS_MB_Plantafel> _colAp3 = new ObservableCollection<WS_MB_Plantafel>();
        ObservableCollection<WS_MB_Plantafel> _colAp4 = new ObservableCollection<WS_MB_Plantafel>();

        RelayCommand<string> addRC;
        RelayCommand<string> removeRC;

        ws_mitarbeiter _selItem = new ws_mitarbeiter();
        WS_MB_Plantafel _selItem2 = new WS_MB_Plantafel();
        WS_MB_Plantafel _selItem3 = new WS_MB_Plantafel();
        WS_MB_Plantafel _selItem4 = new WS_MB_Plantafel();
        WS_MB_Plantafel _selItem5 = new WS_MB_Plantafel();

        private void doUpdate()
        {
            Colmb.Clear();
            Colnavmb.Clear();
            //Hier werden jetzt anhand des Datums alle "freien" MB in die Collection geschrieben
            ws_mitarbeiter[] list = ws_mbservice.ReadMultiple(null, null, 1000);
            foreach(ws_mitarbeiter x in list)
            {
                WS_MB_Plantafel y = ws_navmbservice.Read(x.No);
                if (y == null)
                {
                    bool check = false;
                    foreach (var z in Colmb) { if (z.No == x.No) { check = true; } }
                    if (check == false) { Colmb.Add(x); }
                }
                //check datum
                if (y != null)
                {
                    if (DateTime.Parse(y.Datum).Date != Arbeitsplatzdatum.Date)
                    {             
                        bool check1 = false;
                        foreach (var z in Colmb) { if (z.No == x.No) { check1 = true; } }
                        if (check1 == false) { Colmb.Add(x); }
                    }
                    else
                    {
                        bool check1 = false;
                        foreach (var z in Colnavmb) { if (z.No == y.No) { check1 = true; } }
                        if (check1 == false) { Colnavmb.Add(y); }
                        
                    }
                }

                //jetzt die Daten der heutigen zugewiesenen mb in die Collections schreiben
                ColAp1.Clear(); ColAp2.Clear(); ColAp3.Clear(); ColAp4.Clear();
                foreach(WS_MB_Plantafel mb in Colnavmb)
                {
                    if(mb.Arbeitsplatz == "1") { ColAp1.Add(mb); }
                    if (mb.Arbeitsplatz == "2") { ColAp2.Add(mb); }
                    if (mb.Arbeitsplatz == "3") { ColAp3.Add(mb); }
                    if (mb.Arbeitsplatz == "4") { ColAp4.Add(mb); }
                }
            }
        }
    }
}
