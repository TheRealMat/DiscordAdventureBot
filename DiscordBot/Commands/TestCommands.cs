using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    class TestCommands : BaseCommandModule
    {
        [Command("test")]
        [Description("Returns a message")]
        public async Task Test(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("fuck you").ConfigureAwait(false);
        }

        [Command("powerlich")]
        [Description("Returns a powerlich")]
        public async Task Powerlich(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("<:powerlich:818391341163348008>").ConfigureAwait(false);
        }

        [Command("emojis")]
        [Description("Returns all emojis in guild")]
        public async Task Emojis(CommandContext ctx)
        {
            IEnumerable<DiscordEmoji> emojis;
            emojis = ctx.Guild.Emojis.Values;
            string message = "";

            foreach (DiscordEmoji emoji in emojis)
            {
                message += emoji.ToString();
            }

            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }

        [Command("roll")]
        [Description("rolls custom dice")]
        public async Task Roll(CommandContext ctx, [Description("Die face amount")] int faces, [Description("Die amount")] int number)
        {
            Random random = new Random();
            string message = "";
            int total = 0;
            for (int i = 0; i < number; i++)
            {
                int result = random.Next(1, faces + 1);
                total += result;
                message += result.ToString();
                message += ", ";
            }
            message += Environment.NewLine;
            message += "Total: ";
            message += total.ToString();

            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }
    }
}
