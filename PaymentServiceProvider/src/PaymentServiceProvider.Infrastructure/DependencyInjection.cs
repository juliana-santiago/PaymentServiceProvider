using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentServiceProvider.Domain.Payables;
using PaymentServiceProvider.Domain.Transactions;
using PaymentServiceProvider.Infrastructure.Persistence.Context;
using PaymentServiceProvider.Infrastructure.Persistence.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace PaymentServiceProvider.Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IPayableRepository, PayableRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();

            services.AddScoped<DbContext, DataContext>();

            return services;
        }
    }
}
