using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordStatBot
{
    public class DiscordBotManager
    {
        private readonly ILogger<DiscordBotManager> _logger;
        private readonly IConfiguration _configuration;

        private readonly DiscordSocketClient _client;

        public DiscordBotManager(ILogger<DiscordBotManager> logger, IConfiguration configuration) 
        {
            _logger = logger;
            _configuration = configuration;

            var discordConfig = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildPresences
            };

            _client = new DiscordSocketClient(discordConfig);

            _ = _client.SetActivityAsync(new CustomActivity());

            _client.Log += BotLog;
            _client.Ready += OnBotReady;
            _client.UserIsTyping += UserTyping;
            _client.PresenceUpdated += PresenceUpdated;
        }

        private Task PresenceUpdated(SocketUser user, SocketPresence beforePresence, SocketPresence afterPresence)
        {
            if (beforePresence.Status != afterPresence.Status)
            {
                _logger.LogInformation($"{user.Username}#{user.Discriminator} presence status has updated from {beforePresence.Status} to {afterPresence.Status}");
            }

            return Task.CompletedTask;
        }

        private Task BotLog(LogMessage message)
        {
            _logger.LogInformation(message.ToString());

            return Task.CompletedTask;
        }

        private Task OnBotReady()
        {
            _logger.LogInformation("Discord Bot Ready!");

            return Task.CompletedTask;
        }

        private async Task UserTyping(Cacheable<IUser, ulong> user, Cacheable<IMessageChannel, ulong> channel)
        {
            var latestUser = await user.GetOrDownloadAsync();
            var latestChannel = await channel.GetOrDownloadAsync();
            _logger.LogInformation($"{latestUser.Username}#{latestUser.Discriminator} is typing in {latestChannel.Name}");
        }

        public async Task Login()
        {
            await _client.LoginAsync(TokenType.Bot, _configuration["Discord:BotToken"]);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        
        public void Shutdown()
        {
            _client.StopAsync().GetAwaiter().GetResult();
        }
    }
}
