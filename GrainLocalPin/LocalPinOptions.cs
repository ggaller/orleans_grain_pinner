using System;
using System.Collections.Generic;

namespace OrleansGrainPinner
{
    public class LocalPinOptions
    {
        private readonly List<Type> _pinnedGrainTypes;

        public IEnumerable<Type> PinnedGrainTypes => _pinnedGrainTypes;

        public LocalPinOptions() => _pinnedGrainTypes = new List<Type>();

        public void AddLocalPinnedGrain<TPinnedGrain>() where TPinnedGrain : ILocalPinnedGrain
            => _pinnedGrainTypes.Add(typeof(TPinnedGrain));
    }
}