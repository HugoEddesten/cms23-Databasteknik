
using DbAssignment.Contexts;
using DbAssignment.Repositories;
using DbAssignment.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DbAssignment
{
    public class Program
    {
        
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\nackademin\DatabasHantering\cms23_Databasteknik_Assignment_HugoEddesten\DbAssignment\Contexts\assignment_database.mdf;Integrated Security=True;Connect Timeout=30"));
                   
                    // Services
                    services.AddScoped<CustomerService>();
                    services.AddScoped<AddressService>();
                    services.AddScoped<CustomerInformationsService>();
                    services.AddScoped<MenuService>();
                    services.AddScoped<ProductService>();
                    services.AddScoped<OrderService>();
                    

                    // Repositories
                    services.AddScoped<CustomerRepository>();
                    services.AddScoped<CustomerInformationRepository>();
                    services.AddScoped<CustomerInformationTypeRepository>();
                    services.AddScoped<AddressRepository>();
                    services.AddScoped<ProductRepository>();
                    services.AddScoped<OrderRepository>();
                })

                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })

                .Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var menuService = services.GetRequiredService<MenuService>();

                await menuService.MainMenu();
            }
            

            await host.RunAsync();
            
        }
    }
}