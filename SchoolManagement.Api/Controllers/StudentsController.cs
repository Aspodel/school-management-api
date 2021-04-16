using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.DataObjects;
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
        private readonly UserManager _userManager;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;

        public StudentsController(UserManager userManager ,IStudentRepository studentRepository, IMapper mapper, IDepartmentRepository departmentRepository)
        {
            _userManager = userManager;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var students = await _userManager.FindAllStudent().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<StudentDTO>>(students));
        }

        [HttpGet("{idCard}")]
        public async Task<IActionResult> Get(string idCard)
        {
            var student = await _userManager.FindByIdCardAsync(idCard);
            if (student is null)
                return NotFound();

            return Ok(_mapper.Map<StudentDTO>(student));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDTO dto, CancellationToken cancellationToken=default)
        {
            var department = await _departmentRepository.FindByIdAsync(dto.DepartmentId, cancellationToken);
            if (department is null)
                return BadRequest("Department is not exist");

            var student = _mapper.Map<Student>(dto);
            student.Department = department;
            student.IdCard = GenerateIdCard(department.Students.Max(d => d.IdCard), department.ShortName);
            student.UserName = student.IdCard;

            var result = await _userManager.CreateAsync(student, GeneratePassword(dto.Birthdate));
            if (!result.Succeeded)
            { 
                return BadRequest(result);
            }

            // Add user to specified roles
            var addtoRoleResullt = await _userManager.AddToRoleAsync(student, "student");
            if (!addtoRoleResullt.Succeeded)
            {
                return BadRequest("Fail to add role");
            }

            return CreatedAtAction(nameof(Get), new { student.IdCard }, _mapper.Map<StudentDTO>(student));
        }

        private static string GenerateIdCard(string? prevId, string department)
        {
            var academicYear = DateTime.Now.ToString("yy");
            if (!string.IsNullOrEmpty(prevId))
            {
                prevId = prevId.Remove(0, 8);
                var newId = (int.Parse(prevId) + 1).ToString("D3");
                return string.Format("{0}{0}IU{1}{2}", department, academicYear, newId);
            }
            else
                return string.Format("{0}{0}IU{1}001", department, academicYear);
        }
        
        private static string GeneratePassword(DateTime birthDate)
        {
            return birthDate.ToString("ddMMyy");
        }
    }
}
