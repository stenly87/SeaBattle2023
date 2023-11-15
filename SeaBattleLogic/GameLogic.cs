using SeaBattleRepository.DTO;
using SeaBattleRepository.Implement;

namespace SeaBattleLogic
{
    public class GameLogic
    {
        readonly RepositoryGame repositoryGame;
        readonly RepositoryUser repositoryUser;

        public GameLogic(RepositoryGame repositoryGame, RepositoryUser repositoryUser)
        {
            this.repositoryGame = repositoryGame;
            this.repositoryUser = repositoryUser;
        }

        public async Task<GameDTO> CreateGameAsync(int idUser)
        {
            GameDTO gameDTO = new GameDTO
            {
                Creator = await repositoryUser.SearchEntryByConditionAsync(s => s.Id == idUser),
                IdUserNextTurn = idUser,
                FieldUser1 = new byte[1],
                FieldUser2 = new byte[1],
            };
            gameDTO.Id = await repositoryGame.CreateAsync(gameDTO);
            return gameDTO;
        }
    }
}