using Customon_Creator.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Customon_Creator.Services {
    public class DbUserRepository(ApplicationDbContext db) : IUserRepository {
        private readonly ApplicationDbContext _db = db;


        public int GetUserCount() {
           return _db.Users.Count();
        }

        public async Task<ICollection<Pokemon>> GetPokemonAsync(string userId) {
            var user = await _db.Users.FindAsync(userId);
            if (user != null) {
                _db.Entry(user)
                    .Collection(p => p.Pokemon)
                    .Load();
            }
            return user!.Pokemon;
        }

        public async Task<ICollection<Pokemon>> GetPokemonWithMovesAsync(string userId) {
            var user = await _db.Users.FindAsync(userId);
            if (user != null) {
                _db.Entry(user)
                    .Collection(p => p.Pokemon)
                    .Query()
                    .Include(p => p.MoveList)
                        .ThenInclude(m => m.Move)
                    .Load();
            }
            return user!.Pokemon;
        }

        public async Task<ICollection<ApplicationUser>> GetAllTeamsAsync() {
            return await _db.Users
                .Include(u => u.Pokemon.Where(p => p.TeamSlot != null))
                .ThenInclude(p => p.MoveList)
                .ThenInclude(m => m.Move)
                .ToListAsync();
        }

        public async Task<ICollection<Move>> GetMovesAsync(string userId) {
            var user = await _db.Users.FindAsync(userId);
            if (user != null) {
                _db.Entry(user)
                    .Collection(p => p.Moves)
                    .Load();
            }
            return user!.Moves;
        }
    }
}
