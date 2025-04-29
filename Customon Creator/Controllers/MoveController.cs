using Customon_Creator.Models.Entities;
using Customon_Creator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Customon_Creator.Controllers {
    [Authorize]
    public class MoveController(UserManager<ApplicationUser> userManager, IUserRepository userRepo, IMoveRepository moveRepo) : Controller {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IMoveRepository _moveRepo = moveRepo;

        public async Task<IActionResult> Index() {
            var user = await _userManager.GetUserAsync(User);
            return View(await _userRepo.GetMovesAsync(user!.Id));
        }

        [AllowAnonymous]
        public async Task<IActionResult> All() {
            return View(await _moveRepo.ReadAllAsync());
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Move newMove) {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid) {
                newMove.UserId = user!.Id;
                await _moveRepo.CreateAsync(newMove);
                return RedirectToAction("Index");
            }
            return View(newMove);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id) {
            var user = await _userManager.GetUserAsync(User);
            var move = await _moveRepo.ReadAsync(id);
            ViewBag.isOwner = false;
            if (move == null) {
                return RedirectToAction("Index");
            }
            if (user != null) {
                ViewBag.isOwner = user!.IsOwner(move!);
            }
            return View(move);
        }

        public async Task<IActionResult> Edit(int id) {
            var user = await _userManager.GetUserAsync(User);
            var move = await _moveRepo.ReadAsync(id);
            if (move == null || !user!.IsOwner(move)) {
                return RedirectToAction("Index");
            }
            return View(move);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Move move) {
            var user = await _userManager.GetUserAsync(User);
            //Check if the user is the proper owner of original unedited pokemon
            if (ModelState.IsValid && await _moveRepo.UpdateAsync(user!.Id, move.Id, move)) {
                return RedirectToAction("Index");
            }
            return View(move);
        }

        public async Task<IActionResult> Delete(int id) {
            var user = await _userManager.GetUserAsync(User);
            var move = await _moveRepo.ReadAsync(id);
            if (move == null || !user!.IsOwner(move)) {
                return RedirectToAction("Index");
            }
            return View(move);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var user = await _userManager.GetUserAsync(User);
            await _moveRepo.DeleteAsync(user!.Id, id);
            return RedirectToAction("Index");
        }
    }
}