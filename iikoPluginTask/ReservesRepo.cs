using Resto.Front.Api.Data.Brd;
using Resto.Front.Api.Data.Common;
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

internal class ReservesRepo
{
    public Dictionary<Guid, Reserve> ReserveCollection { get; set; }
    public void AddOrUpdate(IReserve reserveFromIiko)
    {
        if (ReserveCollection.TryGetValue(reserveFromIiko.Id, out var reserveFromCollection))
        {
            if (reserveFromIiko.Order != null)
            {
                (reserveFromIiko.Order).Id.ToString();
            }
            if ((int)reserveFromCollection.Status == 2)
            {
                ReserveCollection.Remove(reserveFromCollection.Id);
            }
            if (reserveFromCollection.Order != null)
            {
                reserveFromCollection.Order.Id.ToString();
            }
            bool isOrderIdEqual = false;
            if (reserveFromIiko.Order != null && reserveFromCollection.Order != null)
            {
                isOrderIdEqual = reserveFromIiko.Order.Id.Equals(reserveFromCollection.Order.Id);
            }
            if (!reserveFromIiko.Client.Id.Equals(reserveFromCollection.Client.Id) || !reserveFromIiko.Client.Name.Equals(reserveFromCollection.Client.Name) || !ComparePhones(reserveFromCollection.Client.Phones.Select((IPhone p) => p.Value).ToList(), reserveFromIiko.Client.Phones.Select((IPhone p) => p.Value).ToList()) || !reserveFromIiko.EstimatedStartTime.Equals(reserveFromCollection.EstimatedStartTime) || !reserveFromIiko.Comment.Equals(reserveFromCollection.Comment) || reserveFromIiko.Status != reserveFromCollection.Status || !CompareTables(reserveFromCollection.Tables.Select((ITable t) => ((IEntity)t).Id).ToList(), reserveFromIiko.Tables.Select((ITable t) => ((IEntity)t).Id).ToList()) || !reserveFromIiko.GuestsCount.Equals(reserveFromCollection.GuestsCount) || !reserveFromIiko.Duration.Equals(reserveFromCollection.Duration) || !reserveFromIiko.ShouldRemind.Equals(reserveFromCollection.ShouldRemind && isOrderIdEqual))
            {
                ILog log = PluginContext.Log;
                ReserveStatus status = reserveFromIiko.Status;
                log.Info("--------------------------------------------------------------------------------------------");
                log.Info($"Client Id:          {reserveFromIiko.Client.Id.Equals(reserveFromCollection.Client.Id)}");
                log.Info($"Client Name:        {reserveFromIiko.Client.Name.Equals(reserveFromCollection.Client.Name)}");
                log.Info($"Client Phones:      {reserveFromIiko.Client.Phones.Select((IPhone p) => p.Value).ToList().Equals(reserveFromCollection.Client.Phones.Select((IPhone p) => p.Value).ToList())}");
                log.Info($"EstimatedStartTime: {reserveFromIiko.EstimatedStartTime.Equals(reserveFromCollection.EstimatedStartTime)}");
                log.Info($"Comment:            {reserveFromIiko.Comment.Equals(reserveFromCollection.Comment)}");
                log.Info($"Status:             {status.Equals((object)reserveFromCollection.Status)}");
                log.Info($"Tables:             {CompareTables(reserveFromCollection.Tables.Select((ITable t) => t.Id).ToList(), reserveFromIiko.Tables.Select((ITable t) => t.Id).ToList())}");
                log.Info($"GuestsCount:        {reserveFromIiko.GuestsCount.Equals(reserveFromCollection.GuestsCount)}");
                log.Info($"Duration:           {reserveFromIiko.Duration.Equals(reserveFromCollection.Duration)}");
                log.Info($"ShouldRemind:       {reserveFromIiko.ShouldRemind.Equals(reserveFromCollection.ShouldRemind)}");
                log.Info($"Order Id:           {isOrderIdEqual}");
                log.Info($"Curr reserve start time: {reserveFromCollection.EstimatedStartTime} | New reserve start time: {reserveFromIiko.EstimatedStartTime}");
                log.Info("--------------------------------------------------------------------------------------------");
                Reserve reserve2 = new Reserve
                {
                    Id = reserveFromIiko.Id,
                    Client = reserveFromIiko.Client,
                    GuestsComingTime = reserveFromIiko.GuestsComingTime,
                    EstimatedStartTime = reserveFromIiko.EstimatedStartTime,
                    Comment = reserveFromIiko.Comment,
                    Status = reserveFromIiko.Status,
                    Tables = reserveFromIiko.Tables,
                    GuestsCount = reserveFromIiko.GuestsCount,
                    Duration = reserveFromIiko.Duration,
                    ShouldRemind = reserveFromIiko.ShouldRemind,
                    Order = reserveFromIiko.Order
                };
                UpdateReserve(reserve2);
                ReserveCollection.Remove(((IEntity)reserveFromIiko).Id);
                ReserveCollection.Add(((IEntity)reserveFromIiko).Id, reserve2);
            }
        }
        else
        {
            Reserve reserve = new Reserve
            {
                Id = reserveFromIiko.Id,
                Client = reserveFromIiko.Client,
                GuestsComingTime = reserveFromIiko.GuestsComingTime,
                EstimatedStartTime = reserveFromIiko.EstimatedStartTime,
                Comment = reserveFromIiko.Comment,
                Status = reserveFromIiko.Status,
                Tables = reserveFromIiko.Tables,
                GuestsCount = reserveFromIiko.GuestsCount,
                Duration = reserveFromIiko.Duration,
                ShouldRemind = reserveFromIiko.ShouldRemind,
                Order = reserveFromIiko.Order
            };
            ReserveCollection.Add(reserveFromIiko.Id, reserve);
            UpdateReserve(reserve);
        }
    }
    private bool CompareTables(List<Guid> oldTables, List<Guid> newTables)
    {
        bool result = true;
        if (oldTables.Count == newTables.Count)
        {
            foreach (Guid ot in oldTables)
            {
                if (!newTables.Contains(ot))
                {
                    result = false;
                    return result;
                }
            }
        }
        else
        {
            result = false;
        }
        return result;
    }

