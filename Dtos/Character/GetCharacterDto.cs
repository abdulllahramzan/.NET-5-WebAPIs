using System.Collections.Generic;
using myapp.Dtos.Skill;
using myapp.Dtos.Weapon;
using myapp.Models;

namespace myapp.Dtos.Character
{
    public class GetCharacterDto
    {
         public int Id { get; set; }

        public string Name { get; set; } = "Frodo";

        public int Hitpoints { get; set; } = 100;

        public int Strength { get; set; } = 10;

        public int Defense { get; set; } =  10;

        public int Intelligence { get; set; } = 10;

        public RpgClass Class { get; set; } = RpgClass.Knight; 
        public GetWeaponDto Weapon { get; set; }
        public List<GetSkillDto> Skill { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }

}