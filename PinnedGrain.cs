using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Placement;

namespace OrleansGrainPinner
{
    public interface IPinnedGrain : ILocalPinnedGrain
    {
    }

    [PreferLocalPlacement]
    public class PinnedGrain : Grain, IPinnedGrain
    {
        private readonly ILogger _logger;

        public PinnedGrain(ILogger<PinnedGrain> logger) => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public override Task OnActivateAsync() 
        { 
            this.
            _logger.LogInformation("I'm activated!");
            return Task.CompletedTask;
        }

        public override Task OnDeactivateAsync() 
        { 
            _logger.LogInformation("I'm deactivated!");
            return Task.CompletedTask;
        }

        Task ILocalPinnedGrain.InitPin() => Task.CompletedTask;

        Task ILocalPinnedGrain.KeepAlivePin() => Task.CompletedTask;
    }
}
