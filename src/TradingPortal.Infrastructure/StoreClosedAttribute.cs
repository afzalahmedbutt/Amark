using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Infrastructure
{
    public class StoreClosedAttribute : ActionFilterAttribute
    {
        private readonly ISettingsService _settingsService;
        public StoreClosedAttribute(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
       

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null || context.HttpContext == null)
                return;



            string actionName = context.ActionDescriptor.DisplayName;
            if (String.IsNullOrEmpty(actionName))
                return;

            string controllerName = context.Controller.ToString();
            if (String.IsNullOrEmpty(controllerName))
                return;

            //don't apply filter to child methods
            //if (filterContext.IsChildAction)
            //    return;

            var isStoreClosed = _settingsService.GetSettingByKey<bool>("storeinformationsettings.storeclosed", storeId: 1);
            if (!isStoreClosed)
            {
                await next();
                //return;
            }
            else
            {
                var jsonString = "{\"url\":\"storeclose\"}";
                //var jsonString = "{\"\":\"storeclose\"}";
                byte[] data = Encoding.UTF8.GetBytes(jsonString);
                context.HttpContext.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
                context.HttpContext.Response.StatusCode = 200;
                //await context.HttpContext.Response.WriteAsync(jsonString);
                await context.HttpContext.Response.WriteAsync(jsonString);
            }

        }
    }
}
