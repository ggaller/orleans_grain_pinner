using Orleans.Hosting;

namespace OrleansGrainPinner
{
    public static class GrainLocalPinSiloHostBuilderExtensions
    {
        public static ILocalPinBuilder AddGrainPinner(this ISiloBuilder hostBuilder) =>
            new LocalPinBuilder(hostBuilder);
    }
}
