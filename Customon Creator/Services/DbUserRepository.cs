using Customon_Creator.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace Customon_Creator.Services {
    public class DbUserRepository(ApplicationDbContext db) : IUserRepository {
        private readonly ApplicationDbContext _db = db;

        public async Task<ICollection<Pokemon>> GetPokemonAsync(string userId) {
            var user = await _db.Users.FindAsync(userId);
            if (user != null) {
                _db.Entry(user)
                    .Collection(p => p.Pokemon)
                    .Load();
            }
            return user!.Pokemon;
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
