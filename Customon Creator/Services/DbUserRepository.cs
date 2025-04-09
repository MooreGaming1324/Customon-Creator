using Customon_Creator.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Customon_Creator.Services {
    public class DbUserRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager) {
        private readonly ApplicationDbContext _db = db;
        private readonly UserManager<ApplicationUser> _userManager = userManager;


    }
}
