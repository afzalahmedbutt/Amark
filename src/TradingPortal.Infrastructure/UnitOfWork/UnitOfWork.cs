using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core;
using TradingPortal.Core.Domain.Common;
using TradingPortal.Infrastructure.DatabaseContexts;
using TradingPortal.Infrastructure.Repositories;

namespace TradingPortal.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(ApplicationDbContext context)
        {
            DbContext = context;
        }

        private IRepository<GenericAttribute> _genericAttributeRepository;




        public IRepository<GenericAttribute> GenericAttributesRepo
        {
            get
            {
                if(_genericAttributeRepository == null)
                {
                    _genericAttributeRepository = new EfRepository<GenericAttribute>(DbContext);
                }
                return _genericAttributeRepository;
            }
            
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        private ApplicationDbContext DbContext { get; }
    }
}
