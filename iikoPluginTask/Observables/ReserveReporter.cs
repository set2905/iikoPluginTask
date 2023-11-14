using iikoPluginTask.DTO.Constructor;
using Resto.Front.Api;
using Resto.Front.Api.Data.Brd;
using Resto.Front.Api.Data.Common;
using System;
using System.Collections.Generic;

namespace iikoPluginTask.Observables
{
    public class ReserveReporter : IObserver<EntityChangedEventArgs<IReserve>>
    {
        private Dictionary<Guid, DateTime> ReserveTimes { get; set; }
        private ReservesRepo reservesRepo { get; set; }

        public ReserveReporter()
        {
            reservesRepo=new ReservesRepo();
            ReserveTimes = new Dictionary<Guid, DateTime>();
        }

        public void OnNext(EntityChangedEventArgs<IReserve> value)
        {
            HandleReserveChanged(value);
        }

        private void HandleReserveChanged(EntityChangedEventArgs<IReserve> reserve)
        {
            try
            {
                if (reserve.EventType == EntityEventType.Removed)
                {
                    return;
                }
                if (ReserveTimes.ContainsKey(reserve.Entity.Id))
                {
                    DateTime creationTime = ReserveTimes[(reserve.Entity).Id];
                    if ((DateTime.Now - creationTime).Seconds > 3)
                    {
                        reservesRepo.AddOrUpdate(reserve.Entity);
                    }
                    if (!reserve.Entity.GuestsComingTime.HasValue || !(Math.Abs((reserve.Entity.GuestsComingTime.Value - DateTime.Now).TotalSeconds) < 2.5))
                    {
                        reservesRepo.AddOrUpdate(reserve.Entity);
                    }
                }
                else if ((int)reserve.Entity.Status != 2)
                {
                    reservesRepo.AddOrUpdate(reserve.Entity);
                }
            }
            catch (Exception ex)
            {
                PluginContext.Log.Error($"{ex.Message}");
            }
        }
        public void OnCompleted()
        {
            PluginContext.Log.Info("Reserve Reporter completed reporting!");
        }

        public void OnError(Exception error)
        {
            PluginContext.Log.Error($"{error.Message}");
        }
    }
}
