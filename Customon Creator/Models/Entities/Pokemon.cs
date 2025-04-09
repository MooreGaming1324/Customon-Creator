using Microsoft.EntityFrameworkCore;
using System;

namespace Customon_Creator.Models.Entities {
    public class Pokemon {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Type { get; set; } = "";
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpAtk { get; set; }
        public int SpDef {  get; set; }
        public int Speed { get; set; }

        // User can have many pokemon
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // pokemon can have many moves (M:N)
        public ICollection<PokemonMoveList> MoveList { get; set; } = new List<PokemonMoveList>();
    }
}
