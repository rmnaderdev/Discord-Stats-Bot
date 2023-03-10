using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordStatBot
{
    internal class CustomActivity : IActivity
    {
        public string Name => "Processing Taxes";

        public ActivityType Type => ActivityType.Playing;

        public ActivityProperties Flags => ActivityProperties.None;

        public string Details => "I am the tax mannn";
    }
}
