using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TradingPortal.Core.Domain
{
    [Table("Holiday")]
    public class Holiday
    {
        [Key]
        public int HolidayId { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [DisplayName("Description")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime DateOf
        {
            get { return _date; }
            set { _date = value; }
        }
               

        public Guid CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }

        [DisplayName("Updated By")]
        public Guid ModifiedBy { get; set; }

        [DisplayName("Update Date")]
        public DateTime DateModified { get; set; }

        private DateTime _date = DateTime.Now;
    }
}
