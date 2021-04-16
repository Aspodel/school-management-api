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
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<Course> FindAll(Expression<Func<Course, bool>>? predicate = null)
            => _dbSet
                .WhereIf(predicate != null, predicate!)
                .Include(c => c.Classes)
                .Include(c => c.Department);

        public IQueryable<Course> FindAll(int departmentId, Expression<Func<Course, bool>>? predicate = null)
            => FindAll(predicate)
                .Where(c => c.DepartmentId == departmentId);

        //public virtual async Task<Course?> FindByIdAsync(string courseCode, CancellationToken cancellationToken = default)
        //    => await FindAll(c => c.CourseCode == courseCode)
        //        .FirstOrDefaultAsync(cancellationToken);
    }
}
