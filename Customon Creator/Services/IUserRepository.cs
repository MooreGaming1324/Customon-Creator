using Customon_Creator.Models.Entities;
using System;

namespace Customon_Creator.Services {
    public interface IUserRepository {
        int GetUserCount();

        Task<ICollection<Pokemon>> GetPokemonAsync(string userId);
        Task<ICollection<Move>> GetMovesAsync(string userId);
        Task<ICollection<ApplicationUser>> GetAllTeamsAsync();
        Task<ICollection<Pokemon>> GetPokemonWithMovesAsync(string userId);


        }
    }