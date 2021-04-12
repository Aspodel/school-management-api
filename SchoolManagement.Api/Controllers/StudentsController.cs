using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.DataObjects.Create;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Database;
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
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager _userManager;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;

        public StudentsController(ApplicationDbContext context, UserManager userManager ,IStudentRepository studentRepository, IMapper mapper, IDepartmentRepository departmentRepository)
        {
            _context = context;
            _userManager = userManager;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var students = await _studentRepository.FindAll().ToListAsync();
            var result = await _context.Departments.Include(x=>x.Users).ToListAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDTO dto, CancellationToken cancellationToken=default)
        {
            var department = await _departmentRepository.FindByIdAsync(dto.DepartmentId);
            //var test = department.Users.Max(d => d.IdCard);
            var user = _mapper.Map<Student>(dto);
            user.Department = department;

            var id = GenerateId(department.Users.Max(d => d.IdCard)); 
            user.IdCard = string.Format("{0}{1}IU{2}", department.ShortName, department.ShortName, id);
            user.UserName = user.IdCard;

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            { 
                return BadRequest(result);
            }

            // Add user to specified roles
            var addtoRolesResullt = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!addtoRolesResullt.Succeeded)
            {
                return BadRequest("Fail to add role");
            }

            await _studentRepository.SaveChangesAsync();
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


    }
}
