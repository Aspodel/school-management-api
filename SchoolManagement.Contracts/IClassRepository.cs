using SchoolManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Contracts
{
    public interface IClassRepository : IBaseRepository<Class>
    {
        IQueryable<Class> FindAll(int courseId, Expression<Func<Class, bool>>? predicate = null);
    }
}
