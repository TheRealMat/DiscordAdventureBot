using DiscordBot.Models;
using DiscordBot.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DSharpPlus.Entities.DiscordEmbedBuilder;

namespace DiscordBot.Commands
{
	public class ProfileCommands : BaseCommandModule
	{
		private readonly IProfileService _profileService;

		public ProfileCommands(IProfileService profileService)
		{
			_profileService = profileService;
		}


		[Command("profile")]
		public async Task Profile(CommandContext ctx)
		{
			await GetProfileToDisplayAsync(ctx, ctx.Member.Id);
		}

		[Command("profile")]
		public async Task Profile(CommandContext ctx, DiscordMember member)
		{
			await GetProfileToDisplayAsync(ctx, member.Id);
		}

		private async Task GetProfileToDisplayAsync(CommandContext ctx, ulong memberId)
		{
			Profile profile = await _profileService.GetOrCreateProfileAsync(memberId).ConfigureAwait(false);

			DiscordMember member = await ctx.Guild.GetMemberAsync(profile.DiscordId).ConfigureAwait(false);


			EmbedThumbnail thumbnail = new EmbedThumbnail();
			thumbnail.Url = member.AvatarUrl;

			var profileEmbed = new DiscordEmbedBuilder
			{
				Title = $"{member.DisplayName}'s profile",
				Thumbnail = thumbnail
			};
			profileEmbed.AddField("Skills", "Strength: 7\nConstitution: 5\nCrafting: 2"); // just for show
			profileEmbed.AddField("Status", "😎");

			await ctx.Channel.SendMessageAsync(embed: profileEmbed).ConfigureAwait(false);
		}

	}
}
