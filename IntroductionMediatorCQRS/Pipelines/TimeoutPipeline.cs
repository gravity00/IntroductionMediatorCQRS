using System;
using System.Threading;
using System.Threading.Tasks;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Pipelines
{
    public class TimeoutPipeline : IPipeline
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromMilliseconds(500);

        public async Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct) 
            where TCommand : class, ICommand
        {
            using var cts = CreateCancellationTokenSource(ct);

            await next(cmd, cts.Token);
        }

        public async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct) 
            where TCommand : class, ICommand<TResult>
        {
            using var cts = CreateCancellationTokenSource(ct);

            return await next(cmd, cts.Token);
        }

        public async Task OnEventAsync<TEvent>(Func<TEvent, CancellationToken, Task> next, TEvent evt, CancellationToken ct) 
            where TEvent : class, IEvent
        {
            using var cts = CreateCancellationTokenSource(ct);

            await next(evt, cts.Token);
        }

        public async Task<TResult> OnQueryAsync<TQuery, TResult>(Func<TQuery, CancellationToken, Task<TResult>> next, TQuery query, CancellationToken ct) 
            where TQuery : class, IQuery<TResult>
        {
            using var cts = CreateCancellationTokenSource(ct);

            return await next(query, cts.Token);
        }

        private static CancellationTokenSource CreateCancellationTokenSource(CancellationToken ct)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(Timeout);

            return cts;
        }
    }
}
