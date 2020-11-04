using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Pipelines
{
    public class ValidationPipeline : Pipeline
    {
        private readonly IServiceProvider _provider;

        public ValidationPipeline(IServiceProvider provider)
        {
            _provider = provider;
        }

        public override async Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct)
        {
            await ValidateAndThrowAsync(cmd, ct);
            await next(cmd, ct);
        }

        public override async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct)
        {
            await ValidateAndThrowAsync(cmd, ct);
            return await next(cmd, ct);
        }

        public override async Task OnEventAsync<TEvent>(Func<TEvent, CancellationToken, Task> next, TEvent evt, CancellationToken ct)
        {
            await ValidateAndThrowAsync(evt, ct);
            await next(evt, ct);
        }

        private async Task ValidateAndThrowAsync<T>(T instance, CancellationToken ct)
        {
            var validator = _provider.GetRequiredService<IValidator<T>>();
            await validator.ValidateAndThrowAsync(instance, ct);
        }
    }
}
