using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Pipelines
{
    public class LoggingPipeline : IPipeline
    {
        private readonly ILogger<LoggingPipeline> _logger;
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public LoggingPipeline(ILogger<LoggingPipeline> logger)
        {
            _logger = logger;
        }

        public async Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct) 
            where TCommand : class, ICommand
        {
            Log("Command: {command}", cmd);

            await next(cmd, ct);
        }

        public async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct) 
            where TCommand : class, ICommand<TResult>
        {
            Log("Command: {command}", cmd);

            var result = await next(cmd, ct);

            Log("Command.Result: {commandResult}", result);

            return result;
        }

        public async Task OnEventAsync<TEvent>(Func<TEvent, CancellationToken, Task> next, TEvent evt, CancellationToken ct) 
            where TEvent : class, IEvent
        {
            Log("Event: {event}", evt);

            await next(evt, ct);
        }

        public async Task<TResult> OnQueryAsync<TQuery, TResult>(Func<TQuery, CancellationToken, Task<TResult>> next, TQuery query, CancellationToken ct) 
            where TQuery : class, IQuery<TResult>
        {
            Log("Query: {query}", query);

            var result = await next(query, ct);

            Log("Query.Result: {queryResult}", result);

            return result;
        }

        private void Log<T>(string message, T instance)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
                _logger.LogTrace(message, JsonSerializer.Serialize(instance, _serializerOptions));
        }
    }
}
