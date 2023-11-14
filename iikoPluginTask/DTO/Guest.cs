using System.Collections.Generic;

namespace iikoPluginTask.DTO
{
    public class Guest
    {
        public string Guid { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string MiddleName { get; set; }

        public List<string> Phones { get; set; }

        public List<string> Emails { get; set; }

        public string Birthday { get; set; }

        public string Gender { get; set; }

        public List<Card> Cards { get; set; }
    }
}
