using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public interface IProfileService
    {
        Task<Profile> GetOrCreateProfileAsync(ulong discordId);
    }
    public class ProfileService : IProfileService
    {
        private readonly RPGContext _context;
        int spawnX = 10;
        int spawnY = 0;

        public ProfileService(RPGContext context)
        {
            _context = context;
        }

        public async Task<Profile> GetOrCreateProfileAsync(ulong discordId)
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync(x => x.DiscordId == discordId).ConfigureAwait(false);

            if (profile != null)
            {
                return profile;
            }

            profile = new Profile
            {
                DiscordId = discordId
            };

            profile.PosX = spawnX;
            profile.PosY = spawnY;

            _context.Add(profile);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return profile;
        }
    }
}
