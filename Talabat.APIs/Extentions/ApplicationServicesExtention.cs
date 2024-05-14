using Microsoft.Extensions.Options;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Extentions
{
    public static  class ApplicationServicesExtention
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            // To Allow Dependence Injection for GenericRepository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IOrderService, OrderService>();
            // To Allow Dependence Injection for Mapper
            services.AddAutoMapper(typeof(MappingProfiles));
            return services;

        }


    }
}
