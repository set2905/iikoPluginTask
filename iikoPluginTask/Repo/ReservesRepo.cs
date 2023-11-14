using Resto.Front.Api.Data.Brd;
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
using iikoPluginTask.Mocking;

internal class ReservesRepo
{
    private Dictionary<Guid, Reserve> reserveCollection;
    private PluginLogger logger;
    private ReservationFaker reservationFaker;

    public ReservesRepo()
    {
        logger = new PluginLogger();
        reserveCollection = new Dictionary<Guid, Reserve>();
        reservationFaker = new ReservationFaker();

        var fakeReservations = reservationFaker.GetFakeReserves(10);
        foreach (var fakeReservation in fakeReservations)
        {
            UpdateIikoReserve(fakeReservation);
            IReserve reserveFromIiko = PluginContext.Operations.GetReserveById(fakeReservation.Id);
            reserveCollection.Add(reserveFromIiko.Id, fakeReservation);
        }
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
        const string samplePin = "12344321";

        IReserve reserveFromIiko = PluginContext.Operations.GetReserveById(handledReservation.Id);
        if (reserveFromIiko!=null && reserveFromIiko.Order == null && reserveFromIiko.Status != ReserveStatus.Closed)
        {
            IEditSession editSession = PluginContext.Operations.CreateEditSession();
            ICredentials credentials = PluginContext.Operations.AuthenticateByPin(samplePin);
            INewOrderStub order = editSession.CreateOrder(handledReservation.Tables, null);
            IOrderType orderType = (from t in PluginContext.Operations.GetOrderTypes()
                                    where t.OrderServiceType == OrderServiceTypes.Common
                                    select t).FirstOrDefault();
            editSession.SetOrderType(orderType, order);
            editSession.AddOrderGuest(handledReservation.Client.Name, order);
            editSession.ChangeEstimatedOrderGuestsCount(handledReservation.GuestsCount, order);
            editSession.BindReserveToOrder(PluginContext.Operations.GetReserveById(handledReservation.Id), order);
            PluginContext.Operations.SubmitChanges(credentials, editSession);
            return;
        }
        string startTime;
        if (handledReservation.GuestsComingTime.Value < DateTime.Now)
        {
            startTime=(!handledReservation.GuestsComingTime.HasValue) ? null : (handledReservation.GuestsComingTime.Value.ToString(startTimeFormat));
        }
        else
        {
            startTime=(!handledReservation.GuestsComingTime.HasValue) ? null : (DateTime.Now.AddSeconds(5.0).ToString(startTimeFormat));
        }

        string status;
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
            default:
                status=null;
                break;
        }

        string isRemind = (!handledReservation.ShouldRemind) ? "0" : "1";
        List<string> tableGuids = handledReservation.Tables.ToList().ConvertAll(x => x.Id.ToString());
        Guest guest = new Guest(handledReservation);
        ChangedReservation changedReservationstoSend = new ChangedReservation(
            handledReservation,
            startTime,
            startTimeFormat,
            guest,
            status,
            tableGuids,
            isRemind);
        RpcRequestConstructor<ChangedReservation> ReserveChangedJson = new RpcRequestConstructor<ChangedReservation>("UpdateIikoReserve", changedReservationstoSend);
        WebSocketClient.GetInstance().SendAndLog("UpdateIikoReserve", ReserveChangedJson.ResultRequest);
    }
}
