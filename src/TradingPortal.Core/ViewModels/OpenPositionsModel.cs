using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class OpenPositionsModel
    {
        public OpenPositionsModel()
        {
            AvailableCategories = new List<SelectListItem>();
        }

        public int GridSize { get; set; }

        public int SearchCategoryId { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        //public GridModel<Nop.Web.Models.Amark.OpenPositionsModel> Trades { get; set; }

        //public bool GridSize1 { get; set; }

       
        public DateTime? StartDate { get; set; }

        
        public DateTime? EndDate { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }

    //
    // Summary:
    //     Represents the selected item in an instance of the System.Web.Mvc.SelectList
    //     class.
    public class SelectListItem
    {
      
        public bool Selected { get; set; }
        
        public string Text { get; set; }
        
        public string Value { get; set; }
    }
}
