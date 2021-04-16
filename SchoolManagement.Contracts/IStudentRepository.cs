using SchoolManagement.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Contracts
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<Student?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
