using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.Domain.Amark;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TradingPortal.Core;
using TradingPortal.Core.ViewModels;
using System.Collections.Generic;
using System;
using TradingPortal.Infrastructure.Services.Interfaces;
using TradingPortal.Infrastructure.Repositories.Interfaces;

namespace TradingPortal.Business
{
    public class ContentManager : IContentManager
    {
        ICurrentUser _currentUser;
        private readonly IContentRepository _contentRepository;
        public ContentManager(IContentRepository contentRepository,ICurrentUser currentUser)
        {
            _contentRepository = contentRepository;
            _currentUser = currentUser;
        }

        public async Task<List<ContentViewModel>> GetAllContent()
        {
            return await _contentRepository.GetAll()
                .Select(c => new ContentViewModel
                {
                    ContentId = c.ContentId,
                    Title = c.Title,
                    Description = c.Description
                }).ToListAsync();
        }

        public async Task<string> GetContentById(int id)
        {
            return await _contentRepository.GetAll()
                .Where(c => c.ContentId == id)
                .Select(c => c.Description).FirstOrDefaultAsync();
 
        }

        public async Task<int> SaveContent(Content content)
        {
            try
            {
                if (content.ContentId == 0)
                {
                    content.DateCreated = DateTime.Now;
                    content.CreatedBy = _currentUser.User.CustomerGuid;
                    await _contentRepository.AddAsyn(content);
                }
                else
                {
                    content.DateModified = DateTime.Now;
                    content.ModifiedBy = _currentUser.User.CustomerGuid;
                    await _contentRepository.UpdateContent(content);
                }

                return content.ContentId;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
