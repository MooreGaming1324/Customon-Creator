using Microsoft.EntityFrameworkCore;
using System;

namespace Customon_Creator.Models.Entities {
    public class Pokemon {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public Type Type1 { get; set; }
        public Type? Type2 { get; set; }

        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpAtk { get; set; }
        public int SpDef {  get; set; }
        public int Speed { get; set; }

        // Null means not on team
        public int? TeamSlot { get; set; } 

        // User can have many pokemon
        public string UserId { get; set; } = "";
        public ApplicationUser? User { get; set; }

        // pokemon can have many moves (M:N)
        public ICollection<PokemonMoveList> MoveList { get; set; } = new List<PokemonMoveList>();

        public enum Type {
            Normal,
            Fire,
            Water,
            Grass,
            Electric,
            Fighting,
            Flying,
            Poison,
            Ground,
            Rock,
            Bug,
            Psychic,
            Ice,
            Dragon,
            Dark,
            Ghost,
            Fairy,
            Steel
        }
    }
}
