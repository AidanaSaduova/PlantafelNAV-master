using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantafelNAV.ViewModel.Helpers
{
    public class AP_Auslastung
    {

        public AP_Auslastung()
        {

        }

        public AP_Auslastung(Int16 dur, string Name)
        {
            ApName = Name;
            Duration = dur;
        }

        string _apName;
        Int16 _duration;

        public string ApName { get => _apName; set => _apName = value; }
        public short Duration { get => _duration; set => _duration = value; }
    }
}
