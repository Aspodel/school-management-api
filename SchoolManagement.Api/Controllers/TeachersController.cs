using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.DataObjects;
using SchoolManagement.Api.DataObjects.Create;
using SchoolManagement.Api.DataObjects.Get;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Entities;
using SchoolManagement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly TeacherManager _teacherManager;
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IClassRepository _classRepository;

        public TeachersController(TeacherManager teacherManager, IMapper mapper, IDepartmentRepository departmentRepository, IClassRepository classRepository)
        {
            _teacherManager = teacherManager;
            _mapper = mapper;
            _departmentRepository = departmentRepository;
            _classRepository = classRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var teachers = await _teacherManager.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GetTeacherDTO>>(teachers));
        }

        [HttpGet("{idCard}")]
        public async Task<IActionResult> Get(string idCard)
        {
            var teacher = await _teacherManager.FindByIdCardAsync(idCard);
            if (teacher is null)
                return NotFound();

            return Ok(_mapper.Map<GetTeacherDetailDTO>(teacher));
        }

        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetByDepartment(int departmentId, CancellationToken cancellationToken = default)
        {
            var teachers = await _teacherManager.FindAll(departmentId).ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GetTeacherDTO>>(teachers));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeacherDTO dto, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(dto.DepartmentId, cancellationToken);
            if (department is null)
                return BadRequest("Department is not exist");

            var teacher = _mapper.Map<Teacher>(dto);
            teacher.Department = department;
            teacher.IdCard = GenerateIdCard(department.Teachers.Max(d => d.IdCard), department.ShortName);
            teacher.UserName = teacher.IdCard;

            var result = await _teacherManager.CreateAsync(teacher, GeneratePassword(dto.Birthdate));
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            // Add user to specified roles
            var addtoRoleResullt = await _teacherManager.AddToRoleAsync(teacher, "teacher");
            if (!addtoRoleResullt.Succeeded)
            {
                return BadRequest("Fail to add role");
            }

            return CreatedAtAction(nameof(Get), new { teacher.IdCard }, _mapper.Map<TeacherDTO>(teacher));
        }

        private static string GenerateIdCard(string? prevId, string department)
        {
            if (!string.IsNullOrEmpty(prevId))
            {
                prevId = prevId.Remove(0, prevId.Length - 3);
                //prevId = prevId.Remove(0, 4);
                var newId = (int.Parse(prevId) + 1).ToString("D3");
                return string.Format("IU{0}{1}", department, newId);
            }
            else
                return string.Format("IU{0}001", department);
        }

        private static string GeneratePassword(DateTime birthDate)
        {
            return birthDate.ToString("ddMMyy");
        }

        [HttpPut("{idCard}")]
        public async Task<IActionResult> Update([FromBody] TeacherDTO dto)
        {
            var teacher = await _teacherManager.FindByIdCardAsync(dto.IdCard);
            if (teacher is null || teacher.IsDeleted)
                return NotFound();

            _mapper.Map(dto, teacher);

            ICollection<Class> classes = teacher.Classes;
            ICollection<int> requestClasses = dto.Classes;
            ICollection<int> originalClasses = teacher.Classes.Select(c => c.Id).ToList();

            // Delete Classes
            ICollection<int> deleteClasses = originalClasses.Except(requestClasses).ToList();
            if (deleteClasses.Count > 0)
            {
                foreach (var itemClass in deleteClasses)
                {
                    var item = classes.First(c => c.Id == itemClass);
                    classes.Remove(item);
                }
            }

            // Add Classes
            ICollection<int> newClasses = requestClasses.Except(originalClasses).ToList();
            if (newClasses.Count > 0)
            {
                foreach (var itemClass in deleteClasses)
                {
                    var item = await _classRepository.FindByIdAsync(itemClass);
                    if (item is null)
                        return BadRequest("ClassId is not valid");

                    classes.Add(item);
                }
            }

            teacher.Classes = classes;

            await _teacherManager.UpdateAsync(teacher);

            return NoContent();
        }

        [HttpDelete("{idCard}")]
        public async Task<IActionResult> Delete(string idCard)
        {
            var teacher = await _teacherManager.FindByIdCardAsync(idCard);
            teacher.IsDeleted = true;
            await _teacherManager.UpdateAsync(teacher);
            return NoContent();
        }
    }
}
