using System.Threading.Tasks;
using Orleans;

namespace OrleansGrainPinner
{
    public interface ILocalPinnedGrain: IGrainWithGuidKey
    {
        Task InitPin();

        Task KeepAlivePin();
    }
}
