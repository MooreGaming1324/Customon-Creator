using Customon_Creator.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Customon_Creator.Services {
    public class DbUserRepository(ApplicationDbContext db) : IUserRepository {
        private readonly ApplicationDbContext _db = db;

        /// <summary>
        /// Returns the total number of users in the database
        /// </summary>
        /// <returns>number of users</returns>
        public int GetUserCount() {
           return _db.Users.Count();
        }

        /// <summary>
        /// Gets a list of pokemon belonging to a specific user based on userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>list of pokemon</returns>
        public async Task<ICollection<Pokemon>> GetPokemonAsync(string userId) {
            var user = await _db.Users.FindAsync(userId);
            if (user != null) {
                _db.Entry(user)
                    .Collection(p => p.Pokemon)
                    .Load();
            }
            return user!.Pokemon;
        }

        /// <summary>
        /// Gets a list of pokemon belonging to a specific user based on userId. Includes moves with the pokemon.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>list of pokemon and moves</returns>
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

        /// <summary>
        /// Gets a list of all users that have a valid team.
        /// </summary>
        /// <returns>list of users with team</returns>
        public async Task<ICollection<ApplicationUser>> GetAllTeamsAsync() {
            return await _db.Users
                .Include(u => u.Pokemon.Where(p => p.TeamSlot != null))
                .ThenInclude(p => p.MoveList)
                .ThenInclude(m => m.Move)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a list of pokemon belonging to a specific user based on userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>list of moves</returns>
        public async Task<ICollection<Move>> GetMovesAsync(string userId) {
            var user = await _db.Users.FindAsync(userId);
            if (user != null) {
                _db.Entry(user)
                    .Collection(p => p.Moves)
                    .Load();
            }
            return user!.Moves;
        }

        /// <summary>
        /// Gets a user with a specific username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>application user</returns>
        public async Task<ApplicationUser?> GetUserAsync(string username) {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
            return user;
        }

    }
}
