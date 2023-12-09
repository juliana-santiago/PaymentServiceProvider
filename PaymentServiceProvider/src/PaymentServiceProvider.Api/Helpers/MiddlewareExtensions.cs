using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace PaymentServiceProvider.Api.Helpers
{
    public static class Extensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null && contextFeature.Error is ValidationException validationException)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteAsync(
                            JsonSerializer.Serialize(validationException.Errors.Select(error => new
                            {
                                Property = error.PropertyName,
                                Message = error.ErrorMessage
                            })));
                    }
                });
            });
        }
    }
}