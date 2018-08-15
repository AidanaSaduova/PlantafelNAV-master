using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantafelNAV.ViewModel.Helpers
{
    public class Auftrag
    {
        public Auftrag(int iD, DateTime beginn, DateTime end, Maschine schmelzofen, Maschine cNCDrehmaschine, Maschine ausrichtestation, Maschine fraesmaschine, Maschine pruefmaschine, string status)
        {
            ID = iD;
            Beginn = beginn;
            End = end;
            Schmelzofen = schmelzofen;
            CNCDrehmaschine = cNCDrehmaschine;
            Ausrichtestation = ausrichtestation;
            Fraesmaschine = fraesmaschine;
            Pruefmaschine = pruefmaschine;
            Status = status;
        }

        public int ID { get; set; }
        public DateTime Beginn { get; set; }
        public DateTime End { get; set; }
        public Maschine Schmelzofen { get; set; }
        public Maschine CNCDrehmaschine { get; set; }
        public Maschine Ausrichtestation { get; set; }
        public Maschine Fraesmaschine { get; set; }
        public Maschine Pruefmaschine { get; set; }
        public string Status { get; set; }

    }
}
