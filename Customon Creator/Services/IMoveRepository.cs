using Customon_Creator.Models.Entities;
using System;

namespace Customon_Creator.Services {
    public interface IMoveRepository {
        int GetMoveCount();
        Task<ICollection<Move>> ReadAllAsync();
        Task<Move> CreateAsync(Move newMove);
        Task<Move?> ReadAsync(int id);
        Task<bool> UpdateAsync(string userId, int oldId, Move move);
        Task DeleteAsync(string userId, int id);
    }
}