using AutoMapper;
using SchoolManagement.Api.DataObjects.Create;
using SchoolManagement.Api.DataObjects.Get;
using SchoolManagement.Core.Entities;
using System.Linq;

namespace SchoolManagement.Api.DataObjects
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<User, UserDTO>()
                .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.UserRoles.Select(ur => ur.Role!.Name)));
            CreateMap<UserDTO, User>()
                .ForMember(d => d.IdCard, opt => opt.Ignore());
            CreateMap<CreateUserDTO, User>();


            CreateMap<Class, ClassDTO>();
            CreateMap<ClassDTO, Class>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.ClassCode, opt => opt.Ignore())
                .ForMember(d => d.CourseId, opt => opt.Ignore())
                .ForMember(d => d.TeacherId, opt => opt.Ignore())
                .ForMember(d => d.Students, opt => opt.Ignore())
                .ForMember(d => d.RestSlot, opt => opt.Ignore());
            CreateMap<CreateClassDTO, Class>()
                .ForMember(d => d.CourseId, opt => opt.Ignore())
                .ForMember(d => d.TeacherId, opt => opt.Ignore());
            CreateMap<Class, GetClassDetailDTO>()
                //.ForMember(d => d.Teacher, opt => opt.MapFrom(s => s.Teacher == null ? null : s.Teacher.FullName))
                .ForMember(d => d.Teacher, opt => opt.MapFrom(s => s.Teacher))
                .ForMember(d => d.Course, opt => opt.MapFrom(s => s.Course))
                //.ForMember(d => d.Department, opt => opt.MapFrom(s => s.Course!.Department!.Name));
                .ForMember(d => d.Department, opt => opt.MapFrom(s => s.Course!.Department));
            CreateMap<Class,GetClassDTO>()
                .ForMember(d => d.Teacher, opt => opt.MapFrom(s => s.Teacher == null ? null : s.Teacher.FullName))
                .ForMember(d => d.Course, opt => opt.MapFrom(s => s.Course!.Name))
                .ForMember(d => d.Department, opt => opt.MapFrom(s => s.Course!.Department!.Name));


            CreateMap<Course, CourseDTO>();
                //.ForMember(d => d.Classes, opt => opt.MapFrom(d => d.Classes));
            CreateMap<CourseDTO, Course>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CourseCode, opt => opt.Ignore());
            CreateMap<CreateCourseDTO, Course>();
            CreateMap<Course, GetCourseDTO>()
                .ForMember(d => d.Department, opt => opt.MapFrom(s => s.Department == null ? null : s.Department.Name))
                .ForMember(d => d.Classes, opt => opt.MapFrom(s => s.Classes.Count));


            CreateMap<Department, DepartmentDTO>();
            CreateMap<DepartmentDTO, Department>()
                .ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<Student, StudentDTO>()
                .ForMember(d => d.Classes, opt => opt.MapFrom(s => s.Classes.Select(c => c.Id)));
            CreateMap<StudentDTO, Student>()
                .ForMember(d => d.IdCard, opt => opt.Ignore())
                .ForMember(d => d.DepartmentId, opt => opt.Ignore())
                .ForMember(d => d.Classes, opt => opt.Ignore());
            CreateMap<CreateStudentDTO, Student>();
            CreateMap<Student, GetStudentDetailDTO>()
                .ForMember(d => d.Department, opt => opt.MapFrom(s => s.Department == null ? null : s.Department.Name));
            CreateMap<Student, GetStudentDTO>()
                .ForMember(d => d.Department, opt => opt.MapFrom(s => s.Department == null ? null : s.Department.Name))
                .ForMember(d => d.Birthdate, opt => opt.MapFrom(s => s.Birthdate.ToString("dd-MM-yyyy")));


            CreateMap<Teacher, TeacherDTO>()
                .ForMember(d => d.Classes, opt => opt.MapFrom(s => s.Classes.Select(c => c.Id)));
            CreateMap<TeacherDTO, Teacher>()
                .ForMember(d => d.IdCard, opt => opt.Ignore())
                .ForMember(d => d.DepartmentId, opt => opt.Ignore())
                .ForMember(d => d.Classes, opt => opt.Ignore());
            CreateMap<CreateTeacherDTO, Teacher>();
            CreateMap<Teacher, GetTeacherDetailDTO>()
                .ForMember(d => d.Department, opt => opt.MapFrom(s => s.Department!.Name))
                .ForMember(d => d.Birthdate, opt => opt.MapFrom(s => s.Birthdate.ToString("dd-MM-yyyy")));
            CreateMap<Teacher, GetTeacherDTO>()
                .ForMember(d => d.Department, opt => opt.MapFrom(s => s.Department!.Name))
                .ForMember(d => d.Birthdate, opt => opt.MapFrom(s => s.Birthdate.ToString("dd-MM-yyyy")));
        }
    }
}
