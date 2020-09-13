using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core.Domain;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Business.interfaces
{
    public interface ITopicManager
    {
        Task<TopicViewModel> GetTopicBySystemName(string systemName);
    }
}
