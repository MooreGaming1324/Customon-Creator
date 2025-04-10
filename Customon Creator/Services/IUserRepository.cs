using Customon_Creator.Models.Entities;
using System;

namespace Customon_Creator.Services {
    public interface IUserRepository {
        Task<ICollection<Pokemon>> GetPokemonAsync(string userId);
        Task<ICollection<Move>> GetMovesAsync(string userId);


    }
}