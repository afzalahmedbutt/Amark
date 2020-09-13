using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core.Domain.Amark;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Business.interfaces
{
    public interface IContentManager
    {
        Task<string> GetContentById(int id);
        Task<List<ContentViewModel>> GetAllContent();
        Task<int> SaveContent(Content content);
    }
}
