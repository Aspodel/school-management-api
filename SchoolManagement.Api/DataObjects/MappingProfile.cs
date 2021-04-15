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
                .ForMember(d => d.Roles, o => o.MapFrom(s => s.UserRoles.Select(ur => ur.Role!.Name))); 
            CreateMap<UserDTO, User>();

            CreateMap<CreateUserDTO, User>();            
            CreateMap<CreateUserDTO, Student>();


            CreateMap<CreateDepartmentDTO, Department>();
            CreateMap<Department, GetDepartmentDTO>();

            CreateMap<CreateCourseDTO, Course>();

        }
    }
}
