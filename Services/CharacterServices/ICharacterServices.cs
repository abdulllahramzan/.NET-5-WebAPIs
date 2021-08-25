using System.Collections.Generic;
using myapp.Models;

namespace myapp.Services.CharacterServices
{
    public interface ICharacterServices
    {
         List<Character> GetAllCharacters();
         Character GetCharacterById(int id);
         List<Character> AddCharacter(Character newCharacter);
    }
}