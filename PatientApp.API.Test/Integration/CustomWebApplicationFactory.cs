using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PatientApp.Data;

namespace PatientApp.API.Test.Integration;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
        //     var dbContextDescriptor = services.SingleOrDefault(
        //         d => d.ServiceType ==
        //              typeof(DbContextOptions<PatientContext>));
        //
        //     //Remove current DBContext
        //     services.Remove(dbContextDescriptor);
        //
        //     // var dbConnectionDescriptor = services.SingleOrDefault(
        //     //     d => d.ServiceType ==
        //     //          typeof(DbConnection));
        //     //
        //     // services.Remove(dbConnectionDescriptor);
        //     //
        //     // // Create open SqliteConnection so EF won't automatically close it.
        //     // services.AddSingleton<DbConnection>(container =>
        //     // {
        //     //     var connection = new SqliteConnection("DataSource=:memory:");
        //     //     connection.Open();
        //     //
        //     //     return connection;
        //     // });
        //
        //     //Add back in a In Memory Context
        //     services.AddDbContext<PatientContext>((container, options) =>
        //     {
        //         var connection = container.GetRequiredService<DbConnection>();
        //         options.UseInMemoryDatabase(Guid.NewGuid().ToString());
        //    });
        });

        builder.UseEnvironment("Development");
    }
}