using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.DataObjects;
using SchoolManagement.Core.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RolesController(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var roles = await _roleManager.Roles.OrderBy(r => r.NormalizedName).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<RoleDTO>>(roles));
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleDTO dTO)
        {
            var role = _mapper.Map<Role>(dTO);
            await _roleManager.CreateAsync(role);

            return Ok(_mapper.Map<RoleDTO>(role));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(RoleDTO dTO)
        {
            var role = await _roleManager.FindByIdAsync(dTO.Id);
            if (role is null)
                return NotFound();

            _mapper.Map(dTO, role);
            await _roleManager.UpdateAsync(role);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();

            await _roleManager.DeleteAsync(role);
            return NoContent();
        }
    }
}
