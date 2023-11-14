using Resto.Front.Api.Data.Brd;
using Resto.Front.Api.Data.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iikoPluginTask.Mocking
{
    public class ClientMock : IClient
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Nick { get; set; }

        public string Comment { get; set; }

        public string CardNumber { get; set; }

        public bool InBlacklist { get; set; }

        public string BlacklistReason { get; set; }

        public IReadOnlyList<IAddress> Addresses { get; set; }

        public int? MainAddressIndex { get; set; }

        public IReadOnlyList<IPhone> Phones { get; set; }

        public IReadOnlyList<IEmail> Emails { get; set; }

        public IMarketingSource MarketingSource { get; set; }

        public Guid IikoNetId { get; set; }

        public Guid IikoBizId { get; set; }

        public DateTime? DateCreated { get; set; }

        public IUser LinkedCounteragent { get; set; }

        public int Revision { get; set; }

        public bool ReceivesNotifications { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? LastOrderDate { get; set; }

        public Gender Gender { get; set; }

        public bool? PersonalDataConsent { get; set; }

        public Guid Id { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
