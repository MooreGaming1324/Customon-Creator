using Customon_Creator.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Customon_Creator.Services {
    public class DbMoveRepository(ApplicationDbContext db) : IMoveRepository {
        private readonly ApplicationDbContext _db = db;

        /// <summary>
        /// Returns the total number of moves in the database
        /// </summary>
        /// <returns>number of moves</returns>
        public int GetMoveCount() {
            return _db.Moves.Count();
        }

        /// <summary>
        /// Returns all moves along with their associated user
        /// </summary>
        /// <returns>list of moves</returns>
        public async Task<ICollection<Move>> ReadAllAsync() {
            return await _db.Moves
                .Include(m => m.User)
                .ToListAsync();
        }

        /// <summary>
        /// Creates a move and adds it to the database
        /// </summary>
        /// <param name="newMove"></param>
        /// <returns>move with updated Id and data</returns>
        public async Task<Move> CreateAsync(Move newMove) {
            await _db.Moves.AddAsync(newMove);
            await _db.SaveChangesAsync();
            return newMove;
        }

        /// <summary>
        /// Retrieves a singular move from move Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>single move</returns>
        public async Task<Move?> ReadAsync(int id) {
            return await _db.Moves.FindAsync(id);
        }

        /// <summary>
        /// Retrieves a singular move along with associated user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>single move and user</returns>
        public async Task<Move?> ReadWithUserAsync(int id) {
            return await _db.Moves
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <summary>
        /// Updates a move, requires authorization to modify the move.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="oldId"></param>
        /// <param name="move"></param>
        /// <returns>boolean value if move was updated properly</returns>
        public async Task<bool> UpdateAsync(string userid ,int oldId, Move move) {
            Move? moveToUpdate = await ReadAsync(oldId);
            if (moveToUpdate != null) {
                if (userid != moveToUpdate.UserId) {
                    return false;
                }
                moveToUpdate.Name = move.Name;
                moveToUpdate.Description = move.Description;
                moveToUpdate.Type = move.Type;
                moveToUpdate.Category = move.Category;
                moveToUpdate.Power = move.Power;
                moveToUpdate.Accuracy = move.Accuracy;
                moveToUpdate.PP = move.PP;
                moveToUpdate.Priority = move.Priority;
                await _db.SaveChangesAsync();
            }
            return true;
        }

        /// <summary>
        /// Deletes a move, requires authorization to modify the move.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string userId, int id) {
            Move? moveToDelete = await ReadAsync(id);
            if (moveToDelete != null && userId == moveToDelete.UserId) {
                _db.Moves.Remove(moveToDelete);
                await _db.SaveChangesAsync();
            }
        }
    }
}
