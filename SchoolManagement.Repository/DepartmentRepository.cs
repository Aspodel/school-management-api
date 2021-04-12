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
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<Department> FindAll(Expression<Func<Department, bool>>? predicate = null)
            => _dbSet
                .WhereIf(predicate != null, predicate!)
                .Include(d => d.Users);

        public override async Task<Department?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
            => await FindAll(d => d.Id == id)
                        //.Include(d => d.Users)
                        .FirstOrDefaultAsync(cancellationToken);
    }
}
