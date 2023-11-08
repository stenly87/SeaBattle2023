using SeaBattleDB.DB;
using SeaBattleRepository.DTO;
using SeaBattleRepository.MapperHelper;

namespace SeaBattleRepository.Implement
{
    public class RepositoryUser : RepositoryBase<UserDTO>
    {
        public RepositoryUser(User29Context context) :
            base(context)
        {
        }

        // переопределить методы из RepositoryBase
        // надо добавить конвертирование данных из модели в дто

        public override async Task CreateAsync(UserDTO entity)
        {
            var model = entity.ToModel();
            await context.Users.AddAsync(model);
        }

        public override async Task UpdateAsync(UserDTO entity)
        {
            var find = await context.Users.FindAsync(entity.Id);
            if (find == null)
                throw new Exception($"not found User with id {entity.Id}");
            var model = entity.ToModel();
            context.Users.Entry(find).CurrentValues.SetValues(model);
        }
    }
}
