using Resto.Front.Api;
using Resto.Front.Api.Data.Brd;
using Resto.Front.Api.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iikoPluginTask
{
    public class ReservePlugin : IFrontPlugin
    {
        private Dictionary<Guid, DateTime> ReserveTimes { get; set; }
        public ReservePlugin()
        {
            PluginContext.Notifications.ReserveChanged.Subscribe(OnReserveChanged);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void OnReserveChanged(EntityChangedEventArgs<IReserve> reserve)
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
                        _partialReserveNotification.AddOrUpdate(reserve.Entity);
                    }
                    if (!reserve.Entity.GuestsComingTime.HasValue || !(Math.Abs((reserve.Entity.GuestsComingTime.Value - DateTime.Now).TotalSeconds) < 2.5))
                    {
                        if (reserve.Entity.Order != null)
                        {
                            _reservedOrders.Add(((IEntity)reserve.Entity.Order).Id);
                        }
                        ReservesWithCreationTime.AddOrUpdate(((IEntity)reserve.Entity).Id, DateTime.Now);
                        _partialReserveNotification.AddOrUpdate(reserve.Entity);
                    }
                }
                else if ((int)reserve.Entity.Status != 2)
                {
                    ReservesWithCreationTime.AddOrUpdate(((IEntity)reserve.Entity).Id, DateTime.Now);
                    _partialReserveNotification.AddOrUpdate(reserve.Entity);
                    _timeLastReserve = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                PluginContext.Log.Error($"{ex}");
            }
        }
    }
}
