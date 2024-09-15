using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.API.Mapster
{
    public static class MappingProfiles
    {
        public static IServiceCollection RegisterMapsterConfigurations(this IServiceCollection services)
        {
            TypeAdapterConfig<Category, Category>
                .NewConfig()
                .ConstructUsing(src => Category.Create(src.Name, src.Description, src.ImageUrl, src.Status, src.ParentCategoryId));

            return services;
        }
    }
}
