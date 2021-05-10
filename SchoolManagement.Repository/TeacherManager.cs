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
    public class TeacherManager : UserManager<Teacher>
    {
        public TeacherManager(
            IUserStore<Teacher> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<Teacher> passwordHasher,
            IEnumerable<IUserValidator<Teacher>> userValidators,
            IEnumerable<IPasswordValidator<Teacher>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<Teacher>> logger
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task<Teacher> FindByIdCardAsync(string idCard)
            => await Users
                .Where(u => u.IdCard == idCard)
                //.Where(u => u.UserRoles.Any(us => us.Role!.NormalizedName == "TEACHER"))
                .Include(u => u.Department)
                .Include(u => u.Classes)
                    .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync();

        public IQueryable<Teacher> FindAll(Expression<Func<Teacher, bool>>? predicate = null)
            => Users
                .Where(u => !u.IsDeleted)
                .WhereIf(predicate != null, predicate!)
                .Include(u => u.Department)
                .Include(u => u.Classes);

        public IQueryable<Teacher> FindAll(int departmentId, Expression<Func<Teacher, bool>>? predicate = null)
            => FindAll(predicate)
                .Where(s => s.DepartmentId == departmentId);
    }
}
