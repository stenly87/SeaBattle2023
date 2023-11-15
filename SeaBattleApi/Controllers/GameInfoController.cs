using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaBattleLogic;
using SeaBattleRepository.DTO;
using SeaBattleRepository.Implement;
using System.Security.Claims;

namespace SeaBattleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameInfoController : ControllerBase
    {
        GameLogic gameLogic;

        public GameInfoController(GameLogic gameLogic)
        {
            this.gameLogic = gameLogic;
        }

        [Authorize]
        [HttpPost("CreateGame")]
        public async Task<ActionResult<GameDTO>> CreateGame()
        {
            int idUser = GetUserID();
            GameDTO game = await gameLogic.CreateGameAsync(idUser);
            return game;
        }

        private int GetUserID()
        {
            var claim = HttpContext.User.Claims
                            .FirstOrDefault(s => s.Type == "ID");
            if (claim == null)
                throw new Exception("Требуется авторизация");
            int idUser = int.Parse(claim.Value);
            return idUser;
        }
    }
}
