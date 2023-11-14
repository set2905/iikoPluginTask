using Resto.Front.Api.Data.Brd;
using Resto.Front.Api.Data.Orders;
using Resto.Front.Api.Data.Organization.Sections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iikoPluginTask.DTO
{
    public class Reserve : IEquatable<IReserve>
    {
        public Reserve()
        {
        }

        public Reserve(IReserve reserveFromIiko)
        {
            Id = reserveFromIiko.Id;
            Client = reserveFromIiko.Client;
            GuestsComingTime = reserveFromIiko.GuestsComingTime;
            EstimatedStartTime = reserveFromIiko.EstimatedStartTime;
            Comment = reserveFromIiko.Comment;
            Status = reserveFromIiko.Status;
            Tables = reserveFromIiko.Tables;
            GuestsCount = reserveFromIiko.GuestsCount;
            Duration = reserveFromIiko.Duration;
            ShouldRemind = reserveFromIiko.ShouldRemind;
            Order = reserveFromIiko.Order;
        }
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

        public bool Equals(IReserve other)
        {
            return other.Client.Id.Equals(this.Client.Id)
                && other.Client.Name.Equals(this.Client.Name)
                && other.Client.Phones.SequenceEqual(Client.Phones)
                && other.EstimatedStartTime.Equals(this.EstimatedStartTime)
                && other.Comment.Equals(this.Comment)
                && other.Status == this.Status
                && other.Tables.SequenceEqual(Tables)
                && other.GuestsCount.Equals(this.GuestsCount)
                && other.Duration.Equals(this.Duration)
                && other.ShouldRemind.Equals(this.ShouldRemind);
        }
    }
}
