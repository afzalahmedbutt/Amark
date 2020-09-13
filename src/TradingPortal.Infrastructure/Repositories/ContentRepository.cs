using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core.Domain.Amark;
using TradingPortal.Infrastructure.DatabaseContexts;
using TradingPortal.Infrastructure.Repositories.Interfaces;

namespace TradingPortal.Infrastructure.Repositories
{
    public class ContentRepository : EFAMarkRespository<Content>,IContentRepository
    {
        private readonly AMarkDbContext dbContext;

        public ContentRepository(AMarkDbContext context) : base(context)
        {
            dbContext = context;
        }
        public async Task<bool> UpdateContent(Content content)
        {
            var entry = dbContext.Entry(content);
            entry.Property(c => c.Title).IsModified = true;
            entry.Property(c => c.Description).IsModified = true;
            entry.Property(c => c.ModifiedBy).IsModified = true;
            entry.Property(c => c.DateCreated).IsModified = true;
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
