using Microsoft.AspNetCore.Identity;

namespace Customon_Creator.Models.Entities {
    public class ApplicationUser : IdentityUser {

        // A user can create many pokemon
        public ICollection<Pokemon> Pokemon { get; set; } = new List<Pokemon>();
        // A user can create many moves
        public ICollection<Move> Moves { get; set; } = new List<Move>();

    }
}
