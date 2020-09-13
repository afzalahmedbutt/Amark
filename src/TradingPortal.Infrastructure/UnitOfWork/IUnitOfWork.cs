using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingPortal.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
