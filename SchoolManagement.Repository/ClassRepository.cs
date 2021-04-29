using Microsoft.EntityFrameworkCore;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Database;
using SchoolManagement.Core.Entities;
using SchoolManagement.Repository.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Repository
{
    public class ClassRepository : BaseRepository<Class>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<Class> FindAll(Expression<Func<Class, bool>>? predicate = null)
            => _dbSet
                .WhereIf(predicate != null, predicate!)
                .Include(c => c.Teacher)
                .Include(c => c.Students)
                .Include(c => c.Course)
                    .ThenInclude(c => c!.Department);

        public IQueryable<Class> FindAll(string courseCode, Expression<Func<Class, bool>>? predicate = null)
            => FindAll(predicate)
                .Where(c => c.Course!.CourseCode == courseCode);

        public override async Task<Class?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
            => await FindAll(c => c.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

        public async Task<Class> FindByClassCode(string classCode, CancellationToken cancellationToken = default)
            => await FindAll()
                .Where(c => c.ClassCode == classCode)
                .FirstOrDefaultAsync(cancellationToken);
    }
    
}
