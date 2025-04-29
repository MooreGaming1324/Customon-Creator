using Customon_Creator.Models.Entities;
using System;

namespace Customon_Creator.Services {
    public interface IPokemonRepository {
        int GetPokemonCount();
        Task<ICollection<Pokemon>> ReadAllAsync();
        Task<Pokemon> CreateAsync(Pokemon newPokemon, List<int>? moveIds);
        Task<Pokemon?> ReadAsync(int id);
        Task<bool> UpdateAsync(string userId, int oldId, Pokemon pokemon, List<int>? moveIds);
        Task DeleteAsync(string userId, int id);
        Task UpdateTeamAsync(string userId, int[] pokemonIds);
    }
}