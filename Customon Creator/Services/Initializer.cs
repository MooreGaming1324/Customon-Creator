using Customon_Creator.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Customon_Creator.Services {
    public class Initializer {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public Initializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager) {
            _db = db;
            _userManager = userManager;
        }
        public async Task SeedDatabaseAsync() {
            _db.Database.EnsureCreated();
            // If there are any users then assume the database is already
            // seeded.
            if (_db.Users.Any()) return;
            var anon = new ApplicationUser { UserName = "Anonymous", Email = "anonymous@fakemail.com" };
            var testuser1 = new ApplicationUser { UserName = "TestUser1", Email = "testuser1@fakemail.com" };
            var testuser2 = new ApplicationUser { UserName = "TestUser2", Email = "testuser2@fakemail.com" };
            var testuser3 = new ApplicationUser { UserName = "TestUser3", Email = "testuser3@fakemail.com" };

            await _userManager.CreateAsync(anon);
            await _userManager.CreateAsync(testuser1);
            await _userManager.CreateAsync(testuser2);
            await _userManager.CreateAsync(testuser3);
            await _db.SaveChangesAsync();

            var pokemon = new List<Pokemon>
            {
                new() { Name = "TestMon1", Description = "This is TestUser1's first pokemon", Attack = 15, Defense = 20, HP = 100, SpAtk = 25, SpDef = 15, Speed = 40, Type1 = Pokemon.Type.Electric, Type2 = Pokemon.Type.Steel, TeamSlot = 1, UserId = testuser1.Id},
                new() { Name = "TestMon2", Description = "This is TestUser1's second pokemon", Attack = 25, Defense = 20, HP = 100, SpAtk = 25, SpDef = 15, Speed = 40, Type1 = Pokemon.Type.Fire, TeamSlot = 2, UserId = testuser1.Id},
                new() { Name = "TestMon3", Description = "This is TestUser1's third pokemon", Attack = 25, Defense = 20, HP = 100, SpAtk = 25, SpDef = 15, Speed = 40, Type1 = Pokemon.Type.Ice, Type2 = Pokemon.Type.Flying, TeamSlot = 3, UserId = testuser1.Id},
                new() { Name = "TestMon4", Description = "This is TestUser1's fourth pokemon", Attack = 25, Defense = 20, HP = 100, SpAtk = 25, SpDef = 15, Speed = 40, Type1 = Pokemon.Type.Dragon, Type2 = Pokemon.Type.Dark, TeamSlot = 4, UserId = testuser1.Id},
                new() { Name = "TestMon5", Description = "This is TestUser1's fifth pokemon", Attack = 25, Defense = 20, HP = 100, SpAtk = 25, SpDef = 15, Speed = 40, Type1 = Pokemon.Type.Psychic, Type2 = Pokemon.Type.Normal, TeamSlot = 5, UserId = testuser1.Id},
                new() { Name = "TestMon6", Description = "This is TestUser1's sixth pokemon", Attack = 25, Defense = 20, HP = 100, SpAtk = 25, SpDef = 15, Speed = 40, Type1 = Pokemon.Type.Fairy, Type2 = Pokemon.Type.Flying, TeamSlot = 6, UserId = testuser1.Id},
                new() { Name = "Test2Mon1", Description = "This is TestUser2's first pokemon", Attack = 25, Defense = 20, HP = 100, SpAtk = 25, SpDef = 15, Speed = 40, Type1 = Pokemon.Type.Rock, TeamSlot = 1, UserId = testuser2.Id},
                new() { Name = "Test2Mon2", Description = "This is TestUser2's second pokemon", Attack = 25, Defense = 20, HP = 100, SpAtk = 25, SpDef = 15, Speed = 40, Type1 = Pokemon.Type.Water, Type2 = Pokemon.Type.Dragon, TeamSlot = 2, UserId = testuser2.Id},
                new() { Name = "Test3Mon", Description = "This is TestUser3's only pokemon", Attack = 25, Defense = 20, HP = 100, SpAtk = 25, SpDef = 15, Speed = 40, Type1 = Pokemon.Type.Bug, TeamSlot = null, UserId = testuser3.Id}
            };
            await _db.Pokemon.AddRangeAsync(pokemon);
            await _db.SaveChangesAsync();

            var moves = new List<Move>
            {  
                new() { Name = "TestMove1", Description = "This is a test move used by TestUser1", Category = Move.DmgCategory.Physical, Power = 115, Accuracy = 95, PP = 5, Priority = 1, Type = Pokemon.Type.Flying, UserId = testuser1.Id},
                new() { Name = "TestMove2", Description = "This is another test move used by TestUser1", Category = Move.DmgCategory.Special, Power = 115, Accuracy = 100, PP = 25, Priority = 1, Type = Pokemon.Type.Dark, UserId = testuser1.Id},
                new() { Name = "TestMove3", Description = "This is a third test move used by TestUser1", Category = Move.DmgCategory.Physical, Power = 150, Accuracy = 75, PP = 6, Priority = 1, Type = Pokemon.Type.Psychic, UserId = testuser1.Id},
                new() { Name = "TestMove4", Description = "This is a 4th test move used by TestUser1", Category = Move.DmgCategory.Status, Power = 115, Accuracy = 95, PP = 10, Priority = 2, Type = Pokemon.Type.Fighting, UserId = testuser1.Id},
                new() { Name = "Test2Move", Description = "This is the only move created by TestUser2", Category = Move.DmgCategory.Special, Power = 200, Accuracy = 100, PP = 50, Priority = 5, Type = Pokemon.Type.Steel, UserId = testuser2.Id}
            };
            await _db.Moves.AddRangeAsync(moves);
            await _db.SaveChangesAsync();

            var pokemonmoves = new List<PokemonMoveList>
            {
                new() { PokemonId = 1, MoveId = 1 },
                new() { PokemonId = 1, MoveId = 2 },
                new() { PokemonId = 1, MoveId = 3 },
                new() { PokemonId = 1, MoveId = 4 },
                new() { PokemonId = 2, MoveId = 3 },
                new() { PokemonId = 2, MoveId = 2 },
                new() { PokemonId = 3, MoveId = 1 },
                new() { PokemonId = 4, MoveId = 4 },
                new() { PokemonId = 5, MoveId = 2 },
                new() { PokemonId = 5, MoveId = 4 },
                new() { PokemonId = 6, MoveId = 1 },
                new() { PokemonId = 6, MoveId = 3 },
                new() { PokemonId = 6, MoveId = 2 },
                new() { PokemonId = 7, MoveId = 5 },
                new() { PokemonId = 8, MoveId = 5 },
            };
            await _db.PokemonMoveList.AddRangeAsync(pokemonmoves);
            await _db.SaveChangesAsync();
        }
    }
}
