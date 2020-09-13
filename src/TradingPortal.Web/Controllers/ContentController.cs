
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.Domain.Amark;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    public class ContentController : Controller
    {
        private readonly IContentManager _contentManager;
        public ContentController(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        [HttpGet("GetContent")]
        public async Task<List<ContentViewModel>> GetContent()
        {
            return await _contentManager.GetAllContent();
        }

        //[HttpPost]
        [HttpGet("GetContentById/{id}")]
        public async Task<string> GetContentById(int id)
        {
            return await _contentManager.GetContentById(id);
        }

        
        [HttpPost("SaveContent")]
        public async Task<int> SaveContent([FromBody]Content content)
        {
            return await _contentManager.SaveContent(content);
        }
    }
}