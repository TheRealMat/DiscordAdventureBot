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

        public ProfileService(RPGContext context)
        {
            _context = context;
        }

        public async Task<Profile> GetOrCreateProfileAsync(ulong discordId)
        {
            // Probably shouldn't include tile every time we get profile
            var profile = await _context.Profiles.Include(T => T.CurrentTile) .FirstOrDefaultAsync(x => x.DiscordId == discordId).ConfigureAwait(false);

            if (profile != null)
            {
                return profile;
            }

            profile = new Profile
            {
                DiscordId = discordId,
            };

            _context.Add(profile);

            await _context.SaveChangesAsync().ConfigureAwait(false);
            _context.Entry(profile).State = EntityState.Detached;

            return profile;
        }

    }
}
