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
            => await Users
                .Where(u => u.IdCard == idCard)
                //.Where(u => u.UserRoles.Any(us => us.Role!.NormalizedName == "STUDENT"))
                .Include(u => u.Department)
                .Include(u => u.Classes)
                    .ThenInclude(c => c.Course)
                .Include(u => u.Classes)
                    .ThenInclude(d => d.Teacher)
                .FirstOrDefaultAsync();

        public IQueryable<Student> FindAll(Expression<Func<Student, bool>>? predicate = null)
            => Users
                .Where(u => !u.IsDeleted)
                .WhereIf(predicate != null, predicate!)
                .Include(u => u.Department)
                .Include(u => u.Classes);

        public IQueryable<Student> FindAll(int departmentId, Expression<Func<Student, bool>>? predicate = null)
            => FindAll(predicate)
                .Where(s => s.DepartmentId == departmentId);
    }
}
