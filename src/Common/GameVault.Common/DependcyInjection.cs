using GameVault.Common.Helpers;
using GameVault.Common.Interfaces.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameVault.Common
{
    public static class DependcyInjection
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services.AddTransient<IImageHandler, ImageHandler>();
            return services;
        }
    }
}
