using iikoPluginTask.DTO.Constructor;
using iikoPluginTask.Observables;
using Resto.Front.Api;

namespace iikoPluginTask
{
    public class ReservePlugin : IFrontPlugin
    {
        private ReserveReporter reserveReporter;
        public ReservePlugin()
        {
            reserveReporter=new ReserveReporter();
            PluginContext.Notifications.ReserveChanged.Subscribe(reserveReporter);
        }

        public void Dispose()
        {
        }
    }
}
