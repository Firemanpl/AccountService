using System;
using System.Threading.Tasks;
using AccountService.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ILogger = NLog.ILogger;

namespace AccountService.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundExcepion notFoundExcepion)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundExcepion.Message);
            }
            catch (ForbidException forbidException)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(forbidException.Message);
            }
            catch (BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequestException.Message);
            }
            catch (WarningException warningException)
            {
                context.Response.StatusCode = 406;
                await context.Response.WriteAsync(warningException.Message);
            }catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }
           
        }
    }
}