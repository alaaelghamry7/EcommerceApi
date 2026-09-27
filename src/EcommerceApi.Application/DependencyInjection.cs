using EcommerceApi.Application.Interfaces.Services;
using EcommerceApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
