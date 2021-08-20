using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFocused.Utility
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConfigurationService(this IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
        }
    }
}
