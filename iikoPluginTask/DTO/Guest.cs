using Resto.Front.Api.Data.Brd;
using Resto.Front.Api.Data.Common;
using Resto.Front.Api.Data.Organization.Sections;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

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


        public Guest()
        {
        }
        public Guest(Reserve handledReserveation)
        {
            this.Guid = handledReserveation.Client.Id.ToString();
            this.Surname = handledReserveation.Client.Surname;
            this.Name = handledReserveation.Client.Name;
            this.MiddleName = string.Empty;

            List<string> phones = new List<string>();
            foreach (IPhone _phones in handledReserveation.Client.Phones)
            {
                phones.Add(_phones.Value);
            }
            this.Phones = phones;
            List<string> emails = new List<string>();
            foreach (IEmail _emails in handledReserveation.Client.Emails)
            {
                emails.Add(_emails.Value);
            }
            this.Emails = emails;
            this.Birthday = handledReserveation.Client.BirthDate.ToString();

            Gender genderEnum = handledReserveation.Client.Gender;
            string gender = (((int)genderEnum == 1) ? "male" : (((int)genderEnum != 2) ? "" : "female"));
            this.Gender = gender;

            string GuestCardNumber = handledReserveation.Client.CardNumber;
            List<Card> cards = new List<Card>
            {
                new Card
                {
                    Number = GuestCardNumber,
                    Track = ""
                }
            };
            this.Cards = cards;

        }

    }
}
