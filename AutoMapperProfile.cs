using AutoMapper;
using myapp.Dtos.Character;
using myapp.Models;

namespace myapp
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character >();
        }
    }
}