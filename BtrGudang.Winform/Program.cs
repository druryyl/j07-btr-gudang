using BtrGudang.Winform.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BtrGudang.Winform
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();


            // Setup DI
            var services = new ServiceCollection();
            ConfigureServices(services, configuration);
            ServiceProvider = services.BuildServiceProvider();

            // Run your main form
            var mainForm = ServiceProvider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register configuration options
            services.Configure<DatabaseOptions>(configuration.GetSection("DatabaseOptions"));

            // Register your DAL and other services
            //services.AddTransient<IPackingOrderItemDal, PackingOrderItemDal>();

            // Register forms
            services.AddTransient<MainForm>();
        }
    }
}
