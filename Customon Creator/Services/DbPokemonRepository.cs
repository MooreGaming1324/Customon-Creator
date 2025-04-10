using Customon_Creator.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Customon_Creator.Services {
    public class DbPokemonRepository(ApplicationDbContext db) : IPokemonRepository {
        private readonly ApplicationDbContext _db = db;

        public async Task<ICollection<Pokemon>> ReadAllAsync() {
            return await _db.Pokemon.ToListAsync();
        }
        public async Task<Pokemon> CreateAsync(Pokemon newPokemon) {
            await _db.Pokemon.AddAsync(newPokemon);
            await _db.SaveChangesAsync();
            return newPokemon;
        }
        public async Task<Pokemon?> ReadAsync(int id) {
            return await _db.Pokemon.FindAsync(id);
        }
        public async Task UpdateAsync(int oldId, Pokemon pokemon) {
            Pokemon? pokemonToUpdate = await ReadAsync(oldId);
            if (pokemonToUpdate != null) {
                pokemonToUpdate.Name = pokemon.Name;
                pokemonToUpdate.Description = pokemon.Description;
                pokemonToUpdate.Type = pokemon.Type;
                pokemonToUpdate.HP = pokemon.HP;
                pokemonToUpdate.Attack = pokemon.Attack;
                pokemonToUpdate.Defense = pokemon.Defense;
                pokemonToUpdate.SpAtk = pokemon.SpAtk;
                pokemonToUpdate.SpDef = pokemon.SpDef;
                pokemonToUpdate.Speed = pokemon.Speed;
                await _db.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(int id) {
            Pokemon? pokemonToDelete = await ReadAsync(id);
            if (pokemonToDelete != null) {
                _db.Pokemon.Remove(pokemonToDelete);
                await _db.SaveChangesAsync();
            }
        }
    }
}
