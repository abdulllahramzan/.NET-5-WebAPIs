using System.Threading.Tasks;
using myapp.Dtos.Fight;
using myapp.Models;

namespace myapp.Services.FightService
{
    public interface IFightService
    {
         Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
         Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);
         
         Task<ServiceResponse<FightResultDto>> Fight(FightResultDto request);
        Task<object> Fight(FightRequestDto request);
    }
}