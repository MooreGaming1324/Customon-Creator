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
        public async Task<bool> UpdateAsync(string userid ,int oldId, Pokemon pokemon) {
            Pokemon? pokemonToUpdate = await ReadAsync(oldId);
            if (pokemonToUpdate != null) {
                if (userid != pokemonToUpdate.UserId) {
                    return false;
                }
                pokemonToUpdate.Name = pokemon.Name;
                pokemonToUpdate.Description = pokemon.Description;
                pokemonToUpdate.Type1 = pokemon.Type1;
                pokemonToUpdate.Type2 = pokemon.Type2;
                pokemonToUpdate.HP = pokemon.HP;
                pokemonToUpdate.Attack = pokemon.Attack;
                pokemonToUpdate.Defense = pokemon.Defense;
                pokemonToUpdate.SpAtk = pokemon.SpAtk;
                pokemonToUpdate.SpDef = pokemon.SpDef;
                pokemonToUpdate.Speed = pokemon.Speed;
                await _db.SaveChangesAsync();
            }
            return true;
        }
        public async Task DeleteAsync(string userId, int id) {
            Pokemon? pokemonToDelete = await ReadAsync(id);
            if (pokemonToDelete != null && userId == pokemonToDelete.UserId) {
                _db.Pokemon.Remove(pokemonToDelete);
                await _db.SaveChangesAsync();
            }
        }
    }
}
