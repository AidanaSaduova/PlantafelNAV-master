using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantafelNAV.ViewModel.Helpers
{
    public class Mitarbeiter
    {
        public Mitarbeiter(int iD, string vorname, string nachname, string status)
        {
            ID = iD;
            Vorname = vorname;
            Nachname = nachname;
            Status = status;
        }

        public int ID { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Status { get; set; }

    }
}
