using Microsoft.EntityFrameworkCore;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Database;
using SchoolManagement.Core.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Repository
{
    public class StudentRepository:BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<Student> FindAll(Expression<Func<Student, bool>>? predicate = null)
            => _dbSet
                .Include(s => s.Department);

        public virtual async Task<Student?> FindByIdAsync(string idCard, CancellationToken cancellationToken = default)
            => await FindAll(s => s.IdCard == idCard)
                .FirstOrDefaultAsync();
    }
}
