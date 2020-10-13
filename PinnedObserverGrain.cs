using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Placement;
using Orleans.Streams;

namespace OrleansGrainPinner
{
    public interface IPinnedObserverGrain : ILocalPinnedGrain
    { }

    public class StreamData { }

    [PreferLocalPlacement]
    public class PinnedObserverGrain : Grain, IPinnedGrain, IAsyncObserver<StreamData>
    {
        private readonly ILogger _logger;

        private StreamSubscriptionHandle<StreamData> _streamSubscription;

        public PinnedObserverGrain(ILogger<PinnedObserverGrain> logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public override async Task OnActivateAsync()
        {
            _logger.LogInformation("I'm activated!");

            var stream = GetStreamProvider("SomeProvider")
                                .GetStream<StreamData>(Guid.Empty, "SomeStream");

            // 
            if (await TryUnsubscribeIfSubscribed(stream))
                return;

            _streamSubscription = await stream.SubscribeAsync(this);
            _logger.LogInformation("I'm subscribed!");
        }

        public override async Task OnDeactivateAsync()
        {
            if (_streamSubscription != null)
            {
                await _streamSubscription.UnsubscribeAsync();
                _logger.LogInformation("I'm unsubscribed!");
            }

            _logger.LogInformation("I'm deactivated!");
        }

        private async Task<bool> TryUnsubscribeIfSubscribed(IAsyncStream<StreamData> stream)
        {
            var subscriptions = await stream.GetAllSubscriptionHandles();

            foreach (var subscription in subscriptions)
                await subscription.UnsubscribeAsync();

            return subscriptions.Count > 0;
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
        Task IAsyncObserver<StreamData>.OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        Task IAsyncObserver<StreamData>.OnErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }

        Task IAsyncObserver<StreamData>.OnNextAsync(StreamData item, StreamSequenceToken token)
        {
            throw new NotImplementedException();
        }
    }
}
