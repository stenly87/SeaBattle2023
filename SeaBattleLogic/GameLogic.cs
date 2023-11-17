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

        public List<GameDTO> ListFreeGame()
        {
            return repositoryGame.GetByCondition(s=>s.Status == 0).ToList();
        }

        #region shit generation
        /*
        // 0 - незаполненные ячейки
        // 1 - корабль
        // корабли не изгибаются, не стоят рядом друг с другом
        static byte[,] field = new byte[10, 10];
        static void Main(string[] args)
        {
            Random random = new Random();
            DrawShip(random, 4);
            DrawShip(random, 3);
            DrawShip(random, 3);
            DrawShip(random, 2);
            DrawShip(random, 2);
            DrawShip(random, 2);
            DrawShip(random, 1);
            DrawShip(random, 1);
            DrawShip(random, 1);
            DrawShip(random, 1);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                    if (field[i, j] == 2)
                        field[i, j] = 0;
            }
            PrintShips();

        }

        private static void PrintShips()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                    Console.Write(field[i, j] + "\t");
                Console.WriteLine();
            }
        }

        private static void DrawShip(Random random, int length)
        {
            bool draw = false;
            do
            {
                draw = false;
                int x = random.Next(0, 10);
                int y = random.Next(0, 10);
                if (field[x, y] == 0)
                {
                    var direction = random.Next(0, 4);
                    if (TestDirection(direction, x, y, length))
                    {
                        Draw(direction, x, y, length);
                        draw = true;
                    }
                }
            }
            while (!draw);
        }

        private static void Draw(int direction, int x, int y, int length)
        {
            switch (direction)
            {
                case 0: // рисование влево
                        // проверка справа (0 или отсутствует)
                    DrawWaterH(x + 1, y);
                    for (int i = 0; i < length; i++)
                    {
                        DrawSideH(x - i, y);
                    }
                    DrawWaterH(x - length, y);
                    break;
                case 1:
                    // вверх
                    DrawWaterV(x, y + 1);
                    for (int i = 0; i < length; i++)
                    {
                        DrawSideV(x, y - i);
                    }
                    DrawWaterV(x, y - length);
                    break;
                case 2: // направо
                    DrawWaterH(x - 1, y);
                    for (int i = 0; i < length; i++)
                    {
                        DrawSideH(x + i, y);
                    }
                    DrawWaterH(x + length, y);
                    break;
                case 3:
                    DrawWaterV(x, y - 1);
                    for (int i = 0; i < length; i++)
                    {
                        DrawSideV(x, y + i);
                    }
                    DrawWaterV(x, y + length);
                    break;
            }
        }

        static bool TestDirection(int direction, int x, int y, int length)
        {
            switch (direction)
            {
                case 0: // рисование влево
                        // проверка справа (0 или отсутствует)
                        // проверка корабля
                    for (int i = -1; i < length + 1; i++)
                    {
                        if (!CheckSideH(x - i, y))
                            return false;
                    }
                    break;
                case 1:
                    // вверх
                    for (int i = -1; i < length + 1; i++)
                    {
                        if (!CheckSideV(x, y - i))
                            return false;
                    }
                    break;
                case 2: // направо
                    for (int i = -1; i < length + 1; i++)
                    {
                        if (!CheckSideH(x + i, y))
                            return false;
                    }
                    break;
                case 3:
                    for (int i = -1; i < length + 1; i++)
                    {
                        if (!CheckSideV(x, y + i))
                            return false;
                    }
                    break;
            }
            return true;
        }

        static void DrawSideH(int x, int y)
        {
            if (x >= 0 && x <= 9 && y >= 0 && y <= 9)
                field[x, y] = 1;
            if (y - 1 >= 0)
                field[x, y - 1] = 2;
            if (y + 1 < 10)
                field[x, y + 1] = 2;
        }

        static void DrawSideV(int x, int y)
        {
            if (x >= 0 && x <= 9 && y >= 0 && y <= 9)
                field[x, y] = 1;
            if (x - 1 >= 0)
                field[x - 1, y] = 2;
            if (x + 1 < 10)
                field[x + 1, y] = 2;
        }

        static void DrawWaterH(int x, int y)
        {
            if (x >= 0 && x <= 9 && y >= 0 && y <= 9)
                field[x, y] = 2;
            if (x >= 0 && x <= 9 && y - 1 >= 0)
                field[x, y - 1] = 2;
            if (x >= 0 && x <= 9 && y + 1 < 10)
                field[x, y + 1] = 2;
        }

        static void DrawWaterV(int x, int y)
        {
            if (x >= 0 && x <= 9 && y >= 0 && y <= 9)
                field[x, y] = 2;
            if (x - 1 >= 0 && x - 1 <= 9 && y >= 0)
                field[x - 1, y] = 2;
            if (x + 1 >= 0 && x + 1 <= 9 && y < 10)
                field[x + 1, y] = 2;
        }

        static bool CheckSideH(int x, int y)
        {
            if (x < 0 || x > 9 || y < 0 || y > 9)
                return false;

            if (field[x, y] != 0)
                return false;

            if (y - 1 >= 0 && field[x, y - 1] == 1)
                return false;

            if (y + 1 < 10 && field[x, y + 1] == 1)
                return false;

            return true;
        }

        static bool CheckSideV(int x, int y)
        {
            if (x < 0 || x > 9 || y < 0 || y > 9)
                return false;

            if (field[x, y] != 0)
                return false;

            if (x - 1 >= 0 && field[x - 1, y] == 1)
                return false;

            if (x + 1 < 10 && field[x + 1, y] == 1)
                return false;

            return true;
        }
        */
        #endregion
    }
}