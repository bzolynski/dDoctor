using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WPFUi.ViewModels;
using WPFUi.Views;

namespace WPFUi
{
    public class Startup
    {
        private IConfiguration AddConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) //current directory is bin/Debug/netcoreapp3.1
                .AddJsonFile("appsettings.json");

            return configurationBuilder.Build();
        } 
        public IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(AddConfiguration());

            services.AddSingleton<ShellView>();
            services.AddSingleton<ShellViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
