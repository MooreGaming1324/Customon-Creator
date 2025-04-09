using Microsoft.EntityFrameworkCore;
using System;

namespace Customon_Creator.Models.Entities {
    [PrimaryKey(nameof(PokemonId), nameof(MoveId))]
    public class PokemonMoveList{

        public int PokemonId { get; set; }
        public Pokemon? Pokemon { get; set; }

        public int MoveId { get; set; }
        public Move? Move { get; set; }
    }
}
