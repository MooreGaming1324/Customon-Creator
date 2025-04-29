using Customon_Creator.Models.Entities;
using Customon_Creator.Models.ViewModels;
using Customon_Creator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [AllowAnonymous]
        public async Task<IActionResult> All() {
            return View(await _pokemonRepo.ReadAllAsync());
        }

        public async Task<IActionResult> Create() {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.Moves = new SelectList(await _userRepo.GetMovesAsync(user!.Id), "Id", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pokemon newPokemon, List<int> moveIds) {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid) {
                newPokemon.UserId = user!.Id;
                await _pokemonRepo.CreateAsync(newPokemon, moveIds);
                return RedirectToAction("Index");
            }
            ViewBag.Moves = new SelectList(await _userRepo.GetMovesAsync(user!.Id), "Id", "Name");
            return View(newPokemon);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id) {
            var user = await _userManager.GetUserAsync(User);
            var pokemon = await _pokemonRepo.ReadAsync(id);
            ViewBag.isOwner = false;
            if (pokemon == null) {
                return RedirectToAction("Index");
            }
            if (user != null) {
                ViewBag.isOwner = user!.IsOwner(pokemon!);
            }
            return View(pokemon);
        }

        public async Task<IActionResult> Edit(int id) {
            var user = await _userManager.GetUserAsync(User);
            var pokemon = await _pokemonRepo.ReadAsync(id);
            if (pokemon == null || !user!.IsOwner(pokemon)) {
                return RedirectToAction("Index");
            }
            ViewBag.Moves = new SelectList(await _userRepo.GetMovesAsync(user!.Id), "Id", "Name");
            //ViewBag.Moves = await _userRepo.GetMovesAsync(user!.Id);
            return View(pokemon);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Pokemon pokemon, List<int> moveIds) {
            var user = await _userManager.GetUserAsync(User);
            //Check if the user is the proper owner of original unedited pokemon
            if (ModelState.IsValid && await _pokemonRepo.UpdateAsync(user!.Id, pokemon.Id, pokemon, moveIds)) {
                return RedirectToAction("Index");
            }
            ViewBag.Moves = new SelectList(await _userRepo.GetMovesAsync(user!.Id), "Id", "Name");
            return View(pokemon);
        }

        public async Task<IActionResult> Delete(int id) {
            var user = await _userManager.GetUserAsync(User);
            var pokemon = await _pokemonRepo.ReadAsync(id);
            if (pokemon == null || !user!.IsOwner(pokemon)) {
                return RedirectToAction("Index");
            }
            return View(pokemon);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var user = await _userManager.GetUserAsync(User);
            await _pokemonRepo.DeleteAsync(user!.Id, id);
            return RedirectToAction("Index");
        }
    }
}
