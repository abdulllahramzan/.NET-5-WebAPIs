using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using myapp.Models;
using myapp.Services.CharacterServices;

namespace myapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterServices _characterService;
        public CharacterController(ICharacterServices characterServices)
        {
            
            _characterService = characterServices;
        }


        [HttpGet("GetAll")]
        public ActionResult<List<Character>> Get()
        {
            return Ok(_characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public ActionResult<Character> GetSingle(int id)
        {
            return Ok(_characterService.GetCharacterById(id));
        }


        [HttpPost]
        public ActionResult<List<Character>> AddCharacter(Character newCharacter)
        {
            
            return Ok(_characterService.AddCharacter(newCharacter));
        }
    }
}