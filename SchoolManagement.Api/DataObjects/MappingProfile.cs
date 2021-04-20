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
                .ForMember(d => d.ClassCode, opt => opt.Ignore());
            CreateMap<CreateClassDTO, Class>()
                .ForMember(d => d.CourseId, opt => opt.Ignore())
                .ForMember(d => d.TeacherId, opt => opt.Ignore());
            CreateMap<Class, GetClassDTO>()
                .ForMember(d => d.Teacher, opt => opt.MapFrom(s => s.Teacher == null ? null : s.Teacher.FullName));


            CreateMap<Course, CourseDTO>()
                .ForMember(d => d.Classes, opt => opt.MapFrom(d => d.Classes));
            CreateMap<CourseDTO, Course>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CourseCode, opt => opt.Ignore());
            CreateMap<CreateCourseDTO, Course>();
            CreateMap<Course, GetCourseDTO>()
                .ForMember(d => d.Department, opt => opt.MapFrom(s => s.Department == null ? null : s.Department.Name))
                .ForMember(d => d.Classes, opt => opt.MapFrom(s => s.Classes.Count));


            CreateMap<Department, DepartmentDTO>();
                //.ForMember(d => d.Courses, opt => opt.MapFrom(d => d.Courses.Select(c => c.Name)))
                //.ForMember(d => d.Teachers, opt => opt.MapFrom(d => d.Teachers))
                //.ForMember(d => d.Students, opt => opt.MapFrom(d => d.Students));
            CreateMap<DepartmentDTO, Department>()
                .ForMember(d => d.Id, opt => opt.Ignore());
            CreateMap<CreateDepartmentDTO, Department>();


            CreateMap<Student, StudentDTO>()
                .ForMember(d => d.Classes, opt => opt.MapFrom(s => s.Classes.Select(c => c.Id)));
            CreateMap<StudentDTO, Student>()
                .ForMember(d => d.IdCard, opt => opt.Ignore());
            CreateMap<CreateStudentDTO, Student>();


            CreateMap<Teacher, TeacherDTO>()
                .ForMember(d => d.Classes, opt => opt.MapFrom(s => s.Classes.Select(c => c.Id)));
            CreateMap<TeacherDTO, Teacher>()
                .ForMember(d => d.IdCard, opt => opt.Ignore());
            CreateMap<CreateTeacherDTO, Teacher>();
        }
    }
}
