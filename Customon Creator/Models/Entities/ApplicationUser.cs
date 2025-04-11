using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customon_Creator.Models.Entities {
    public class ApplicationUser : IdentityUser {

        // A user can create many pokemon
        public ICollection<Pokemon> Pokemon { get; set; } = new List<Pokemon>();
        // A user can create many moves
        public ICollection<Move> Moves { get; set; } = new List<Move>();

        [NotMapped]
        public Pokemon[] Team => Pokemon
            .Where(p => p.TeamSlot != null)
            .OrderBy(p => p.TeamSlot)
            .ToArray();

        public bool IsOwner(Pokemon pokemon) {
            if (this.Id == pokemon.UserId) return true;
            return false;
        }
        public bool IsOwner(Move move) {
            if (this.Id == move.UserId) return true;
            return false;
        }
    }
}
