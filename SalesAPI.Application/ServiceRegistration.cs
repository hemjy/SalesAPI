using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SalesAPI.Application.Validations;

namespace SalesAPI.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplication(this IServiceCollection services)
        {

            var assembly = typeof(AppAssembly).Assembly;
            services.AddValidatorsFromAssembly(assembly);
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}
