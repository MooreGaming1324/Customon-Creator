using Customon_Creator.Models.Entities;
using Customon_Creator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Customon_Creator.Controllers {
    [Authorize]
    public class PokemonController(UserManager<ApplicationUser> userManager, IUserRepository userRepo, IPokemonRepository pokemonRepo) : Controller {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserRepository _userRepo = userRepo;

        private readonly IPokemonRepository _pokemonRepo = pokemonRepo;

        public async Task<IActionResult> Index() {
            var user = await _userManager.GetUserAsync(User);
            return View(await _userRepo.GetPokemonAsync(user!.Id));
        }

        public async Task<IActionResult> All() {
            return View(await _pokemonRepo.ReadAllAsync());
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pokemon newPokemon) {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid) {
                newPokemon.UserId = user!.Id;
                await _pokemonRepo.CreateAsync(newPokemon);
                return RedirectToAction("Index");
            }
            return View(newPokemon);
        }
    }
}
