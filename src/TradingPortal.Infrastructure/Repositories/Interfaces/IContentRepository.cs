using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core;
using TradingPortal.Core.Domain.Amark;

namespace TradingPortal.Infrastructure.Repositories.Interfaces
{
    public interface IContentRepository : IAMarkRepository<Content>
    {
        Task<bool> UpdateContent(Content content);
    }
}
