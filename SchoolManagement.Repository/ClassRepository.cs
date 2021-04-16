using Microsoft.EntityFrameworkCore;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Database;
using SchoolManagement.Core.Entities;
using SchoolManagement.Repository.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SchoolManagement.Repository
{
    public class ClassRepository : BaseRepository<Class>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<Class> FindAll(Expression<Func<Class, bool>>? predicate = null)
            => _dbSet
                .WhereIf(predicate != null, predicate!)
                .Include(c => c.Teacher)
                .Include(c => c.Course)
                    .ThenInclude(c => c!.Department);

        public IQueryable<Class> FindAll(int courseId, Expression<Func<Class, bool>>? predicate = null)
            => FindAll(predicate)
                .Where(c => c.CourseId == courseId);
    }
    
}
