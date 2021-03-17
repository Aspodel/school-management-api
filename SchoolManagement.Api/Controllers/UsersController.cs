using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SchoolManagement.Api.DataObjects.Create;
using SchoolManagement.Api.Settings;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly JwTokenConfig _jwTokenConfig;
        private readonly IDepartmentRepository _departmentRepository;

        public UsersController(UserManager<User> userManager, IMapper mapper, IOptions<JwTokenConfig> jwTokenConfig, IDepartmentRepository departmentRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwTokenConfig = jwTokenConfig.Value;
            _departmentRepository = departmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO dTO, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(dTO.DepartmentId);
            var user = _mapper.Map<User>(dTO);
            user.Department = department;
            var number = department.Users.Count;
            user.IdCard = department.ShortName + department.ShortName + "IU" + number ?? "1";
            return Ok(user.IdCard);
        }
    }
}
