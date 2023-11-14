using iikoPluginTask.DTO.Constructor;
using iikoPluginTask.Observables;
using Resto.Front.Api;
using Resto.Front.Api.Data.Brd;
using Resto.Front.Api.Data.Common;
using Resto.Front.Api.Data.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iikoPluginTask
{
    public class ReservePlugin : IFrontPlugin
    {
        private WebSocketClient _webSocket;
        private ReserveReporter reserverReporter;
        public ReservePlugin()
        {
            reserverReporter=new ReserveReporter();
            PluginContext.Notifications.ReserveChanged.Subscribe(reserverReporter);
        }

        public void Dispose()
        {
        }


    }
}
