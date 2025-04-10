using Customon_Creator.Models.Entities;
using System;

namespace Customon_Creator.Services {
    public interface IPokemonRepository {
        Task<ICollection<Pokemon>> ReadAllAsync();
        Task<Pokemon> CreateAsync(Pokemon newPokemon);
        Task<Pokemon?> ReadAsync(int id);
        Task UpdateAsync(int oldId, Pokemon pokemon);
        Task DeleteAsync(int id);
    }
}