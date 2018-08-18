using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantafelNAV.ws_aufarbservice;
namespace PlantafelNAV.ViewModel
{

    public class APAuslastungVm:ViewModelBase
    {
        WS_Auf_Arb_Nav_Service ws_arbeitsplan = new WS_Auf_Arb_Nav_Service();
        


        public void readData()
        {
            WS_Auf_Arb_Nav[] list = ws_arbeitsplan.ReadMultiple(null, null, 1000);
            foreach (WS_Auf_Arb_Nav item in list)
            {
                //item.Ende =
            }

        }

    }
}
