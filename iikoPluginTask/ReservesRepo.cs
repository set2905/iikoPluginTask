using Resto.Front.Api.Data.Brd;
using Resto.Front.Api.Data.Organization.Sections;
using Resto.Front.Api;
using System.Linq;
using System.Collections.Generic;
using System;
using iikoPluginTask.DTO;
using Resto.Front.Api.Data.Organization;
using Resto.Front.Api.Data.Security;
using Resto.Front.Api.Editors.Stubs;
using Resto.Front.Api.Editors;
using iikoPluginTask.DTO.Constructor;
using iikoPluginTask.Logging;

internal class ReservesRepo
{
    private Dictionary<Guid, Reserve> reserveCollection;
    private PluginLogger logger;

    public ReservesRepo()
    {
        logger = new PluginLogger();
        reserveCollection = new Dictionary<Guid, Reserve>();
    }

    /// <summary>
    /// Создает или редактирует бронирование в iiko, в зависимости от наличия <paramref name="reserveFromIiko"/> в коллекции бронирований плагина
    /// </summary>
    /// <param name="reserveFromIiko"></param>
    public void AddOrUpdate(IReserve reserveFromIiko)
    {
        if (reserveCollection.TryGetValue(reserveFromIiko.Id, out var reserveFromCollection))
        {
            if (reserveFromCollection.Status == ReserveStatus.Closed)
            {
                reserveCollection.Remove(reserveFromCollection.Id);
            }
            if (reserveFromCollection.Order != null)
            {
                reserveFromCollection.Order.Id.ToString();
            }
            if (!reserveFromCollection.Equals(reserveFromIiko))
            {
                logger.LogReserve(reserveFromIiko);
                Reserve reserve = new Reserve(reserveFromIiko);
                UpdateIikoReserve(reserve);
                reserveCollection.Remove(reserveFromIiko.Id);
                reserveCollection.Add(reserveFromIiko.Id, reserve);
            }
        }
        else
        {
            Reserve reserve = new Reserve(reserveFromIiko);
            reserveCollection.Add(reserveFromIiko.Id, reserve);
            UpdateIikoReserve(reserve);
        }
    }

    /// <summary>
    /// Уведомляет iiko о изменении <paramref name="handledReservation"/> через WebSocket
    /// </summary>
    /// <param name="handledReservation"></param>
    private void UpdateIikoReserve(Reserve handledReservation)
    {
        const string startTimeFormat = "yyyy-MM-ddTHH:mm:sszzzz";
        IReserve reserveFromIiko = PluginContext.Operations.GetReserveById(handledReservation.Id);
        if (reserveFromIiko.Order == null && reserveFromIiko.Status != ReserveStatus.Closed)
        {
            IEditSession editSession = PluginContext.Operations.CreateEditSession();
            ICredentials credentials = PluginContext.Operations.AuthenticateByPin("12344321");
            INewOrderStub order = editSession.CreateOrder(handledReservation.Tables, null);
            IOrderType orderType = (from t in PluginContext.Operations.GetOrderTypes()
                                    where (int)t.OrderServiceType == 1
                                    select t).FirstOrDefault();
            editSession.SetOrderType(orderType, (IOrderStub)(object)order);
            editSession.AddOrderGuest(handledReservation.Client.Name, (IOrderStub)(object)order);
            editSession.ChangeEstimatedOrderGuestsCount(handledReservation.GuestsCount, (IOrderStub)(object)order);
            editSession.BindReserveToOrder(PluginContext.Operations.GetReserveById(handledReservation.Id), order);
            PluginContext.Operations.SubmitChanges(credentials, editSession);
            return;
        }
        string status = null;
        string startTime;
        if (handledReservation.GuestsComingTime.Value < DateTime.Now)
        {
            startTime=(!handledReservation.GuestsComingTime.HasValue) ? null : (handledReservation.GuestsComingTime.Value.ToString(startTimeFormat));
        }
        else
        {
            startTime=(!handledReservation.GuestsComingTime.HasValue) ? null : (DateTime.Now.AddSeconds(5.0).ToString(startTimeFormat));
        }

        switch (handledReservation.Status)
        {
            case ReserveStatus.New:
                status = "other";
                break;
            case ReserveStatus.Started:
                status = "come";
                break;
            case ReserveStatus.Closed:
                status = "cancel";
                break;
        }

        string isRemind = (!handledReservation.ShouldRemind) ? "0" : "1";
        List<string> tableGuids = new List<string>();
        foreach (ITable table in handledReservation.Tables)
            tableGuids.Add(table.Id.ToString());

        Guest guest = new Guest(handledReservation);
        ChangedReservation changedReservationstoSend = new ChangedReservation
        {
            Guid = handledReservation.Id.ToString(),
            Date = handledReservation.EstimatedStartTime.ToString(startTimeFormat),
            RegisterTime = DateTime.Now.ToString(startTimeFormat),
            ComingTime = startTime,
            ClosingTime = null,
            Guest = guest,
            Duration = handledReservation.Duration.TotalMinutes.ToString(),
            Number = "1",
            Type = "reserve",
            GuestCount = handledReservation.GuestsCount.ToString(),
            Status = status,
            Comment = handledReservation.Comment,
            TablesGUIDs = tableGuids,
            IsRemind = isRemind,
            CancelReason = ""
        };
        RpcRequestConstructor<ChangedReservation> ReserveChangedJson = new RpcRequestConstructor<ChangedReservation>("UpdateIikoReserve", changedReservationstoSend);
        WebSocketClient.GetInstance().SendAndLog("UpdateIikoReserve", ReserveChangedJson.ResultRequest);
    }
}
