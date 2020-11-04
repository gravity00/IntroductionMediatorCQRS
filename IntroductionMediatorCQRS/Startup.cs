using System.Linq;
using FluentValidation;
using IntroductionMediatorCQRS.Database;
using IntroductionMediatorCQRS.Pipelines;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntroductionMediatorCQRS
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen();

            services.AddDbContext<ApiDbContext>(o =>
            {
                o.UseInMemoryDatabase("ApiDbContext");
            });

            services.AddMediator(o =>
            {
                o.AddPipeline<LoggingPipeline>();
                o.AddPipeline<TimeoutPipeline>();

                o.AddHandlersFromAssemblyOf<Startup>();
            });

            foreach (var implementationType in typeof(Startup)
                .Assembly
                .ExportedTypes
                .Where(t => t.IsClass && !t.IsAbstract))
            {
                foreach (var serviceType in implementationType
                    .GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
                {
                    services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));
                }
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mediator ExampleApi V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
