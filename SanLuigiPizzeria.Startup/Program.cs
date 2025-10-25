using Microsoft.Extensions.DependencyInjection;
using SanLuigiPizzeria.Business;
using SanLuigiPizzeria.DataAccess;
using SanLuigiPizzeria.Presentation;
using System.Threading.Tasks;

namespace SanLuigiPizzeria.Startup
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Mamma mia pizzeria!");

            var services = new ServiceCollection();
            services.AddSingleton<FileRepository>();
            services.AddSingleton<PizzeriaService>();
            services.AddSingleton<ConsoleView>();

            var provider = services.BuildServiceProvider();

            var console = provider.GetRequiredService<ConsoleView>();

            console.StartApplicationAsync();
        }
    }
}
