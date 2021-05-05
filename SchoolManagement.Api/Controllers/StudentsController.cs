﻿using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentManager _studentManager;
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IClassRepository _classRepository;

        public StudentsController(StudentManager studentManager , IMapper mapper, IDepartmentRepository departmentRepository, IClassRepository classRepository)
        {
            _studentManager = studentManager;
            _mapper = mapper;
            _departmentRepository = departmentRepository;
            _classRepository = classRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var students = await _studentManager.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GetStudentDTO>>(students));
        }

        [HttpGet("{idCard}")]
        public async Task<IActionResult> Get(string idCard)
        {
            var student = await _studentManager.FindByIdCardAsync(idCard);
            if (student is null)
                return NotFound();

            //student.Classes = student.Classes.OrderBy(c => c.Day).ThenBy(c => c.StartPeriods).ToList();
            return Ok(_mapper.Map<GetStudentDetailDTO>(student));
        }

        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetByDepartment(int departmentId, CancellationToken cancellationToken = default)
        {
            var students = await _studentManager.FindAll(departmentId).ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GetStudentDTO>>(students));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDTO dto, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(dto.DepartmentId, cancellationToken);
            if (department is null)
                return BadRequest("Department is not exist");

            var student = _mapper.Map<Student>(dto);
            student.Department = department;
            student.IdCard = GenerateIdCard(department.Students.Max(d => d.IdCard), department.ShortName);
            student.UserName = student.IdCard;

            var result = await _studentManager.CreateAsync(student, GeneratePassword(dto.Birthdate));
            if (!result.Succeeded)
            { 
                return BadRequest(result);
            }

            // Add user to specified roles
            var addtoRoleResullt = await _studentManager.AddToRoleAsync(student, "student");
            if (!addtoRoleResullt.Succeeded)
            {
                return BadRequest("Fail to add role");
            }

            return CreatedAtAction(nameof(Get), new { student.IdCard }, _mapper.Map<StudentDTO>(student));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFromExcel (IFormFile file, CancellationToken cancellationToken = default)
        {
            var extension = "." + file.FileName.Split('.')[^1];

            //Create a new Name for the file due to security reasons.
            var fileName = DateTime.Now.Ticks + extension;

            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files", fileName);

            using (var newStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(newStream, cancellationToken);
            }

            var stream = System.IO.File.Open(path, FileMode.Open);

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, false))
            {
                //create the object for workbook part  
                WorkbookPart workbookPart = doc.WorkbookPart;
                SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                SharedStringTable sst = sstpart.SharedStringTable;

                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                Worksheet sheet = worksheetPart.Worksheet;

                var cells = sheet.Descendants<Cell>();
                var rows = sheet.Descendants<Row>();

                System.Diagnostics.Debug.WriteLine("Row count = {0}", rows.LongCount());
                System.Diagnostics.Debug.WriteLine("Cell count = {0}", cells.LongCount());

                foreach (Row row in rows)
                {
                    foreach (Cell c in row.Elements<Cell>())
                    {
                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                        {
                            int ssid = int.Parse(c.CellValue!.Text);
                            string str = sst.ChildElements[ssid].InnerText;
                            System.Diagnostics.Debug.WriteLine("Shared string {0}: {1}", ssid, str);
                        }
                        else if (c.CellValue != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Cell contents: {0}", c.CellValue.Text);
                        }
                    }
                }
            }

            return Ok(new { extension, fileName, path});
        }

        private static string GenerateIdCard(string? prevId, string department)
        {
            var academicYear = DateTime.Now.ToString("yy");
            if (!string.IsNullOrEmpty(prevId))
            {
                prevId = prevId.Remove(0, prevId.Length - 3);
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

        [HttpPut("{idCard}")]
        public async Task<IActionResult> Update([FromBody] StudentDTO dto)
        {
            var student = await _studentManager.FindByIdCardAsync(dto.IdCard);
            if (student is null || student.IsDeleted)
                return NotFound();

            _mapper.Map(dto, student);

            ICollection<Class> classes = student.Classes;
            ICollection<int> requestClasses = dto.Classes;
            ICollection<int> originalClasses = student.Classes.Select(c => c.Id).ToList();

            // Delete Classes
            ICollection<int> deleteClasses = originalClasses.Except(requestClasses).ToList();
            if (deleteClasses.Count > 0) { 
                foreach(var itemClass in deleteClasses)
                {
                    //var item = classes.First(c => c.Id == itemClass);
                    var item = await _classRepository.FindByIdAsync(itemClass);
                    if (item is null)
                        return BadRequest($"ClassId {itemClass} is not valid");

                    item.RestSlot++;
                    classes.Remove(item);
                }
            }

            // Add Classes
            ICollection<int> newClasses = requestClasses.Except(originalClasses).ToList();
            if (newClasses.Count > 0)
            {
                foreach (var itemClass in newClasses)
                {
                    var item = await _classRepository.FindByIdAsync(itemClass);
                    if (item is null)
                        return BadRequest($"ClassId {itemClass} is not valid");

                    item.RestSlot--;
                    classes.Add(item);
                }
            }

            classes = classes.OrderBy(c => c.Day).ThenBy(c => c.StartPeriods).ToList();
            student.Classes = classes;

            await _classRepository.SaveChangesAsync();
            await _studentManager.UpdateAsync(student);

            return Ok(student);
        }

        [HttpDelete("{idCard}")]
        public async Task<IActionResult> Delete(string idCard)
        {
            var student = await _studentManager.FindByIdCardAsync(idCard);
            student.IsDeleted = true;
            await _studentManager.UpdateAsync(student);
            return NoContent();
        }
    }
}
