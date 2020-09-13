using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TradingPortal.Core.Domain.Amark
{
    public class Content : BaseEntity
    {
        [Key]
        public int ContentId { get; set; }
        
        public string Title { get; set; }

        
        public string Description { get; set; }

        public Guid? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateContent { get; set; }

        //public virtual List<Pages_Content> Pages_Content { get; set; }

    }
}
