using System;
using Orleans.Hosting;

namespace OrleansGrainPinner
{
    public interface ILocalPinBuilder
    {
        ILocalPinBuilder AddLocalPinnedGrain<TPinnedGrain>() where TPinnedGrain:ILocalPinnedGrain;
    }

    internal class LocalPinBuilder: ILocalPinBuilder
    {
        private static bool _pinnerAdded;

        private readonly ISiloBuilder _hostBuilder;

        public LocalPinBuilder(ISiloBuilder hostBuilder)
        {
            _hostBuilder = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));
            AddGrainPinner();
        }

        public ILocalPinBuilder AddLocalPinnedGrain<TPinnedGrain>() where TPinnedGrain:ILocalPinnedGrain
        {
            _hostBuilder.Configure<LocalPinOptions>(opt => opt.AddLocalPinnedGrain<TPinnedGrain>());
            return this;
        }
        
        private void AddGrainPinner()
        {
            if (_pinnerAdded)
                return;

            _hostBuilder.AddGrainService<LocalGrainPinner>();
            _pinnerAdded = true;
        }
    }
}
