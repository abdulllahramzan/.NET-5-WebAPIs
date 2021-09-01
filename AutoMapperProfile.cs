using AutoMapper;
using myapp.Dtos.Character;
using myapp.Dtos.Fight;
using myapp.Dtos.Skill;
using myapp.Dtos.Weapon;
using myapp.Models;

namespace myapp
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character >();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();
            CreateMap<Character, HighScoreDto>();
        }
    }
}