using Resto.Front.Api.Data.Organization.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iikoPluginTask.Mocking
{
    internal class TableMock : ITable
    {
        public int Number { get; set; }

        public Guid Id { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
