using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentServiceProvider.Infrastructure.Persistence.Context;

namespace PaymentServiceProvider.Test.Api
{
    public class CustomWebApplicationFactory
      : WebApplicationFactory<Program>
    {
        public DataContext DataContext { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureTestServices(services =>
            {
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryAppDb");

                    var internalServiceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                    options.UseInternalServiceProvider(internalServiceProvider);
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                DataContext = serviceProvider.GetRequiredService<DataContext>();
                DataContext.Database.EnsureCreated();
            });
        }

        public HttpClient CreateHttpClient(Action<IServiceCollection> servicesConfiguration)
        {
            var client = WithWebHostBuilder(builder => builder.ConfigureTestServices(servicesConfiguration)).CreateClient();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-Correletion-ID", Guid.NewGuid().ToString());

            return client;
        }
    }
}