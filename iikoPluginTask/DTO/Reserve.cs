using Resto.Front.Api.Data.Brd;
using Resto.Front.Api.Data.Orders;
using Resto.Front.Api.Data.Organization.Sections;
using System;
using System.Collections.Generic;

namespace iikoPluginTask.DTO
{
    public class Reserve
    {
        public Guid Id { get; set; }

        public IClient Client { get; set; }

        public DateTime? GuestsComingTime { get; set; }

        public DateTime EstimatedStartTime { get; set; }

        public string Comment { get; set; }

        public ReserveStatus Status { get; set; }

        public IReadOnlyList<ITable> Tables { get; set; }

        public int GuestsCount { get; set; }

        public IOrder Order { get; set; }

        public TimeSpan Duration { get; set; }

        public bool ShouldRemind { get; set; }
    }
}
