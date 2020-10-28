using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class CreatedProductEventHandler : IEventHandler<CreatedProductEvent>
    {
        private readonly ILogger<CreatedProductEventHandler> _logger;

        public CreatedProductEventHandler(ILogger<CreatedProductEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(CreatedProductEvent evt, CancellationToken ct)
        {
            _logger.LogInformation("The product '{externalId}' has been created", evt.ExternalId);

            return Task.CompletedTask;
        }
    }
}