using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.DataObjects;
using SchoolManagement.Contracts;
using SchoolManagement.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        public DepartmentsController(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var departments = await _departmentRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<DepartmentDTO>>(departments));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(id, cancellationToken);
            if (department is null)
                return NotFound();

            return Ok(_mapper.Map<DepartmentDTO>(department));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentDTO dto, CancellationToken cancellationToken = default)
        {
            var department = _mapper.Map<Department>(dto);
            _departmentRepository.Add(department);
            await _departmentRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { department.Id }, _mapper.Map<DepartmentDTO>(department));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] DepartmentDTO dto, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (department is null)
                return NotFound();

            _mapper.Map(dto, department);
            await _departmentRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(id, cancellationToken);
            if (department is null)
                return NotFound();

            _departmentRepository.Delete(department);
            await _departmentRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
