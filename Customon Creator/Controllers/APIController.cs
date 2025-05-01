using Customon_Creator.Models.Entities;
using Customon_Creator.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Customon_Creator.Controllers {
    [EnableCors]
    [Route("api")]
    [ApiController]
    public class APIController (IUserRepository userRepo, IMoveRepository moveRepo) : Controller {
        private readonly IMoveRepository _moveRepo = moveRepo;
        private readonly IUserRepository _userRepo = userRepo;


        [HttpGet("move")]
        public async Task<IActionResult> Get() {
            var moves = await _moveRepo.ReadAllAsync();
            //Only include username to prevent showing all user data
            var movesWithUsername = moves.Select(move => new
            {
                move.Id,
                move.Name,
                move.Description,
                Type = Enum.GetName(move.Type),
                Category = Enum.GetName(move.Category),
                move.Power,
                move.Accuracy,
                move.PP,
                move.Priority,
                Author = move.User?.UserName
            });
            return Ok(movesWithUsername);
        }

        [HttpPost("move")]
        public async Task<IActionResult> Post([FromForm] Move move) {
            var user = await _userRepo.GetUserAsync("Anonymous");
            move.UserId = user!.Id;
            await _moveRepo.CreateAsync(move);
            return CreatedAtAction("Get", new { id = move.Id }, new {
                move.Id,
                move.Name,
                move.Description,
                Type = Enum.GetName(move.Type),
                Category = Enum.GetName(move.Category),
                move.Power,
                move.Accuracy,
                move.PP,
                move.Priority,
                Author = move.User?.UserName
            });
        }

        [HttpGet("move/{id}")]
        public async Task<IActionResult> Get(int id) {
            var move = await _moveRepo.ReadWithUserAsync(id);
            if (move == null) {
                return NotFound();
            }
            var moveWithUsername = new {
                move.Id,
                move.Name,
                move.Description,
                Type = Enum.GetName(move.Type),
                Category = Enum.GetName(move.Category),
                move.Power,
                move.Accuracy,
                move.PP,
                move.Priority,
                Author = move.User?.UserName
            };
            return Ok(moveWithUsername);
        }

        [HttpPut("move")]
        public async Task<IActionResult> Put([FromForm] Move move) {
            var user = await _userRepo.GetUserAsync("Anonymous");
            move.UserId = user!.Id;
            bool status = await _moveRepo.UpdateAsync(user!.Id, move.Id, move);
            if (status) {
                return NoContent(); // 204 as per HTTP specification
            }
            return BadRequest();
        }

        [HttpDelete("move/{id}")]
        public async Task<IActionResult> Delete(int id) {
            var user = await _userRepo.GetUserAsync("Anonymous");
            await _moveRepo.DeleteAsync(user!.Id, id);
            return NoContent(); // 204 as per HTTP specification
        }
    }
}
