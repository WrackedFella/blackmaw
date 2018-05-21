using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Blackmaw.Api.ExceptionHandling
{
    public class BlackmawExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BlackmawExceptionHandlingMiddleware> _logger;

        public BlackmawExceptionHandlingMiddleware(RequestDelegate next, ILogger<BlackmawExceptionHandlingMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
//#if DEBUG
//            await this._next(context);
//#else
//            try
//            {
//                await this._next(context);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message);
//                await HandleExceptionAsync(context, ex);
//            }
//#endif
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var exceptionMessage = exception.Message;

            // Add custom exceptions as needed.
            //  ToDo: filter DB errors from here.
            code = HttpStatusCode.BadRequest;
            exceptionMessage = exception.Message;

            var result = JsonConvert.SerializeObject(new { message = exceptionMessage });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
