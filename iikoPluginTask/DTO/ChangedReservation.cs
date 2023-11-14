using System.Collections.Generic;

namespace iikoPluginTask.DTO
{
    public class ChangedReservation
    {
        public string guid { get; set; }

        public string date { get; set; }

        public string registerTime { get; set; }

        public string comingTime { get; set; }

        public string closingTime { get; set; }

        public Guest guest { get; set; }

        public string duration { get; set; }

        public string number { get; set; }

        public string type { get; set; }

        public string guestCount { get; set; }

        public string status { get; set; }

        public string comment { get; set; }

        public List<string> tablesGUIDs { get; set; }

        public string isRemind { get; set; }

        public string cancelReason { get; set; }
    }
}
