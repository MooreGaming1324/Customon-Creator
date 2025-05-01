using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Customon_Creator.Models.Entities {
    public class Pokemon {
        public int Id { get; set; }
        [StringLength(64)]
        public string Name { get; set; } = "";
        [StringLength(512)]
        public string Description { get; set; } = "";
        public Type Type1 { get; set; }
        public Type? Type2 { get; set; }
        [Range(1, 500)]
        public int HP { get; set; }
        [Range(1, 500)]
        public int Attack { get; set; }
        [Range(1, 500)]
        public int Defense { get; set; }
        [Range(1, 500)]
        public int SpAtk { get; set; }
        [Range(1, 500)]
        public int SpDef {  get; set; }
        [Range(1, 500)]
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
