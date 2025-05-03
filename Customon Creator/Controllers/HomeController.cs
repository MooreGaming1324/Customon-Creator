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

        /// <summary>
        /// Main home page of the website, displays generic statistics
        /// </summary>
        /// <returns>Appropriate View</returns>
        public IActionResult Index() {
            ViewBag.Users = _userRepo.GetUserCount();
            ViewBag.Pokemon = _pokemonRepo.GetPokemonCount();
            ViewBag.Moves = _moveRepo.GetMoveCount();
            return View();
        }

        /// <summary>
        /// Returns a view of all users and their respective teams if they exist
        /// </summary>
        /// <returns>Appropriate View</returns>
        public async Task<IActionResult> AllTeams() {
            var teams = await _userRepo.GetAllTeamsAsync();
            return View(teams);
        }

        /// <summary>
        /// Returns a view detailing the endpoints and documentation about the API.
        /// </summary>
        /// <returns>Appropriate View</returns>
        public IActionResult API() {
            return View();
        }

        /// <summary>
        /// Authorization Required. Displays current user's team and various other 
        /// </summary>
        /// <returns>Appropriate View</returns>
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
        ///  Directed from the /Home/Team method, sending a POST request to update a users team.
        /// </summary>
        /// <param name="teamIds">An array of length 6 with the Ids of pokemon and their respective placement on the team.</param>
        /// <returns>Respective HTTP status codes</returns>
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

        /// <summary>
        /// Returns a view of the websites privacy policy.
        /// </summary>
        /// <returns>Appropriate View</returns>
        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
