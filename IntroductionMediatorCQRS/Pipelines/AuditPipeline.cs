using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Pipelines
{
    public class AuditPipeline : Pipeline
    {
        private readonly DbSet<CommandEntity> _commands;
        private readonly DbSet<EventEntity> _events;

        public AuditPipeline(ApiDbContext context)
        {
            _commands = context.Set<CommandEntity>();
            _events = context.Set<EventEntity>();
        }

        public override async Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct)
        {
            var command = (await _commands.AddAsync(new CommandEntity
            {
                ExternalId = cmd.Id,
                Name = typeof(TCommand).Name,
                Payload = JsonSerializer.Serialize(cmd),
                Result = null,
                CreatedOn = cmd.CreatedOn,
                CreatedBy = cmd.CreatedBy,
                ExecutionTime = TimeSpan.Zero
            }, ct)).Entity;

            using (CommandScope.Begin(command.ExternalId, command.Id))
            {
                await next(cmd, ct);
            }

            command.ExecutionTime = DateTimeOffset.UtcNow - cmd.CreatedOn;
        }

        public override async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct)
        {
            var command = (await _commands.AddAsync(new CommandEntity
            {
                ExternalId = cmd.Id,
                Name = typeof(TCommand).Name,
                Payload = JsonSerializer.Serialize(cmd),
                Result = null,
                CreatedOn = cmd.CreatedOn,
                CreatedBy = cmd.CreatedBy,
                ExecutionTime = TimeSpan.Zero
            }, ct)).Entity;

            TResult result;
            
            using (CommandScope.Begin(command.ExternalId, command.Id))
            {
                result = await next(cmd, ct);
            }

            command.Result = result == null ? null : JsonSerializer.Serialize(result);
            command.ExecutionTime = DateTimeOffset.UtcNow - cmd.CreatedOn;

            return result;
        }

        public override async Task OnEventAsync<TEvent>(Func<TEvent, CancellationToken, Task> next, TEvent evt, CancellationToken ct)
        {
            await _events.AddAsync(new EventEntity
            {
                CommandId = CommandScope.Current.Id,
                ExternalId = evt.Id,
                Name = typeof(TEvent).Name,
                Payload = JsonSerializer.Serialize(evt),
                CreatedOn = evt.CreatedOn,
                CreatedBy = evt.CreatedBy,
            }, ct);

            await next(evt, ct);
        }

        private class CommandScope : IDisposable
        {
            private CommandScope(Guid externalId, long id)
            {
                ExternalId = externalId;
                Id = id;

                AsyncLocal.Value = this;
            }

            public Guid ExternalId { get; }

            public long Id { get; }

            public void Dispose()
            {
                AsyncLocal.Value = null;
            }

            private static readonly AsyncLocal<CommandScope> AsyncLocal = new AsyncLocal<CommandScope>();

            public static CommandScope Current => AsyncLocal.Value;

            public static IDisposable Begin(Guid externalId, long id) => new CommandScope(externalId, id);
        }
    }
}
