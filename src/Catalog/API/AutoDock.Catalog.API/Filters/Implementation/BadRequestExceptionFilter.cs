using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AutoDock.Catalog.API.Filters.Implementation
{
    public sealed class BadRequestExceptionFilter : IExceptionFilter
    {
        public ILogger Logger { get; }
        
        public BadRequestExceptionFilter(ILogger<BadRequestExceptionFilter> logger)
        {
            Logger = logger;
        }
        
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() != typeof(ApplicationException)) return;

            var message = new
            {
                Origin = nameof(ApplicationException),
                ErrorMessage = context.Exception.Message,
                Trace = context.Exception.StackTrace
            };

            Logger.LogInformation("{message}", message);

            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Content = context.Exception.Message
            };

            context.ExceptionHandled = true;
        }
    }
}
