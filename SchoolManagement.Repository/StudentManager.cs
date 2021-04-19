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
    public class StudentManager : UserManager<Student>
    {
        public StudentManager(
            IUserStore<Student> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<Student> passwordHasher,
            IEnumerable<IUserValidator<Student>> userValidators,
            IEnumerable<IPasswordValidator<Student>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<Student>> logger
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
        public async Task<Student> FindByIdCardAsync(string idCard)
            => await Users.Where(u => u.IdCard == idCard).FirstOrDefaultAsync();


        public IQueryable<Student> FindAll(Expression<Func<Student, bool>>? predicate = null)
            => Users
                .Where(u => !u.IsDeleted)
                .WhereIf(predicate != null, predicate!);
    }
}
