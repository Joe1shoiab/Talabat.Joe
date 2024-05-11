using Microsoft.Extensions.Options;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;

namespace Talabat.APIs.Extentions
{
    public static  class ApplicationServicesExtention
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            // To Allow Dependence Injection for GenericRepository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IBasketRepository, BasketRepository>();
            // To Allow Dependence Injection for Mapper
            services.AddAutoMapper(typeof(MappingProfiles));
            return services;

        }


    }
}
