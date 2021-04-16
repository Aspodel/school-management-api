using Microsoft.EntityFrameworkCore;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Database;
using SchoolManagement.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Repository
{
    public class TeacherRepository:BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Teacher?> FindByIdAsync(string idCard, CancellationToken cancellationToken = default)
            => await FindAll(t => t.IdCard == idCard)
                .FirstOrDefaultAsync(cancellationToken);
    }
}
