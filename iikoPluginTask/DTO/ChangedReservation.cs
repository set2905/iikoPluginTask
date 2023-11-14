using System.Collections.Generic;

namespace iikoPluginTask.DTO
{
    public class ChangedReservation
    {
        public string Guid { get; set; }

        public string Date { get; set; }

        public string RegisterTime { get; set; }

        public string ComingTime { get; set; }

        public string ClosingTime { get; set; }

        public Guest Guest { get; set; }

        public string Duration { get; set; }

        public string Number { get; set; }

        public string Type { get; set; }

        public string GuestCount { get; set; }

        public string Status { get; set; }

        public string Comment { get; set; }

        public List<string> TablesGUIDs { get; set; }

        public string IsRemind { get; set; }

        public string CancelReason { get; set; }
    }
}
