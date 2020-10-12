using System;
using System.Threading.Tasks;
using Orleans.Hosting;
using Orleans;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OrleansGrainPinner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder.UseLocalhostClustering()
                            .AddGrainPinner()
                            .AddLocalPinnedGrain<IPinnedGrain>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    
                })
                .UseConsoleLifetime()
                .Build();

                await host.RunAsync();
        }
    }
}
