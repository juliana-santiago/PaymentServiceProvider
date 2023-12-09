using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentServiceProvider.Infrastructure.Persistence.Context;
using System.Diagnostics.CodeAnalysis;

namespace PaymentServiceProvider.Infrastructure.Extensions.Database
{
    [ExcludeFromCodeCoverage]
    public static class DatabaseExtensions
    {
        public static void RegisterDatabase(
            this IServiceCollection services,
            IConfiguration configurations)
        {
            var connectionString = configurations.GetConnectionString("SqlServer");

            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
