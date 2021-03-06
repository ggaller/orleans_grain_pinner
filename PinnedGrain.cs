﻿using System;
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

        public PinnedGrain(ILogger<PinnedGrain> logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public override Task OnActivateAsync()
        {
            _logger.LogInformation("I'm activated!");
            return Task.CompletedTask;
        }

        public override Task OnDeactivateAsync()
        {
            _logger.LogInformation("I'm deactivated!");
            return Task.CompletedTask;
        }

        Task ILocalPinnedGrain.InitPin()
        {
            _logger.LogInformation("I'm pinned!");
            return Task.CompletedTask;
        }

        Task ILocalPinnedGrain.KeepAlivePin()
        {
            _logger.LogInformation("I'm still alive!");
            return Task.CompletedTask;
        }
    }
}
