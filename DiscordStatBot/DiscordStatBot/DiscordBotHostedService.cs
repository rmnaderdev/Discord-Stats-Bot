using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordStatBot
{
    public class DiscordBotHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly DiscordBotManager _discordBotManager;

        public DiscordBotHostedService(
            IHostApplicationLifetime hostApplicationLifetime,
            DiscordBotManager discordBotManager)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _discordBotManager = discordBotManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            _hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
            _hostApplicationLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _ = _discordBotManager.Login();


        }

        private void OnStopping()
        {
            _discordBotManager.Shutdown();
        }

        private void OnStopped()
        {
            
        }
    }
}
