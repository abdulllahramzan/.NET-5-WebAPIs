using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myapp.Data;
using myapp.Dtos.Fight;
using myapp.Models;

namespace myapp.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        public FightService(DataContext context)
        {
            _context = context;

        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await _context.Characters
               .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                int damage = DoWeaponAttack(attacker, opponent);
                if (opponent.Hitpoints < 0)
                {
                    response.Message = $"{opponent.Name} has been Defeated !";
                }
                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    AttackHp = attacker.Hitpoints,
                    Opponent = opponent.Name,
                    OpponentHp = opponent.Hitpoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defense);
            if (damage > 0)
            {
                opponent.Hitpoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                .Include(c => c.Skill)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await _context.Characters
               .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                var skill = attacker.Skill.FirstOrDefault(s => s.Id == request.SkillId);
                if (skill == null)
                {
                    response.Success = false;
                    response.Message = $"{attacker.Name} doesn't know this Skill";
                    return response;
                }

                int damage = DoSkillAttack(attacker, opponent, skill);
                if (opponent.Hitpoints < 0)
                {
                    response.Message = $"{opponent.Name} has been Defeated !";
                }
                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    AttackHp = attacker.Hitpoints,
                    Opponent = opponent.Name,
                    OpponentHp = opponent.Hitpoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
    }

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(opponent.Defense);
            if (damage > 0)
            {
                opponent.Hitpoints -= damage;
            }

            return damage;
        }

        public Task<ServiceResponse<FightResultDto>> Fight(FightResultDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<object> Fight(FightRequestDto request)
        {
           var response = new ServiceResponse<FightResultDto>
           {
               Data = new FightResultDto()
           };
           try
           {
               var characters = await _context.Characters
               .Include(c => c.Weapon)
               .Include(c => c.Skill)
               .Where(c => request.CharacterIds.Contains(c.Id )).ToListAsync();

               bool defeated = false;
               while(!defeated)
               {
                   foreach(var attacker  in characters)
                    {
                            var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                            var opponent = opponents[new Random().Next(opponents.Count)];

                            int damage = 0;
                            string attackUsed = string.Empty;

                            bool useWeapon = new Random().Next(2) == 0;
                            if(useWeapon)
                            {
                                attackUsed =attacker.Weapon.Name;
                                damage = DoWeaponAttack(attacker, opponent);
                            }
                            else
                            {
                                var skill = attacker.Skill[new Random().Next(attacker.Skill.Count)];
                                attackUsed = skill.Name;
                                damage = DoSkillAttack(attacker, opponent, skill);
                            }

                     response.Data.Log
                    .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0 )} damage.");

                            if(opponent.Hitpoints <= 0 )
                            {
                                defeated =  true;
                                attacker.Victories++;
                                opponent.Defeats++;
                                response.Data.Log.Add($"{opponent.Name} has been defeated !");
                                response.Data.Log.Add($"{attacker.Name} wins with {attacker.Hitpoints} Hp left !");
                                break;
                            }   
                    }
               }

               characters.ForEach( c => 
                {
                    c.Fights++;
                    c.Hitpoints = 100;
                }
               );
               await _context.SaveChangesAsync();
           }

           catch (Exception ex)
           {
               response.Success = false;
               response.Message = ex.Message;
               
           }
           return response;
        }
    }
}