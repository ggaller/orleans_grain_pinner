using Orleans.Hosting;

namespace OrleansGrainPinner
{
    public static class GrainLocalPinSiloHostBuilderExtensions
    {
        private const string GrainPinnerPropertyName = nameof(LocalGrainPinner);

        public static ISiloBuilder AddLocalPinnedGrain<TPinnedGrain>(this ISiloBuilder hostBuilder)
                                                                            where TPinnedGrain : ILocalPinnedGrain
        {
            if(!hostBuilder.Properties.ContainsKey(GrainPinnerPropertyName))
            {
                hostBuilder.AddGrainService<LocalGrainPinner>();
                hostBuilder.Properties[GrainPinnerPropertyName] = true;
            }

            hostBuilder.Configure<LocalPinOptions>(opt => opt.AddLocalPinnedGrain<TPinnedGrain>());
            return hostBuilder;
        }
    }
}
