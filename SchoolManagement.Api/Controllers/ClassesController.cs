using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.DataObjects;
using SchoolManagement.Api.DataObjects.Create;
using SchoolManagement.Api.DataObjects.Get;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Entities;
using SchoolManagement.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassRepository _classRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly TeacherManager _teacherManager;
        private readonly IMapper _mapper;

        public ClassesController(IClassRepository classRepository ,ICourseRepository courseRepository,TeacherManager teacherManager, IMapper mapper)
        {
            _classRepository = classRepository;
            _courseRepository = courseRepository;
            _teacherManager = teacherManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var classes = await _classRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GetClassDTO>>(classes));
        }

        [HttpGet("{classCode}")]
        public async Task<IActionResult> Get(string classCode, CancellationToken cancellationToken = default)
        {
            var @class = await _classRepository.FindByClassCode(classCode, cancellationToken);
            if (@class is null)
                return NotFound();

            return Ok(_mapper.Map<GetClassDetailDTO>(@class));
        }

        [HttpGet("{courseCode}")]
        public async Task<IActionResult> GetByCourseCode(string courseCode, CancellationToken cancellationToken = default)
        {
            var classes = await _classRepository.FindAll(courseCode).ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GetClassDTO>>(classes));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClassDTO dto, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.FindByCourseCode(dto.CourseCode, cancellationToken);
            if (course is null)
                return BadRequest(new { message = "Course is not found" });

            var teacher = await _teacherManager.FindByIdCardAsync(dto.TeacherIdCard);
            if (teacher is null)
                return BadRequest(new { message = "Teacher is not found" });

            var @class = _mapper.Map<Class>(dto);
            @class.RestSlot = @class.Slot;
            @class.Course = course;
            @class.Teacher = teacher;
            @class.ClassCode = GenerateCourseCode(course.Classes.Max(d => d.ClassCode), course.CourseCode);

            _classRepository.Add(@class);
            await _classRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { @class.ClassCode }, _mapper.Map<ClassDTO>(@class));
        }

        private string GenerateCourseCode(string? prevId, string course)
        {
            if (!string.IsNullOrEmpty(prevId))
            {
                prevId = prevId.Remove(0, prevId.Length - 2);
                //prevId = Regex.Replace(prevId, "[^0-9.]", "");
                var newId = (int.Parse(prevId) + 1).ToString("D2");
                return string.Format("{0}{1}", course, newId);
            }
            else
                return string.Format("{0}01", course);
        }

        [HttpPut("{classCode}")]
        public async Task<IActionResult> Update([FromBody] ClassDTO dto, CancellationToken cancellationToken = default)
        {
            var course = await _classRepository.FindByClassCode(dto.ClassCode, cancellationToken);
            if (course is null)
                return NotFound();

            _mapper.Map(dto, course);
            await _classRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{classCode}")]
        public async Task<IActionResult> Delete(string classCode, CancellationToken cancellationToken = default)
        {
            var course = await _classRepository.FindByClassCode(classCode, cancellationToken);
            if (course is null)
                return NotFound();

            _classRepository.Delete(course);
            await _classRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
