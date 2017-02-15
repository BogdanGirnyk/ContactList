using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactList.Models;

namespace ContactList.DAL
{
    public class ContactListInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ContactListContext>
    {
        protected override void Seed(ContactListContext context)
        {
            var contacts = new List<Contact>
            {
                new Contact {FirstName="Carson",LastName="Alexander", Adress="Kyiv" },
                new Contact {FirstName="Meredith",LastName="Alonso", Adress = "Chicago" },
                new Contact {FirstName="Laura",LastName="Norman" }
            };
            contacts.ForEach(s => context.Contacts.Add(s));
            context.SaveChanges();

            var phones = new List<PhoneNumber>
            {
                new PhoneNumber {ContactID = 1,number = "0631337563",Type = PhoneNumber.PhoneType.Personal},
                new PhoneNumber {ContactID = 1,number = "0445437894",Type = PhoneNumber.PhoneType.Work},
                new PhoneNumber {ContactID = 2,number = "0995782753",Type = PhoneNumber.PhoneType.Personal},
                new PhoneNumber {ContactID = 3,number = "0664639964",Type = PhoneNumber.PhoneType.Personal}
            };
            phones.ForEach(s => context.PhoneNumbers.Add(s));
            context.SaveChanges();


            var users = new List<User>
            {
                new User {username = "admin", password = "1", roles = "admin" },
                new User {username = "user" , password = "2", roles = "" }
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

        }
    }
}