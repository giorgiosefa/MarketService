using MarketService.Api.Extensions;
using MarketService.Contracts.Base;
using MarketService.Domain.Exeptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MarketService.Api.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ErrorResponse response = new ErrorResponse()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Description = exception.Message
            };

            if (exception is IsNotRegisteredException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Description = exception.Message;
            }else if(exception is IsRegisteredException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                response.StatusCode = (int)HttpStatusCode.Conflict;
                response.Description = exception.Message;
            }

            return context.Response.WriteAsync(response.ToJson());
        }
    }
}
