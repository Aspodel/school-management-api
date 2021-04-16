using SchoolManagement.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Contracts
{
    public interface ITeacherRepository : IBaseRepository<Teacher>
    {
        Task<Teacher?> FindByIdAsync(string idCard, CancellationToken cancellationToken = default);
    }
}
