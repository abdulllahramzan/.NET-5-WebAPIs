using System.Threading.Tasks;
using myapp.Dtos.Character;
using myapp.Dtos.Weapon;
using myapp.Models;

namespace myapp.Services.WeaponService
{
    public interface IWeaponService
    {
         Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}