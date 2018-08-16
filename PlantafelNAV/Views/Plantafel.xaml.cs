using PlantafelNAV.TimelineNAV;
using PlantafelNAV.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        //timelines erstellen
        Timeline t1 = new Timeline(1000, 80);
        Timeline t2 = new Timeline(1000, 80);
        Timeline t3 = new Timeline(1000, 80);
        Timeline t4 = new Timeline(1000, 80);



        ObservableCollection<TMElement> _tmelements = new ObservableCollection<TMElement>();
        private decimal dlfz1;
        private decimal dlfz2;
        private decimal dlfz3;
        private decimal dlfz4;

        public ObservableCollection<TMElement> Tmelements { get => _tmelements; set => _tmelements = value; }
        public decimal Dlfz1 { get => dlfz1; set => dlfz1 = value; }
        public decimal Dlfz2 { get => dlfz2; set => dlfz2 = value; }
        public decimal Dlfz3 { get => dlfz3; set => dlfz3 = value; }
        public decimal Dlfz4 { get => dlfz4; set => dlfz4 = value; }
        public ObservableCollection<TMElement> Prods { get => _prods; set => _prods = value; }

        ObservableCollection<TMElement> _prods = new ObservableCollection<TMElement>();

        public Plantafel()
        {
            //Da wir kommunikationsfreudig sind, wird zuerst der Datacontext per Code geladen (notwendig für PlantafelVM DataContext)
            DataContext = new PlantafelNAV.ViewModel.PlantafelVm();
            PlantafelNAV.ViewModel.PlantafelVm planVM = this.DataContext as PlantafelNAV.ViewModel.PlantafelVm;
            InitializeComponent();


            //jetzt werden einmal die 4 timelines erzeugt
            t1.Setup(28800, 57600, 3600, 120);
            t2.Setup(28800, 57600, 3600, 120);
            t3.Setup(28800, 57600, 3600, 120);
            t4.Setup(28800, 57600, 3600, 120);
            host1.Children.Add(t1);
            host2.Children.Add(t2);
            host3.Children.Add(t3);
            host4.Children.Add(t4);

            //jetzt brauchen wir das Datum
            DateTime prod_date = planVM.Prod_date;

            //Eventhandler der die Timelines leert
            datePicker1.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(reloadTimelines);

            //TimelineEntrys erzeugen
            doTheTimelineStuff();
        }

        

        private void reloadTimelines(object sender, SelectionChangedEventArgs e)
        {


            doTheTimelineStuff();
        }


        private void addTimelineEntry(Timeline tl, int Starttime, int Duration, string Id)
        {

            tl.AddElement(Starttime, Duration, Id);
        }


        private void msgbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void doTheTimelineStuff()
        {
           
            t1.clearCanvas(); 
            t2.clearCanvas(); 
            t3.clearCanvas(); 
            t4.clearCanvas();

            //remove all elements
            t1.TElements1.Clear();
            t2.TElements1.Clear();
            t3.TElements1.Clear();
            t4.TElements1.Clear();





            PlantafelNAV.ViewModel.PlantafelVm planVM = this.DataContext as PlantafelNAV.ViewModel.PlantafelVm;
            DateTime prod_date = planVM.Prod_date;
            //Aufträge laden
            planVM.loadProduction();
            Tmelements.Clear();
            foreach (var x in planVM.Production)
            {
                TMElement tmp = new TMElement();
                tmp.Id = x.No;
                tmp.Quantity = x.Quantity;
                tmp.StartDate = x.Starting_Date;
                tmp.StartTime = x.Starting_Time;
                tmp.EndDate = x.Ending_Date;
                tmp.EndTime = x.Ending_Time;
                tmp.Key = x.Key;
                Tmelements.Add(tmp);
            }

            //Arbeitsplätze und Zeiten laden 
            planVM.loadArbeitsplatz();

            //evtl nicht mehr vorhandene Aufträge aus Tabelle löschen

            //++++++++++++++++++++//

            //jetzt für jede Produktionsplanung die entsprechenden Timelinelemente hinzufügen
            //dafür müssen zuerst aus unserer eigenen Tabelle alle verfügbaren Aufträge um die bereits vorhandenen Daten in der Tabelle ergänzt werden

            foreach (TMElement x in Tmelements)
            {
                //einmal, falls vorhanden, tabelleneintrag holen und Auftrag ergänzen
                if (planVM.getSpecAuftrag(x.Id) != null)
                {
                    Prods.Clear();
                    Prods = planVM.getProdOfSpecDate(prod_date);

                    // Hier gehts weiter mit dem Erzeugen der TimelineElements - bisheriges Problem das nur Auftragsnummer in Tabelle hineingeschrieben wird durch strings statt datetimes gelöst
                    Debug.WriteLine("Anzahl: " + Prods.Count);
                    foreach (TMElement y in Prods)
                    {
                        
                        //alle zeiten müssen in sekunden übergeben werden
                        //timeline, Anfangszeit in Sekunden, Dauer in Sekunden, ID
                        int begin = (y.StartDate.Hour * 60 * 60) + (y.StartDate.Minute * 60);
                        int duration = (y.EndDate.Hour - y.StartDate.Hour) * 60 + (y.EndDate.Minute - y.StartDate.Minute);
                        int end = duration * 60;
                        if (y.TimeLineNumber == 1) { addTimelineEntry(t1, begin, end, y.Id); }
                        if (y.TimeLineNumber == 2) { addTimelineEntry(t2, begin, end, y.Id); }
                        if (y.TimeLineNumber == 3) { addTimelineEntry(t3, begin, end, y.Id); }
                        if (y.TimeLineNumber == 4) { addTimelineEntry(t4, begin, end, y.Id);  }
                        //addTimelineEntry(t1, 8*60*60, , y.Id );
                    }
                }
                else
                {
                    //auftrag "berechnen", in Tabelle schreiben, in Nav aktualisieren & dann TM-Elemente erzeugen
                    //wir nehmen an, dass ein Auftrag nicht länger als 8 stunden ist ... todo
                    //DLFZeiten
                    //achtung bei quantities unter 10 stimmt die Zeit nicht, da immer mit 10 gearbeitet wird ... ein weiters todo
                    decimal ApDLFZ1 = planVM.calculateRuntime(1, x.Quantity);
                    decimal ApDLFZ2 = planVM.calculateRuntime(2, x.Quantity);
                    decimal ApDLFZ3 = planVM.calculateRuntime(3, x.Quantity);
                    decimal ApDLFZ4 = planVM.calculateRuntime(4, x.Quantity);

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

                    DateTime start = new DateTime(x.StartDate.Year, x.StartDate.Month, x.StartDate.Day, x.StartTime.Hour, x.StartTime.Minute, x.StartTime.Second);
                    ApStart1 = start;
                    ApEnd1 = start.AddMinutes((double)ApDLFZ1);
                    //Überprüfen ob sich der zweite noch ausgeht//
                    tmp = ApEnd1.AddMinutes((double)ApDLFZ2);
                    tmp1 = new DateTime(x.StartDate.Year, x.StartDate.Month, x.StartDate.Day, 16, 00, 00);
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
                    // Debug.WriteLine(ApStart1 + "/" + ApEnd1 + ApStart2 + "/" + ApEnd2 + ApStart3 + "/" + ApEnd3 + ApStart4+ "/" + ApEnd4 );

                    //jetzt Werte in Tabelle schreiben
                    planVM.newProdToTable(x.Id, ApStart1, ApEnd1, ApStart1, ApStart2, ApStart3, ApStart4, ApEnd1, ApEnd2, ApEnd3, ApEnd4);

                    //jetzt Auftrag in Nav aktualisieren
                    planVM.doUpdateProdNav(x.Id, ApEnd4);
                    //jetzt Elemente zur Timeline hinzufügen
                    ObservableCollection<TMElement> prods = planVM.getProdOfSpecDate(prod_date);

                    // Hier gehts weiter mit dem Erzeugen der TimelineElements - bisheriges Problem das nur Auftragsnummer in Tabelle hineingeschrieben wird durch strings statt datetimes gelöst
                    foreach (TMElement y in prods)
                    {
                        //alle zeiten müssen in sekunden übergeben werden
                        //timeline, Anfangszeit in Sekunden, Dauer in Sekunden, ID
                        int begin = (y.StartDate.Hour * 60 * 60) + (y.StartDate.Minute * 60);
                        int duration = (y.EndDate.Hour - y.StartDate.Hour) * 60 + (y.EndDate.Minute - y.StartDate.Minute);
                        int end = duration * 60;
                        if (y.TimeLineNumber == 1) { addTimelineEntry(t1, begin, end, y.Id); }
                        if (y.TimeLineNumber == 2) { addTimelineEntry(t2, begin, end, y.Id); }
                        if (y.TimeLineNumber == 3) { addTimelineEntry(t3, begin, end, y.Id); }
                        if (y.TimeLineNumber == 4) { addTimelineEntry(t4, begin, end, y.Id); }
                        //addTimelineEntry(t1, 8*60*60, , y.Id );
                    }
                }
            }

        }
       


    }
}