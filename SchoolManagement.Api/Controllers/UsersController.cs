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
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var users = await _userManager.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
        }

        [HttpGet("{idCard}")]
        public async Task<IActionResult> Get(string idCard)
        {
            var user = await _userManager.FindByIdCardAsync(idCard);
            if (user is null)
                return NotFound();

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
        {
            var user = _mapper.Map<User>(dto);
            user.IdCard = await GenerateIdCard();
            user.UserName = user.IdCard;

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(_mapper.Map<UserDTO>(user));
        }

        private async Task<string> GenerateIdCard()
        {
            var users = await _userManager.FindAll().ToListAsync();
            var prevId = users.Max(u => u.IdCard);
            if (!string.IsNullOrEmpty(prevId))
            {
                prevId = prevId.Remove(0, 2);
                var newId = (int.Parse(prevId) + 1).ToString("D3");
                return string.Format("IU{0}", newId);
            }
            else
                return string.Format("IU001");
        }

        [HttpPut("{idCard}")]
        public async Task<IActionResult> Update(UserDTO dTO)
        {
            var user = await _userManager.FindByIdCardAsync(dTO.IdCard);
            if (user is null || user.IsDeleted)
                return NotFound();

            _mapper.Map(dTO, user);
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpDelete("{idCard}")]
        public async Task<IActionResult> Delete(string idCard)
        {
            var user = await _userManager.FindByIdCardAsync(idCard);
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
