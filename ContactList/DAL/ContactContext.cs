using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ContactList.Models;
using System.Text;

namespace ContactList.DAL
{
    public class ContactListContext : DbContext
    {
        public ContactListContext() :  base("ContactListContext")
        {}

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Logging> Loggings { get; set; }


        public static void LogTrace(string message)
        {
            var context = System.Web.HttpContext.Current;
            var logEntity = new ContactList.Models.Logging()
            {
                Application = "Contact list " + context.Session.SessionID,
                LogType = "Trace",
                Message = message,
                UserId = string.IsNullOrEmpty(context.User.Identity.Name) ? "" : context.User.Identity.Name,
                CreatedDate = DateTime.Now,
            };


            var storeDB = new ContactListContext();
            storeDB.Loggings.Add(logEntity);
            storeDB.SaveChanges();

        }

        public static void LogTrace(Logging Log)
        { 
            var storeDB = new ContactListContext();
            storeDB.Loggings.Add(Log);
            storeDB.SaveChanges();

        }

        public static void LogException(Exception ex)
        {
            var sb = new StringBuilder();

            var exceptionType = ex.GetType().ToString();
            var exceptionTypeSplit = exceptionType.Split('.');
            exceptionType = exceptionTypeSplit[exceptionTypeSplit.Length - 1];

            sb.Append(exceptionType).Append(": ").Append(ex.Message);
            sb.Append(ex.StackTrace);

            var innerException = ex.InnerException;
            if (innerException != null)
            {
                var innerExType = innerException.GetType().ToString();
                var innerExTypeTypeSplit = innerExType.Split('.');
                innerExType = innerExTypeTypeSplit[innerExTypeTypeSplit.Length - 1];

                sb.Append("\r\nINNER EXCEPTION: ").Append(innerExType).Append(": ")
                    .Append(innerException.Message).Append(innerException.StackTrace);
            }

            var context = System.Web.HttpContext.Current;
            var logEntity = new Logging()
            {
                Application = "Contact List " + context.Session.SessionID,
                LogType = "Exception",
                Message = sb.ToString(),
                UserId = string.IsNullOrEmpty(context.User.Identity.Name) ? "" : context.User.Identity.Name,
                CreatedDate = DateTime.Now,
            };


                LogTrace(logEntity);

        }
    }

}