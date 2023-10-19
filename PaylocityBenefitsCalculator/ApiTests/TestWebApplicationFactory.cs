using Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ApiTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Replace the registered database connection with an in-memory database.
                var dbConnectionDescriptor = services.Single(d => d.ServiceType == typeof(BenefitsContext));
                services.Remove(dbConnectionDescriptor);

                services.AddDbContext<BenefitsContext>(opt =>
                    opt.UseInMemoryDatabase("Tests"));
            });
        }
    }
}
