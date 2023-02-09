using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebMandel.Models
{
    public class ContactModel
    {
        [StringLength(256, MinimumLength = 3), Display(Name = "Your Name")]
        public string Name { get; set; }
        [EmailAddress, Required, Display(Name = "Your Email")]
        public string Email { get; set; }
        public string Subject { get; set; }
        [Required, DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}