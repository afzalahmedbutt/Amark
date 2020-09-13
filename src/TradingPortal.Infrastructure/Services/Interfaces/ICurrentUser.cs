
using TradingPortal.Core.Domain.Identity;

namespace TradingPortal.Infrastructure.Services.Interfaces
{
    public interface ICurrentUser
    {
        Customer User { get; }
        string GetCurrentIpAddress();
        string GetAbsoluteUri();
        

        
    }
}
