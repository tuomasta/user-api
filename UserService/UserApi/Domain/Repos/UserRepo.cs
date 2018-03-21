using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using UserApi.Exceptions;
using UserApi.ExceptionHandling;

using User = UserApi.Controllers.User;
using UserApi.Controllers;

namespace UserApi.Repos
{
    public class UserRepo : IUserRepo
    {
        public static IUserRepo Configure()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MigrateContext, Migrations.Configuration>());
            using (var context = new MigrateContext())
            {
                context.Database.Initialize(true);
            }

            return new UserRepo();
        }


        public async Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>> predicate = null)
        {
            using (var context = new ReadContext())
            {
                var result = predicate == null ? context.Users : context.Users.Where(predicate);

                return await result.ToArrayAsync().ConfigureAwait(false); 
            }
        }

        public async Task<User> GetUserAsync(Expression<Func<User, bool>> predicate)
        {
            using (var context = new ReadContext())
            {
                var user = await context.Users.SingleOrDefaultAsync(predicate).ConfigureAwait(false);
                return user;
            }
        }

        public async Task<User> CreateUserAsync(User user)
        {
            using (var context = new WriteContext())
            {
                context.Users.Add(user);
                try
                {
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch(DbUpdateException e) when (e.ToString().Contains("Cannot insert duplicate key row in object 'dbo.Users' with unique index 'IX_Email"))
                {
                    throw new ValidationException(ValidationCode.DuplicateEmail, $"User with the email address { user.Email } already exists.");
                }
                return user;
            }
        }
    }
}