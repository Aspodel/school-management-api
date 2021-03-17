using AutoMapper;
using SchoolManagement.Api.DataObjects.Create;
using SchoolManagement.Api.DataObjects.Get;
using SchoolManagement.Core.Entities;

namespace SchoolManagement.Api.DataObjects
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<CreateUserDTO, User>();


            CreateMap<CreateDepartmentDTO, Department>();
            CreateMap<Department, GetDepartmentDTO>();

            CreateMap<CreateCourseDTO, Course>();

        }
    }
}
