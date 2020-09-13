using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.Services.Interfaces;


namespace TradingPortal.Infrastructure.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        //private readonly IGenericAttributeManager genericAttributeManager;
        private readonly IHostingEnvironment _env;

        //private readonly ILoggerFactory _factory;
        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger, IHostingEnvironment env)
        {
            _logger = logger;
            _next = next;
            _env = env;
            
        }

        public async Task InvokeAsync(HttpContext httpContext, ICurrentUser currentUser, IGenericAttributeManager genericAttributeManager)
        {
            try
            {
              
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Guid exceptionIdentifier = Guid.Empty;
                if (_env.IsProduction())
                {

                    var ipAddress = httpContext?.Connection?.RemoteIpAddress?.ToString();
                    MappedDiagnosticsContext.Set("IpAddress", ipAddress);
                    var pageUrl = httpContext?.Request?.GetDisplayUrl();
                    MappedDiagnosticsContext.Set("PageUrl", pageUrl);
                    var customerId = currentUser.User?.Id;
                    MappedDiagnosticsContext.Set("CustomerId", customerId);
                    if (customerId != null)
                    {

                    }

                    exceptionIdentifier = Guid.NewGuid();
                    MappedDiagnosticsContext.Set("LogUniqueId", exceptionIdentifier);
                    _logger.LogError(ex);
                }
                    //var loggerFromDI = _factory.CreateLogger("Values");
                    //loggerFromDI.LogDebug("From dependency injection factory");
                    //_logger.LogError(ex);
                    //_logger.LogError($"Something went wrong: {ex}");
                    await HandleExceptionAsync(httpContext, ex, currentUser, exceptionIdentifier,_env);
                
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception,ICurrentUser currentUser,Guid exceptionIdentifier,IHostingEnvironment _env)
        {
            var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                // Get which route the exception occurred at
                string routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                Exception exceptionThatOccurred = exceptionFeature.Error;

                // TODO: Do something with the exception
                // Log it with Serilog?
                // Send an e-mail, text, fax, or carrier pidgeon?  Maybe all of the above?
                // Whatever you do, be careful to catch any exceptions, otherwise you'll end up with a blank page and throwing a 500
            }

            if (_env.IsProduction())
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return context.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error from the custom middleware.",
                    ExceptionIdentifier = exceptionIdentifier
                }.ToString());
            }
            else
            {
                return context.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = exception.InnerException.Message
                    
                }.ToString());
            }
        }
    }
}
