using Bogus;
using iikoPluginTask.DTO;
using Resto.Front.Api.Data.Brd;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iikoPluginTask.Mocking
{
    public class ReservationFaker
    {
        private Faker<Reserve> reserveFaker;
        private Faker<ClientMock> clientFaker;
        private Faker<TableMock> tableFaker;
        public ReservationFaker()
        {
            tableFaker = new Faker<TableMock>()
                .RuleFor(t => t.Id, f => Guid.NewGuid())
                .RuleFor(t => t.Number, f => f.UniqueIndex);
            clientFaker = new Faker<ClientMock>()
                .RuleFor(c => c.Name, f => f.Name.FirstName())
                .RuleFor(c => c.Surname, f => f.Name.LastName())
                .RuleFor(c => c.Nick, f => f.Internet.UserName())
                .RuleFor(c => c.Comment, f => f.Lorem.Sentence())
                .RuleFor(c => c.CardNumber, f => f.Person.Phone)
                .RuleFor(c => c.InBlacklist, f => false)
                .RuleFor(c => c.BlacklistReason, f => "");
            reserveFaker=new Faker<Reserve>()
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.Client, f => clientFaker.Generate())
                .RuleFor(r => r.EstimatedStartTime, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(7)))
                .RuleFor(r => r.Comment, f => f.Lorem.Sentence())
                .RuleFor(r => r.Status, f => f.PickRandom<ReserveStatus>())
                .RuleFor(r => r.Tables, f => tableFaker.Generate(10).ToList())
                .RuleFor(r => r.GuestsCount, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Order, f => null)
                .RuleFor(r => r.Duration, f => TimeSpan.FromMinutes(f.Random.Number(60, 240)))
                .RuleFor(r => r.ShouldRemind, f => f.Random.Bool());
        }

        public List<Reserve> GetFakeReserves(int count)
        {
            return reserveFaker.Generate(count);
        }
    }
}
