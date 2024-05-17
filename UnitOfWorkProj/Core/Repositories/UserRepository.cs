using Microsoft.EntityFrameworkCore;
using UnitOfWorkProj.Core.IRepository;
using UnitOfWorkProj.DAL;
using UnitOfWorkProj.Models;

namespace UnitOfWorkProj.Core.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context, ILogger logger) : base(context, logger) { }
        

        public override async Task<IEnumerable<User>> All()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "{Repo} All method error", typeof(UserRepository));
                return new List<User>();
            }
        }
        public override async Task<bool> Upsert(User entity)
        {
            try
            {
                var existingUser = await _dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (existingUser == null)
                {
                    return await Add(entity);
                }
                existingUser.FirstName = entity.FirstName;
                existingUser.LastName = entity.LastName;
                existingUser.Email = entity.Email;
                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "{Repo} All method error", typeof(UserRepository));
                return false;
            }
        }
        public override async Task<bool> Delete(Guid id)
        {
            try
            {
                var userExist = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
                if(userExist != null)
                {
                    _dbSet.Remove(userExist);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "{Repo} All method error", typeof(UserRepository));
                return false;
            }
        }
    }
}
