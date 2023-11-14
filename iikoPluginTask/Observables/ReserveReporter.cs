using Resto.Front.Api.Data.Brd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iikoPluginTask.Observables
{
    public class ReserveReporter : IObserver<IReserve>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IReserve value)
        {
            throw new NotImplementedException();
        }
    }
}
