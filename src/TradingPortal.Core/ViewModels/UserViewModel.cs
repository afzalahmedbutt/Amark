using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TradingPortal.Core.Domain.Identity;

namespace TradingPortal.Core.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Username is required"), StringLength(200, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 200 characters")]
        public string UserName { get; set; }

        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //[Required(ErrorMessage = "Email is required"), StringLength(200, ErrorMessage = "Email must be at most 200 characters"), EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string AmarkTradingPartnerNumber { get; set; }
        public string AmarkTPAPIKey { get; set; }
        public string AdminComment { get; set; }
        public bool IsTaxExempt { get; set; }
        public int VendorId { get; set; }
        //public string JobTitle { get; set; }

        //public string PhoneNumber { get; set; }

        //public string Configuration { get; set; }

        //public bool IsEnabled { get; set; }

        //public bool IsLockedOut { get; set; }
        public bool Active { get; set; }

        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastActivityDateUtc { get; set; }
        public string CompanyName { get; set; }

        //[MinimumCount(1, ErrorMessage = "Roles cannot be empty")]
        public string[] Roles { get; set; }
        public List<SelectListItem> Vendors { get; set; }
    }

    public class UsersGridCommand
    {
        public UsersGridCommand()
        {
            this.CustomerRoleIds = new int[0];
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int FirstRow { get; set; }
        public int LastRow { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public int[] CustomerRoleIds { get; set; }
    }

    public class UsersGridDto
    {
        public List<Tuple<Customer,string[]>> Customers { get; set; }
        public int Count { get; set; }
    }

    public class UsersGridResponse
    {
        public List<UserViewModel> Users { get; set; }
        public int Count { get; set; }
    }
}
