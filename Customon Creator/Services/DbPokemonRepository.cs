using Customon_Creator.Models.Entities;
using Customon_Creator.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Customon_Creator.Services {
    public class DbPokemonRepository(ApplicationDbContext db) : IPokemonRepository {
        private readonly ApplicationDbContext _db = db;

        /// <summary>
        /// Returns the total number of pokemon in the database
        /// </summary>
        /// <returns>number of moves</returns>
        public int GetPokemonCount() {
            return _db.Pokemon.Count();
        }

        /// <summary>
        /// Returns all pokemon along with their associated user
        /// </summary>
        /// <returns>list of pokemon</returns>
        public async Task<ICollection<Pokemon>> ReadAllAsync() {
            return await _db.Pokemon
                .Include(p => p.User)
                .ToListAsync();
        }

        /// <summary>
        /// Creates a pokemon and adds it to the database
        /// </summary>
        /// <param name="newPokemon"></param>
        /// <param name="moveIds"></param>
        /// <returns>pokemon with updated Id and data</returns>
        public async Task<Pokemon> CreateAsync(Pokemon newPokemon, List<int>? moveIds) {
            await _db.Pokemon.AddAsync(newPokemon);
            if (moveIds != null) {
                foreach (var moveId in moveIds) {
                    if (moveId == 0) { continue; }
                    var move = await _db.Moves.FindAsync(moveId);
                    if (move != null) {
                        var pokemonmove = new PokemonMoveList {
                            Pokemon = newPokemon,
                            Move = move
                        };
                        newPokemon.MoveList.Add(pokemonmove);
                        move.PokemonList.Add(pokemonmove);
                    }
                }
            }
            await _db.SaveChangesAsync();
            return newPokemon;
        }

        /// <summary>
        /// Retrieves a singular pokemon from pokemon Id. Includes moves
        /// </summary>
        /// <param name="id"></param>
        /// <returns>single pokemon</returns>
        public async Task<Pokemon?> ReadAsync(int id) {
            return await _db.Pokemon
                .Include(p => p.MoveList)
                .ThenInclude(m => m.Move)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Updates a pokemon, requires authorization to modify the pokemon.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="oldId"></param>
        /// <param name="pokemon"></param>
        /// <param name="moveIds"></param>
        /// <returns>boolean value if pokemon was updated properly</returns>
        public async Task<bool> UpdateAsync(string userid, int oldId, Pokemon pokemon, List<int>? moveIds = null) {
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

                if (moveIds != null) {
                    await ResetMovesAsync(pokemon.Id, moveIds);
                }

                await _db.SaveChangesAsync();
            }
            return true;
        }

        /// <summary>
        /// Deletes a pokemon, requires authorization to modify the pokemon.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string userId, int id) {
            Pokemon? pokemonToDelete = await ReadAsync(id);
            if (pokemonToDelete != null && userId == pokemonToDelete.UserId) {
                _db.Pokemon.Remove(pokemonToDelete);
                await _db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes all pokemon from the team, then reassignes based on pokemonIds
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pokemonIds"></param>
        /// <returns></returns>
        public async Task UpdateTeamAsync(string userId, int[] pokemonIds) {
            for (int i = 0; i < 6; i++) {
                if (pokemonIds[i] != 0) {
                    Pokemon? mon = await _db.Pokemon.FindAsync(pokemonIds[i]);
                    if (mon != null) { mon.TeamSlot = i+1; }
                } else {
                    Pokemon? mon = await _db.Pokemon.FirstOrDefaultAsync(p => p.TeamSlot == i+1);
                    if (mon != null) { mon.TeamSlot = null; }
                }
            }
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Private internal method for removing the moves from a pokemon.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="moveIds"></param>
        /// <returns></returns>
        private async Task ResetMovesAsync(int id, List<int> moveIds) {
            var pokemon = await ReadAsync(id);
            var moves = pokemon!.MoveList;
            //remove all old moves
            foreach (var move in moves.ToList()) {
                pokemon.MoveList.Remove(move);
                move.Move!.PokemonList.Remove(move);
            }
            //add new moves
            foreach (var moveId in moveIds) {
                if (moveId == 0) { continue; }
                var move = await _db.Moves.FindAsync(moveId);
                if (move != null && pokemon != null) {
                    var pokemonmove = new PokemonMoveList {
                        Pokemon = pokemon,
                        Move = move
                    };
                    pokemon.MoveList.Add(pokemonmove);
                    move.PokemonList.Add(pokemonmove);
                }
            }
            await _db.SaveChangesAsync();
        }


    }
}
