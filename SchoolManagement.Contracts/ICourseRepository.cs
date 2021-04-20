using SchoolManagement.Core.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Contracts
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        IQueryable<Course> FindAll(int departmentId, Expression<Func<Course, bool>>? predicate = null);
        Task<Course> FindByCourseCode(string courseCode, CancellationToken cancellationToken = default);
    }
}
