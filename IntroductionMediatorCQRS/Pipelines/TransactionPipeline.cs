using System;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Pipelines
{
    public class TransactionPipeline : Pipeline
    {
        private readonly ApiDbContext _context;

        public TransactionPipeline(ApiDbContext context)
        {
            _context = context;
        }

        public override async Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct)
        {
            await using var tx = await _context.Database.BeginTransactionAsync(ct);

            await next(cmd, ct);

            await _context.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }

        public override async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct)
        {
            await using var tx = await _context.Database.BeginTransactionAsync(ct);

            var result = await next(cmd, ct);

            await _context.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return result;
        }

        //public override async Task<TResult> OnQueryAsync<TQuery, TResult>(Func<TQuery, CancellationToken, Task<TResult>> next, TQuery query, CancellationToken ct)
        //{
        //    await using var tx = await _context.Database.BeginTransactionAsync(ct);

        //    var result = await next(query, ct);

        //    await tx.RollbackAsync(ct);

        //    return result;
        //}
    }
}
