using System;
using System.Collections.Generic;

namespace iikoPluginTask.DTO
{
    public class ChangedReservation
    {
        public ChangedReservation()
        {
        }

        public ChangedReservation(Reserve handledReservation,
            string startTime,
            string startTimeFormat,
            Guest guest,
            string status,
            List<string> tableGuids,
            string isRemind)
        {
            Guid = handledReservation.Id.ToString();
            Date = handledReservation.EstimatedStartTime.ToString(startTimeFormat);
            RegisterTime = DateTime.Now.ToString(startTimeFormat);
            ComingTime = startTime;
            ClosingTime = null;
            Guest = guest;
            Duration = handledReservation.Duration.TotalMinutes.ToString();
            Number = "1";
            Type = "reserve";
            GuestCount = handledReservation.GuestsCount.ToString();
            Status = status;
            Comment = handledReservation.Comment;
            TablesGUIDs = tableGuids;
            IsRemind = isRemind;
            CancelReason = "";
        }

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
