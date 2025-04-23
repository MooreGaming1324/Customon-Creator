using Customon_Creator.Models.Entities;
using System;

namespace Customon_Creator.Models.ViewModels {
    public class PokemonVM {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public Pokemon.Type Type1 { get; set; }
        public Pokemon.Type? Type2 { get; set; }

        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpAtk { get; set; }
        public int SpDef { get; set; }
        public int Speed { get; set; }

        public List<Move> MoveIds { get; set; } = new List<Move>();

        public Pokemon GetPokemonInstance() {
            return new Pokemon {
                Id = 0,
                Name = "TBD",
                Description = this.Description,
                Type1 = this.Type1,
                Type2 = this.Type2,
                HP = this.HP,
                Attack = this.Attack,
                Defense = this.Defense,
                SpAtk = this.SpAtk,
                SpDef = this.SpDef,
                Speed = this.Speed,
            };
        }
    }
}
