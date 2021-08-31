using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using myapp.Data;
using myapp.Dtos.Character;
using myapp.Models;

namespace myapp.Services.CharacterServices
{
    public class CharacterServices : ICharacterServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public CharacterServices(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;

            _mapper = mapper;

        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters
            .Where(c => c.User.Id == GetUserId())
            .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int Id)
        {
            {
                var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
                try
                {
                    Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == Id && c.User.Id == GetUserId());
                        if(character != null)
                        {
                    _context.Characters.Remove(character);
                    
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = _context.Characters
                    .Where(c => c.User.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                        }
                        else{
                            serviceResponse.Success = false;
                            serviceResponse.Message = "Character not found";
                        }
                }
                catch (Exception ex)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = ex.Message;
                }
                return serviceResponse;


            }
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        { 
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters
            .Include(c=> c.Weapon)
            .Include(c=> c.Skill)
            .Where(c => c.User.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacters = await _context.Characters
            .Include(c=> c.Weapon)
            .Include(c=> c.Skill)
            .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                if(character.User.Id == GetUserId())
                {
                character.Name = updatedCharacter.Name;
                character.Hitpoints = updatedCharacter.Hitpoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                }
                else 
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not Found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;


        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
           var response = new ServiceResponse<GetCharacterDto>();
            try{
                var character = await _context.Characters
                .Include(c=> c.Weapon)
                .Include(c=> c.Skill)
                .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId());
                if(character == null)
                {
                    response.Success = false;
                    response.Message = "Character not Found.";
                    return response;
                }

                var  skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if(skill == null)
                {
                    response.Success = false;
                     response.Message = "Skill not found.";
                     return response;
                }

                    character.Skill.Add(skill);
                    await _context.SaveChangesAsync();
                    response.Data =     _mapper.Map<GetCharacterDto>(character);
            }
            
            catch(Exception ex){
                response.Success = false;
                response.Message = ex.Message; 
            }
            return response;
        }
    }
}