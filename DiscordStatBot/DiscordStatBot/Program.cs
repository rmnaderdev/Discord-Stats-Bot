using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordStatBot
{
    public class Program
    {
        private IHost AppHost { get; }
        public IHost GetHost() => AppHost;

        static void Main(string[] args) => new Program(args).MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            await AppHost.RunAsync();
        }

        public Program(string[] args)
        {
            AppHost = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<DiscordBotManager>();
                    services.AddHostedService<DiscordBotHostedService>();
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddUserSecrets<Program>();
                })
                .Build();
        }
    }
}