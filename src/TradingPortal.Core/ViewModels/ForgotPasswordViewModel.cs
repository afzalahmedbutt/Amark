using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class ForgotPasswordViewModel
    {
        //[Required]
        //[EmailAddress]
        public string Email { get; set; }
    }
}
