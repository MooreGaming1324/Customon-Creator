using Customon_Creator.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Customon_Creator.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {
        }
        public DbSet<Pokemon> Pokemon => Set<Pokemon>();
        public DbSet<Move> Moves => Set<Move>();
        public DbSet<PokemonMoveList> PokemonMoveList => Set<PokemonMoveList>();

    }
}
