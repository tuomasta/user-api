using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using User = UserApi.Controllers.User;

namespace UserApi.Repos
{
    public interface IUserRepo
    {
        Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>> predicate = null);
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserAsync(Expression<Func<User, bool>> predicate);
    }
}