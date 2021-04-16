using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.DataObjects;
using SchoolManagement.Api.DataObjects.Create;
using SchoolManagement.Api.DataObjects.Get;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public CoursesController(ICourseRepository courseRepository, IDepartmentRepository departmentRepository,IMapper mapper)
        {
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var courses = await _courseRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GetCourseDTO>>(courses));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.FindByIdAsync(id, cancellationToken);
            if (course is null)
                return NotFound();

            return Ok(_mapper.Map<CourseDTO>(course));
        }

        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetByDepartmentId(int departmentId, CancellationToken cancellationToken = default)
        {
            var courses = await _courseRepository.FindAll(departmentId).ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GetCourseDTO>>(courses));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDTO dTO, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(dTO.DepartmentId, cancellationToken);
            if (department is null)
                return BadRequest("Department is not exist");

            var course = _mapper.Map<Course>(dTO);
            course.Department = department;
            course.CourseCode = GenerateCourseCode(department.Courses.Max(d => d.CourseCode), department.ShortName);

            _courseRepository.Add(course);
            await _courseRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { course.Id }, _mapper.Map<CourseDTO>(course));
        }

        private string GenerateCourseCode(string? prevId, string department)
        {
            if (!string.IsNullOrEmpty(prevId))
            {
                prevId = Regex.Replace(prevId, "[^0-9.]", "");
                var newId = (int.Parse(prevId) + 1).ToString("D3");
                return string.Format("{0}{1}IU", department, newId);
            }
            else
                return string.Format("{0}001IU", department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CourseDTO dTO, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.FindByIdAsync(dTO.Id, cancellationToken);
            if (course is null)
                return NotFound();

            _mapper.Map(dTO, course);
            await _courseRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.FindByIdAsync(id, cancellationToken);
            if (course is null)
                return NotFound();

            _courseRepository.Delete(course);
            await _courseRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
