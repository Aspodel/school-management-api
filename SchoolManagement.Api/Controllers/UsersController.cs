using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchoolManagement.Api.DataObjects;
using SchoolManagement.Api.DataObjects.Create;
using SchoolManagement.Api.Settings;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Entities;
using SchoolManagement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IMapper _mapper;
        private readonly JwTokenConfig _jwTokenConfig;
        private readonly IDepartmentRepository _departmentRepository;

        public UsersController(UserManager userManager, IMapper mapper, IOptions<JwTokenConfig> jwTokenConfig, IDepartmentRepository departmentRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwTokenConfig = jwTokenConfig.Value;
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.FindAll().ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO dTO, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(dTO.DepartmentId, cancellationToken);

            var user = _mapper.Map<User>(dTO);
            user.Department = department;

            var id = GenerateId(department.Users.LastOrDefault()?.IdCard);
            user.IdCard = string.Format("{0}{1}IU{2}", department.ShortName, department.ShortName, id);
            user.UserName = user.IdCard;

            var result = await _userManager.CreateAsync(user, dTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(user);
        }

        private int GenerateId(string prevId)
        {
            if (prevId is null)
                return 1;

            prevId = prevId.Remove(0, 6);
            int newId = int.Parse(prevId) + 1;
            return newId;
        }

        [HttpPut("{idCard}")]
        public async Task<IActionResult> UpdateUser(UserDTO dTO)
        {
            var user = await _userManager.FindByIdCardAsync(dTO.IdCard);
            if (user is null || user.IsDelete)
                return NotFound();

            _mapper.Map(dTO, user);
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpDelete("{idCard}")]
        public async Task<IActionResult> Delete(string idCard)
        {
            var user = await _userManager.FindByIdCardAsync(idCard);
            user.IsDelete = true;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
