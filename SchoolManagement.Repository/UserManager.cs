using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SchoolManagement.Core.Entities;
using SchoolManagement.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Repository
{
    public class UserManager : UserManager<User>
    {
        public UserManager(
            IUserStore<User> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<User> passwordHasher, 
            IEnumerable<IUserValidator<User>> userValidators, 
            IEnumerable<IPasswordValidator<User>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services,
            ILogger<UserManager<User>> logger
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task<User> FindByIdCardAsync(string id)
            => await Users.Where(u => u.IdCard == id).FirstOrDefaultAsync();

        public IQueryable<User> FindAll(Expression<Func<User, bool>>? predicate = null)
            => Users
                .WhereIf(predicate != null, predicate!);
    }
}
