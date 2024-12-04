using AutoMapper;
using ManageLogFile.Dtos;
using ManageLogFile.Model.Entities;

namespace ManageLogFile.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, SignupRequestDto>().ReverseMap();
        }
    }
}
