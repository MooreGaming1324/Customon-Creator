using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System;

namespace Customon_Creator.Models.Entities {
    public class Move {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public Pokemon.Type Type { get; set; }
        public DmgCategory Category { get; set; }
        public int Power { get; set; }
        public int Accuracy { get; set; }
        public int PP { get; set; }
        public int Priority { get; set; }

        // User can have many pokemon
        public string UserId { get; set; } = "";
        public ApplicationUser? User { get; set; }

        // A move can belong to many pokemon (M:N)
        public ICollection<PokemonMoveList> PokemonList { get; set; } = new List<PokemonMoveList>();

        public enum DmgCategory {
            Physical,
            Special,
            Status
        }
    }
}
