﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class PasswordRecoveryToken
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}
