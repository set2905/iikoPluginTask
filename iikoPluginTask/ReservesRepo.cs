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

    public void UpdateReserve(Reserve handledReserveation)
    {
        //IL_001d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0023: Invalid comparison between Unknown and I4
        //IL_015b: Unknown result type (might be due to invalid IL or missing references)
        //IL_016b: Unknown result type (might be due to invalid IL or missing references)
        //IL_0171: Invalid comparison between Unknown and I4
        //IL_017c: Unknown result type (might be due to invalid IL or missing references)
        //IL_0182: Invalid comparison between Unknown and I4
        IReserve reserveFromIiko = PluginContext.Operations.GetReserveById(handledReserveation.Id);
        if (reserveFromIiko.Order == null && (int)reserveFromIiko.Status != 2)
        {
            IEditSession editSession = PluginContext.Operations.CreateEditSession();
            ICredentials credentials = PluginContext.Operations.AuthenticateByPin("12344321");
            INewOrderStub order = editSession.CreateOrder(handledReserveation.Tables, (IUser)null);
            IOrderType orderType = (from t in PluginContext.Operations.GetOrderTypes()
                                    where (int)t.OrderServiceType == 1
                                    select t).FirstOrDefault();
            editSession.SetOrderType(orderType, (IOrderStub)(object)order);
            editSession.AddOrderGuest(handledReserveation.Client.Name, (IOrderStub)(object)order);
            editSession.ChangeEstimatedOrderGuestsCount(handledReserveation.GuestsCount, (IOrderStub)(object)order);
            editSession.BindReserveToOrder((IReserveStub)(object)PluginContext.Operations.GetReserveById(handledReserveation.Id), (IOrderStub)(object)order);
            PluginContext.Operations.SubmitChanges(credentials, editSession);
            return;
        }
        string Status = null;
        string ClosingTime = null;
        string StartTime = ((!handledReserveation.GuestsComingTime.HasValue) ? null : ((handledReserveation.GuestsComingTime.Value < DateTime.Now) ? handledReserveation.GuestsComingTime.Value.ToString("yyyy-MM-ddTHH:mm:sszzzz") : DateTime.Now.AddSeconds(5.0).ToString("yyyy-MM-ddTHH:mm:sszzzz")));
        if ((int)handledReserveation.Status == 0)
        {
            Status = "other";
        }
        else if ((int)handledReserveation.Status == 1)
        {
            Status = "come";
        }
        else if ((int)handledReserveation.Status == 2)
        {
            Status = "cancel";
        }
        string IsRemind = ((!handledReserveation.ShouldRemind) ? "0" : "1");
        _ = handledReserveation.Client.CardNumber;
        List<string> tableGuids = new List<string>();
        foreach (ITable _TableGuids in handledReserveation.Tables)
        {
            tableGuids.Add(((IEntity)_TableGuids).Id.ToString());
        }
        Guest guest = Helpers.GetGuestInfo(handledReserveation);
        ChangedReservationstoSend changedReservationstoSend = new ChangedReservationstoSend
        {
            guid = handledReserveation.Id.ToString(),
            date = handledReserveation.EstimatedStartTime.ToString("yyyy-MM-ddTHH:mm:sszzzz"),
            registerTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzzz"),
            comingTime = StartTime,
            closingTime = ClosingTime,
            guest = guest,
            duration = handledReserveation.Duration.TotalMinutes.ToString(),
            number = "1",
            type = "reserve",
            guestCount = handledReserveation.GuestsCount.ToString(),
            status = Status,
            comment = handledReserveation.Comment,
            tablesGUIDs = tableGuids,
            isRemind = IsRemind,
            cancelReason = ""
        };
        RpcRequestConstructor<ChangedReservationstoSend> ReserveChangedJson = new RpcRequestConstructor<ChangedReservationstoSend>("UpdateReserve", changedReservationstoSend);
        WebSocketClient.GetInstance().SendAndLog("UpdateReserve", ReserveChangedJson.ResultRequest);
    }
}
