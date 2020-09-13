using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class UserEditViewModel : UserViewModel
    {
        public string CurrentPassword { get; set; }

        //[MinLength(6, ErrorMessage = "New Password must be at least 6 characters")]
        public string NewPassword { get; set; }
        private bool IsLockedOut { get; } //Hide base member
    }
}
