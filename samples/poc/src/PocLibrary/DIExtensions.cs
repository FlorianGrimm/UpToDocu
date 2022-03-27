using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Poc.Entity;
using Poc.Repository;

using Microsoft.Extensions.DependencyInjection;

namespace Poc {
    public static class DIExtensions {
        public static IServiceCollection AddRepository(
            this IServiceCollection services
            ) {
            services.AddTransient<PocContext>();
            services.AddTransient<PocRepository>();
            services.AddScoped<PocContextScoped>();
            return services;
        }
    }
}
