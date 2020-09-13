using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Infrastructure.CustomExceptionMiddleware;

namespace TradingPortal.Infrastructure.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        //public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        //{
        //    app.UseExceptionHandler(appError =>
        //    {
        //        appError.Run(async context =>
        //        {
        //            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //            context.Response.ContentType = "application/json";

        //            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        //            if (contextFeature != null)
        //            {
        //                //logger.LogError($"Something went wrong: {contextFeature.Error}");

        //                await context.Response.WriteAsync(new ErrorDetails()
        //                {
        //                    StatusCode = context.Response.StatusCode,
        //                    Message = "Internal Server Error."
        //                }.ToString());
        //            }
        //        });
        //    });
        //}

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
