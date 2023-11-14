using Resto.Front.Api.Data.Brd;
using Resto.Front.Api.Data.Organization.Sections;
using Resto.Front.Api;
using iikoPluginTask.DTO;
using System.Linq;
using System.Text;

namespace iikoPluginTask.Logging
{
    public class PluginLogger
    {
        public void LogReserve(IReserve reserveFromIiko)
        {
            ILog log = PluginContext.Log;
            ReserveStatus status = reserveFromIiko.Status;
            log.Info("--------------------------------------------------------------------------------------------");
            log.Info($"Client Id:          {reserveFromIiko.Client.Id}");
            log.Info($"Client Name:        {reserveFromIiko.Client.Name}");
            StringBuilder phonesStringBuilder = new StringBuilder();
            foreach (var phone in reserveFromIiko.Client.Phones)
                phonesStringBuilder.AppendLine(phone.ToString());
            log.Info($"Client Phones:      {phonesStringBuilder}");
            log.Info($"EstimatedStartTime: {reserveFromIiko.EstimatedStartTime}");
            log.Info($"Comment:            {reserveFromIiko.Comment}");
            log.Info($"Status:             {status}");
            StringBuilder tablesBuilder = new StringBuilder();
            foreach (ITable table in reserveFromIiko.Tables)
                tablesBuilder.AppendLine(table.ToString());
            log.Info($"Tables:             {tablesBuilder}");
            log.Info($"GuestsCount:        {reserveFromIiko.GuestsCount}");
            log.Info($"Duration:           {reserveFromIiko.Duration}");
            log.Info($"ShouldRemind:       {reserveFromIiko.ShouldRemind}");
            log.Info($"Order Id:           {reserveFromIiko.Order.Id}");
            log.Info($"New reserve start time: {reserveFromIiko.EstimatedStartTime}");
            log.Info("--------------------------------------------------------------------------------------------");
        }
    }
}
