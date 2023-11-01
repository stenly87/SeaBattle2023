using SeaBattleDB.DB;
using SeaBattleRepository.DTO;
using SeaBattleRepository.MapperHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleRepository.Implement
{
    public class RepositoryGame : RepositoryBase<GameDTO>
    {
        public RepositoryGame(User29Context context) : base(context)
        {
        }

        public override async Task CreateAsync(GameDTO entity)
        {
            // если мы будем сохранять объект таким образом
            // то мы получим ошибку
            // потому что есть коллекция с юзерами
            // которые уже есть в бentity.ToModel()д
            // но метод Add помечает всю иерархию объектов
            // как новые записи
            //context.Games.Add(entity.ToModel());
            // поэтому добавляем объект через присоединение
            // и изменение статуса на "добавленный"
            var model = entity.ToModel();
            context.Games.Attach(model);
            context.Games.Entry(model).State = 
                Microsoft.EntityFrameworkCore.EntityState.Added;
            await Task.CompletedTask;
        }

        public override async Task UpdateAsync(GameDTO entity)
        {
            var find = await context.Games.FindAsync(entity.Id);
            if (find == null)
                throw new Exception($"game with id {entity.Id} not found");

            var model = entity.ToModel();
            context.Games.Entry(find).CurrentValues.SetValues(model);
            foreach (var user in model.IdUsers)
                if (!find.IdUsers.Contains(user))
                    find.IdUsers.Add(user);
            await Task.CompletedTask;
        }
    }
}
