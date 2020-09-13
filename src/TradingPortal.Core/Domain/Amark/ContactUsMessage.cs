using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TradingPortal.Core.Domain.Amark
{
    public class ContactUsMessage : BaseEntity
    {
        //[Key]
        //public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string SendToEmailAliasCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PreferredContactMethod { get; set; }
        //public string PhoneOrEmail { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
