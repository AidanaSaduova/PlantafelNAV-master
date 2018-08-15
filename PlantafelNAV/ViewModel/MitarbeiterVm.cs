using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantafelNAV.ws_mitarbeiterservice;
using System.Collections.ObjectModel;

namespace PlantafelNAV.ViewModel
{
    public class MitarbeiterVm:ViewModelBase
    {
        ws_mitarbeiter_Service ws_service_mb = new ws_mitarbeiter_Service();
       

        ObservableCollection<ws_mitarbeiter> _mitarbeiter = new ObservableCollection<ws_mitarbeiter>();
        ws_mitarbeiter _selUser = new ws_mitarbeiter();

        public MitarbeiterVm()
        {
            ws_service_mb.UseDefaultCredentials = true;
            loadMitarbeiter();

        }

        private void loadMitarbeiter()
        {
            Mitarbeiter.Clear();
            ws_mitarbeiter[] list = ws_service_mb.ReadMultiple(null, null, 100);

            foreach (ws_mitarbeiter x in list)
            {
                ws_mitarbeiter tmp = new ws_mitarbeiter();
                tmp.Last_Name = x.Last_Name;
                tmp.First_Name = x.First_Name;
                Mitarbeiter.Add(tmp);
            }

        }

        public ObservableCollection<ws_mitarbeiter> Mitarbeiter { get => _mitarbeiter; set => _mitarbeiter = value; }
        public ws_mitarbeiter SelUser { get => _selUser; set => _selUser = value; }
    }
}
