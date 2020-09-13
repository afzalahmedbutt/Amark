using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class ResetPasswordViewModel
    {
        public int UserId { get; set; }
        //[Required]
        //[EmailAddress]
        public string Email { get; set; }

        public string NewPassword { get; set; }
        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
