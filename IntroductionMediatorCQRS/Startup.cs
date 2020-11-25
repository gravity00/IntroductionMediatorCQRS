using System.Linq;
using System.Text;
using System.Text.Json;
using FluentValidation;
using IntroductionMediatorCQRS.Database;
using IntroductionMediatorCQRS.Pipelines;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
                o.UseInMemoryDatabase("ApiDbContext").ConfigureWarnings(warn =>
                {
                    // since InMemoryDatabase does not support transactions
                    // for test purposes we are going to ignore this exception
                    warn.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                });
            });

            // registration using this project custom pipelines
            services.AddMediator(o =>
            {
                o.AddPipeline<LoggingPipeline>();
                o.AddPipeline<TimeoutPipeline>();
                o.AddPipeline<ValidationPipeline>();
                o.AddPipeline<TransactionPipeline>();
                o.AddPipeline<AuditPipeline>();

                foreach (var implementationType in typeof(Startup)
                    .Assembly
                    .ExportedTypes
                    .Where(t => t.IsClass && !t.IsAbstract))
                {
                    foreach (var serviceType in implementationType
                        .GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
                    {
                        o.Services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));
                    }
                }

                o.AddHandlersFromAssemblyOf<Startup>();
            });

            // or, if using the SimpleSoft.Mediator.Microsoft.Extensions.* pipelines
            //services.AddMediator(o =>
            //{
            //    o.AddPipelineForLogging(options =>
            //    {
            //        options.LogCommandResult = true;
            //        options.LogQueryResult = true;
            //    });
            //    o.AddPipeline<TimeoutPipeline>();
            //    o.AddPipelineForValidation(options =>
            //    {
            //        options.ValidateCommand = true;
            //        options.ValidateEvent = true;
            //    });
            //    o.AddPipelineForEFCoreTransaction<ApiDbContext>(options =>
            //    {
            //        options.BeginTransactionOnCommand = true;
            //    });
            //    o.AddPipeline<AuditPipeline>();

            //    o.AddValidatorsFromAssemblyOf<Startup>();
            //    o.AddHandlersFromAssemblyOf<Startup>();
            //});
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
