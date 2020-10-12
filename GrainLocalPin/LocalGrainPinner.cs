using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Core;
using Orleans.Runtime;
using Orleans.Services;

namespace OrleansGrainPinner
{
    internal interface ILocalGrainPinner : IGrainService
    {}

    internal class LocalGrainPinner : GrainService, ILocalGrainPinner
    {
        private readonly IGrainFactory _grainFactory;

        private readonly IOptions<LocalPinOptions> _pinOptions;

        private readonly List<ILocalPinnedGrain> _pinnedGrains;

        private IDisposable _timerDisposer;

        public LocalGrainPinner(IGrainFactory grainFactory, IOptions<LocalPinOptions> options, IGrainIdentity id, Silo silo, ILoggerFactory loggerFactory) 
            : base(id, silo, loggerFactory)
        {
            _grainFactory = grainFactory ?? throw new ArgumentNullException(nameof(grainFactory));
            _pinOptions = options ?? throw new ArgumentNullException(nameof(options));

            _pinnedGrains = new List<ILocalPinnedGrain>();
        }

        public override async Task Start()
        {
            _pinnedGrains.Clear();

            foreach (var pinnedGrainType in _pinOptions.Value.PinnedGrainTypes)
                _pinnedGrains.Add((ILocalPinnedGrain)_grainFactory.GetGrain(pinnedGrainType, Guid.NewGuid()));

            foreach (var pinnedGrain in _pinnedGrains)
                await pinnedGrain.InitPin();

            if (_timerDisposer != null)
                ForgetTimer();

            _timerDisposer = RegisterTimer(KeepAlivePins, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        }

        public override Task Stop()
        {
            ForgetTimer();
            return base.Stop();
        }

        private async Task KeepAlivePins(object arg)
        {
            foreach (var pinnedGrain in _pinnedGrains)
                await pinnedGrain.KeepAlivePin();
        }

        private void ForgetTimer()
        {
            _timerDisposer?.Dispose();
            _timerDisposer = null;
        }
    }
}
