using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradingPortal.Infrastructure;

namespace TradingPortal.Web.Controllers
{
    [ServiceFilter(typeof(StoreClosedAttribute))]
    public class BaseController : Controller
    {
        private readonly IHttpContextAccessor _context;

        public BaseController(IHttpContextAccessor context)
        {
            _context = context;
        }

        public ISession Session
        {
            get
            {
                return _context.HttpContext.Session;
            }
        }
    }
}