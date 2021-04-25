using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.DataObjects;
using SchoolManagement.Api.DataObjects.Create;
using SchoolManagement.Core.Entities;
using SchoolManagement.Repository;
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

        public UsersController(UserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
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

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            // Add user to specified roles
            var addtoRoleResullt = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!addtoRoleResullt.Succeeded)
            {
                return BadRequest("Fail to add role");
            }

            return Ok(_mapper.Map<UserDTO>(user));
        }

        private async Task<string> GenerateIdCard()
        {
            //var users = await _userManager.FindAll()
            //    .Where(u => u.UserRoles.Any(us => us.Role!.NormalizedName != "STUDENT"))
            //    .Where(u => u.UserRoles.Any(us => us.Role!.NormalizedName != "TEACHER"))
            //    .ToListAsync();

            var users = await _userManager.Users
                .Where(u => u.IdCard.Length == 5)
                .ToListAsync();

            var prevId = users.Max(u => u.IdCard);
            if (!string.IsNullOrEmpty(prevId))
            {
                prevId = prevId.Remove(0, prevId.Length - 3);
                var newId = (int.Parse(prevId) + 1).ToString("D3");
                return string.Format("IU{0}", newId);
            }
            else
                return string.Format("IU001");
        }

        [HttpPut("{idCard}")]
        public async Task<IActionResult> Update(UserDTO dto)
        {
            var user = await _userManager.FindByIdCardAsync(dto.IdCard);
            if (user is null || user.IsDeleted)
                return NotFound();

            _mapper.Map(dto, user);
            await _userManager.UpdateAsync(user);

            ICollection<string> requestRoles = dto.Roles;
            ICollection<string> originalRoles = await _userManager.GetRolesAsync(user);

            // Delete Roles
            ICollection<string> deleteRoles = originalRoles.Except(requestRoles).ToList();
            if (deleteRoles.Count > 0)
                await _userManager.RemoveFromRolesAsync(user, deleteRoles);

            // Add Roles
            ICollection<string> newRoles = requestRoles.Except(originalRoles).ToList();
            if (newRoles.Count > 0)
                await _userManager.AddToRolesAsync(user, newRoles);

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
