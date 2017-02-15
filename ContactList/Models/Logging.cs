using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ContactList.Models
{
    public class Logging
    {
        [Key]
        public int LogId { get; set; }
        public string Application { get; set; }
        public string LogType { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}