using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactList.Models
{

    public class PhoneNumber
    {
        public enum PhoneType
        {
          Work,
          Personal
        };

        public int ID { get; set; }
        public int ContactID { get; set; }
        public string number { get; set; }
        [Required(AllowEmptyStrings = false)]
        public PhoneType Type { get; set; }

        public virtual Contact Contact { get; set; }

    }
}