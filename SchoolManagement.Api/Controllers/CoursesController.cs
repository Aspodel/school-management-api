using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.DataObjects;
using SchoolManagement.Api.DataObjects.Create;
using SchoolManagement.Api.DataObjects.Get;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Api.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await _courseRepository.FindByIdAsync(id);
            if (course is null)
                return NotFound();

            return Ok(_mapper.Map<GetCourseDTO>(course));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseDTO dTO)
        {
            var department = await _departmentRepository.FindByIdAsync(dTO.DepartmentId);
            var course = _mapper.Map<Course>(dTO);
            course.Department = department;
            _courseRepository.Add(course);
            await _courseRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseDTO dTO, CancellationToken cancellationToken = default)
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
