using Customon_Creator.Models;
using Customon_Creator.Models.Entities;
using Customon_Creator.Models.ViewModels;
using Customon_Creator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Customon_Creator.Controllers {
    // [AllowAnonymous]
    // Method Team() does not function with the [Authorize] decorator when controller is stated to allow anonymous
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepo;
        private readonly IPokemonRepository _pokemonRepo;
        private readonly IMoveRepository _moveRepo;


        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, IUserRepository userRepo, IPokemonRepository pokemonRepo, IMoveRepository moveRepo) {
            _userManager = userManager;
            _userRepo = userRepo;
            _pokemonRepo = pokemonRepo;
            _moveRepo = moveRepo;
            _logger = logger;
        }

        public IActionResult Index() {
            ViewBag.Users = _userRepo.GetUserCount();
            ViewBag.Pokemon = _pokemonRepo.GetPokemonCount();
            ViewBag.Moves = _moveRepo.GetMoveCount();
            return View();
        }

        public async Task<IActionResult> AllTeams() {
            var teams = await _userRepo.GetAllTeamsAsync();
            return View(teams);
        }
        public async Task<IActionResult> API() {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Team() {
            var user = await _userManager.GetUserAsync(User);
            var pokemon = await _userRepo.GetPokemonWithMovesAsync(user!.Id);
            var moves = await _userRepo.GetMovesAsync(user!.Id);
            var teamVM = new TeamVM {
                PokemonList = pokemon,
                MoveList = moves
            };
            return View(teamVM);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="teamIds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateTeam(int[] teamIds) {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && teamIds != null) {
                if (ModelState.IsValid) {
                    await _pokemonRepo.UpdateTeamAsync(user!.Id, teamIds);
                    return Ok();
                }
            }
            return Problem();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
