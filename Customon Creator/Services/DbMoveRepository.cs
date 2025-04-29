using Customon_Creator.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Customon_Creator.Services {
    public class DbMoveRepository(ApplicationDbContext db) : IMoveRepository {
        private readonly ApplicationDbContext _db = db;


        public int GetMoveCount() {
            return _db.Moves.Count();
        }
        public async Task<ICollection<Move>> ReadAllAsync() {
            return await _db.Moves
                .Include(p => p.User)
                .ToListAsync();
        }
        public async Task<Move> CreateAsync(Move newMove) {
            await _db.Moves.AddAsync(newMove);
            await _db.SaveChangesAsync();
            return newMove;
        }
        public async Task<Move?> ReadAsync(int id) {
            return await _db.Moves.FindAsync(id);
        }
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
        public async Task DeleteAsync(string userId, int id) {
            Move? moveToDelete = await ReadAsync(id);
            if (moveToDelete != null && userId == moveToDelete.UserId) {
                _db.Moves.Remove(moveToDelete);
                await _db.SaveChangesAsync();
            }
        }
    }
}
