using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.Repositories;
using OrderApi.Application.Interfaces;

namespace OrderApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedService<OrderDbContext>(services, config, config["MySerilog:FineName"]!);
            services.AddScoped<IOrder, OrderRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);
            return app; 
        }
    }
}
