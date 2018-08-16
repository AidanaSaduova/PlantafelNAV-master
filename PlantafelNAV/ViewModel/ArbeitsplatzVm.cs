using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantafelNAV.ws_arbeitsplatzservice;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;

namespace PlantafelNAV.ViewModel
{
    public class ArbeitsplatzVm:ViewModelBase
    {
        WS_Arbeitzplatz_Service ws_serviceap = new WS_Arbeitzplatz_Service();
        ObservableCollection<WS_Arbeitzplatz> _arbeitsplaetze = new ObservableCollection<WS_Arbeitzplatz>();
        private WS_Arbeitzplatz _selitem = new WS_Arbeitzplatz();
        private RelayCommand doUpdate;
        public ObservableCollection<WS_Arbeitzplatz> Arbeitsplaetze { get => _arbeitsplaetze; set => _arbeitsplaetze = value; }
        public WS_Arbeitzplatz Selitem
        {
            get { return _selitem; }
            set { _selitem = value; RaisePropertyChanged(); }
        }

        public RelayCommand DoUpdate
        {
            get { return doUpdate; }
            set { doUpdate = value; }
        }

        public ArbeitsplatzVm()
        {
            
            DoUpdate = new RelayCommand(doUpdateMeth);
            ws_serviceap.UseDefaultCredentials = true;
            loadArbeitsplatz();
        }

        private void doUpdateMeth()
        {
            ws_serviceap.Update(ref _selitem);
            loadArbeitsplatz();
        }

        private void loadArbeitsplatz()
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
