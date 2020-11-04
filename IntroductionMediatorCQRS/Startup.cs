using System.Linq;
using System.Text;
using System.Text.Json;
using FluentValidation;
using IntroductionMediatorCQRS.Database;
using IntroductionMediatorCQRS.Pipelines;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

                // comment if using SimpleSoft.Mediator.Microsoft.Extensions.ValidationPipeline
                o.AddPipeline<ValidationPipeline>();

                // remove comment if using SimpleSoft.Mediator.Microsoft.Extensions.ValidationPipeline
                //o.AddPipelineForValidation(options =>
                //{
                //    options.ValidateCommand = true;
                //    options.ValidateEvent = true;
                //});
                //o.AddValidatorsFromAssemblyOf<Startup>();

                o.AddHandlersFromAssemblyOf<Startup>();
            });

            // comment if using SimpleSoft.Mediator.Microsoft.Extensions.ValidationPipeline
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

            app.Use(async (ctx, next) =>
            {
                try
                {
                    await next();
                }
                catch (ValidationException e)
                {
                    var response = ctx.Response;
                    if (response.HasStarted)
                        throw;

                    ctx.RequestServices
                        .GetRequiredService<ILogger<Startup>>()
                        .LogWarning(e, "Invalid data has been submitted");

                    response.Clear();
                    response.StatusCode = 422;
                    await response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        Message = "Invalid data has been submitted",
                        ModelState = e.Errors.ToDictionary(error => error.ErrorCode, error => error.ErrorMessage)
                    }), Encoding.UTF8, ctx.RequestAborted);
                }
            });

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
