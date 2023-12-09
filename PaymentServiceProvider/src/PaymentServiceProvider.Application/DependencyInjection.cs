using Microsoft.Extensions.DependencyInjection;
using PaymentServiceProvider.Application.Services;
using PaymentServiceProvider.Application.Services.Abstractions;
using PaymentServiceProvider.Application.Validators;
using System.Diagnostics.CodeAnalysis;

namespace PaymentServiceProvider.Application
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //Services
            services.AddTransient<IPayableService, PayableService>();
            services.AddTransient<ITransactionService, TransactionService>();

            //Validators
            services.AddTransient<CreateTransactionValidator>();

            return services;
        }
    }
}
