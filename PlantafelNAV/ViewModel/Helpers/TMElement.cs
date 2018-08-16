using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantafelNAV.ViewModel.Helpers
{

    

    public class TMElement
    {
        private int timeLineNumber;
        private DateTime startTime;
        private DateTime endTime;
        private DateTime startDate;
        private DateTime endDate;
        private string id;
        private decimal quantity;
        private string key;

        public int TimeLineNumber { get => timeLineNumber; set => timeLineNumber = value; }
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }
        public string Id { get => id; set => id = value; }
        public decimal Quantity { get => quantity; set => quantity = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public string Key { get => key; set => key = value; }
    }
}
