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

        public List<GameDTO> ListFreeGame(int opponentId)
        {
            return repositoryGame.GetByCondition(s => s.Status == 0 && s.CreatorUserId != opponentId).ToList();
        }

        public async Task<bool> JoinGameAsync(int opponentId, int idGame)
        {
            var game = await repositoryGame.SearchEntryByConditionAsync(s => s.Id == idGame && s.CreatorUserId != opponentId);
            if (game.Id == 0)
                throw new Exception($"not found game by id {idGame}");
            var userOpponent = await repositoryUser.SearchEntryByConditionAsync(s => s.Id == opponentId);
            if (userOpponent.Id == 0)
                throw new Exception($"not found opponent by id {opponentId}");
            game.Opponent = userOpponent;
            var field1 = FieldGeneration.Execute();
            game.FieldUser1 = FieldGeneration.GetOneDimensionField(field1);
            var field2 = FieldGeneration.Execute();
            game.FieldUser2 = FieldGeneration.GetOneDimensionField(field2);
            game.Status = 1;
            game.IdUserNextTurn = game.Creator.Id;
            await repositoryGame.UpdateAsync(game);
            await repositoryGame.SaveAsync();
            return true;
        }

        public async Task<GameTurn> CheckTurnAsync(int userId, int idGame)
        {
            var game = await repositoryGame.SearchEntryByConditionAsync(s => s.Id == idGame);
            if (game.Id == 0)
                throw new Exception($"not found game by id {idGame}");
            var userOpponent = await repositoryUser.SearchEntryByConditionAsync(s => s.Id == userId);
            if (userOpponent.Id == 0)
                throw new Exception($"not found opponent by id {userId}");

            if (game.IdUserNextTurn == userId)
            {
                return new GameTurn
                {
                    FieldUser = game.Creator.Id == userId ? game.FieldUser1 : game.FieldUser2,
                    IdUserNextTurn = userId
                };
            }
        }
    }
}