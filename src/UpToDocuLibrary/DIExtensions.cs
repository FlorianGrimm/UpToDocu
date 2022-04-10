using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace UpToDocu {
    public static class DIExtensions {
        public static IServiceCollection AddUpToDocu(
            this IServiceCollection services,
            Action<UTDServiceOptions>? configure) {

            services.AddSingleton<UtdService>();
            var optionsBuilder = services.AddOptions<UTDServiceOptions>();
            if (configure is not null) {
                optionsBuilder.Configure(configure);
            }
            return services;
        }
    }
}
