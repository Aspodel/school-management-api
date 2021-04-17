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

        public async Task<User> FindByIdCardAsync(string idCard)
            => await Users.Where(u => u.IdCard == idCard).FirstOrDefaultAsync();

        public new async Task<User?> FindByNameAsync(string userName)
        {
            var user = await base.FindByNameAsync(userName);
            if (user is null || user.IsDeleted)
                return null;

            return user;
        }

        public IQueryable<User> FindAll(Expression<Func<User, bool>>? predicate = null)
            => Users
                .Where(u => !u.IsDeleted)
                .WhereIf(predicate != null, predicate!);

        public IQueryable<User> FindAllStudents(Expression<Func<User, bool>>? predicate = null)
            => Users
                .Where(u => !u.IsDeleted)
                .Where(u => u.UserRoles.Any(ur => ur.Role!.Name == "student"))
                .WhereIf(predicate != null, predicate!);

        public IQueryable<User> FindAllTeachers(Expression<Func<User, bool>>? predicate = null)
            => Users
                .Where(u => !u.IsDeleted)
                .Where(u => u.UserRoles.Any(us => us.Role!.Name == "teacher"))
                .WhereIf(predicate != null, predicate!);


        public IQueryable<User> FindStudentByDepartment(int departmentId, Expression<Func<User, bool>>? predicate = null)
            => FindAllStudents().Where(s => s.DepartmentId == departmentId);

        public async Task<User> FindStudentByIdCard(string idCard, Expression<Func<User, bool>>? predicate = null)
            => await FindAllStudents()
                .Where(s => s.IdCard == idCard)
                .FirstOrDefaultAsync();
    }
}
