using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using TradingPortal.Infrastructure.Extensions;

namespace TradingPortal.Infrastructure.ComplexTypes
{
    public class SearchCustomerViewModel : IDbDataReaderResult<SearchCustomerViewModel>
    {
        public int? Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public int? TotalCustomers { get; set; }
        public bool IsActive { get; set; }
        public string CreatedOnUtc { get; set; }
        public string LastActivityDateUtc { get; set; }
        public string Roles { get; set; }

        public SearchCustomerViewModel Convert(DbDataReader reader)
        {
            Id = reader.SafeGetNullableInt("Id");
            UserName = reader.SafeGetString("UserName");
            FirstName = reader.SafeGetString("FirstName");
            LastName = reader.SafeGetString("LastName");
            Email = reader.SafeGetString("Email");
            Company = reader.SafeGetString("Company");
            TotalCustomers = reader.SafeGetNullableInt("TotalCustomers");
            IsActive = reader.SafeGetNullableBool("IsActive") ?? false;
            CreatedOnUtc = reader.SafeGetString("CreatedOnUtc");
            LastActivityDateUtc = reader.SafeGetString("LastActivityDateUtc");
            Roles = reader.SafeGetString("Roles");
            FullName = FirstName + " " + LastName;
           
            return this;
        }
    }

    public class SearchCustomerGridDto
    {
        public SearchCustomerGridDto()
        {
            this.Customers = new List<SearchCustomerViewModel>();
        }
        public int TotalCustomers { get; set; }
        public List<SearchCustomerViewModel> Customers { get; set; }
    }
}
