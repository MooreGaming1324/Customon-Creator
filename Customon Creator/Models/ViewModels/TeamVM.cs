using Customon_Creator.Models.Entities;
using System;

namespace Customon_Creator.Models.ViewModels {
    public class TeamVM {
        public ICollection<Pokemon>? PokemonList { get; set; }
        public ICollection<Move>? MoveList { get; set; }

    }
}