    private bool ComparePhones(List<string> oldPhones, List<string> newPhones)
    {
        bool result = true;
        if (oldPhones.Count == newPhones.Count)
        {
            foreach (string op in oldPhones)
            {
                if (!newPhones.Contains(op))
                {
                    result = false;
                    return result;
                }
            }
        }
        else
        {
            result = false;
        }
        return result;
    }

    public void UpdateReserve(Reserve handledReservation)
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
            editSession.BindReserveToOrder((IReserveStub)(object)PluginContext.Operations.GetReserveById(handledReservation.Id), (IOrderStub)(object)order);
            PluginContext.Operations.SubmitChanges(credentials, editSession);
            return;
        }
        string Status = null;
        string ClosingTime = null;
        string StartTime = ((!handledReservation.GuestsComingTime.HasValue) ? null : ((handledReservation.GuestsComingTime.Value < DateTime.Now) ? handledReservation.GuestsComingTime.Value.ToString(startTimeFormat) : DateTime.Now.AddSeconds(5.0).ToString(startTimeFormat)));
        if ((int)handledReservation.Status == 0)
        {
            Status = "other";
        }
        else if ((int)handledReservation.Status == 1)
        {
            Status = "come";
        }
        else if ((int)handledReservation.Status == 2)
        {
            Status = "cancel";
        }
        string IsRemind = ((!handledReservation.ShouldRemind) ? "0" : "1");
        _ = handledReservation.Client.CardNumber;
        List<string> tableGuids = new List<string>();
        foreach (ITable _TableGuids in handledReservation.Tables)
        {
            tableGuids.Add(((IEntity)_TableGuids).Id.ToString());
        }
        Guest guest = new Guest(handledReservation);
        ChangedReservation changedReservationstoSend = new ChangedReservation
        {
            Guid = handledReservation.Id.ToString(),
            Date = handledReservation.EstimatedStartTime.ToString(startTimeFormat),
            RegisterTime = DateTime.Now.ToString(startTimeFormat),
            ComingTime = StartTime,
            ClosingTime = ClosingTime,
            Guest = guest,
            Duration = handledReservation.Duration.TotalMinutes.ToString(),
            Number = "1",
            Type = "reserve",
            GuestCount = handledReservation.GuestsCount.ToString(),
            Status = Status,
            Comment = handledReservation.Comment,
            TablesGUIDs = tableGuids,
            IsRemind = IsRemind,
            CancelReason = ""
        };
        RpcRequestConstructor<ChangedReservation> ReserveChangedJson = new RpcRequestConstructor<ChangedReservation>("UpdateReserve", changedReservationstoSend);
        WebSocketClient.GetInstance().SendAndLog("UpdateReserve", ReserveChangedJson.ResultRequest);
    }
}
