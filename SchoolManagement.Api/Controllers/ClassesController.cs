using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.DataObjects;
using SchoolManagement.Api.DataObjects.Create;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Entities;
using SchoolManagement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            return Ok(_mapper.Map<IEnumerable<ClassDTO>>(classes));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var @class = await _classRepository.FindByIdAsync(id, cancellationToken);
            if (@class is null)
                return NotFound();

            return Ok(_mapper.Map<ClassDTO>(@class));
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetByCourseId(int courseId, CancellationToken cancellationToken = default)
        {
            var classes = await _classRepository.FindAll(courseId).ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<ClassDTO>>(classes));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClassDTO dto, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.FindByCourseCode(dto.CourseCode, cancellationToken);
            var teacher = await _teacherManager.FindByIdCardAsync(dto.TeacherIdCard);
            if (course is null || teacher is null)
                return BadRequest("Course or Teacher is not exist");

            var @class = _mapper.Map<Class>(dto);
            @class.Course = course;
            @class.Teacher = teacher;
            @class.ClassCode = GenerateCourseCode(course.Classes.Max(d => d.ClassCode), course.CourseCode);

            _classRepository.Add(@class);
            await _classRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { @class.Id }, _mapper.Map<ClassDTO>(@class));
        }

        private string GenerateCourseCode(string? prevId, string course)
        {
            if (!string.IsNullOrEmpty(prevId))
            {
                prevId = prevId.Remove(0, 7);
                //prevId = Regex.Replace(prevId, "[^0-9.]", "");
                var newId = (int.Parse(prevId) + 1).ToString("D2");
                return string.Format("{0}{1}", course, newId);
            }
            else
                return string.Format("{0}01", course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CourseDTO dto, CancellationToken cancellationToken = default)
        {
            var course = await _classRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (course is null)
                return NotFound();

            _mapper.Map(dto, course);
            await _classRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var course = await _classRepository.FindByIdAsync(id, cancellationToken);
            if (course is null)
                return NotFound();

            _classRepository.Delete(course);
            await _classRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
