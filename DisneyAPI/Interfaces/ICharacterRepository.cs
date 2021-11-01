using DisneyAPI.Entities;
using System.Collections.Generic;

namespace DisneyAPI.Interfaces
{
    public interface ICharacterRepository : IRepository<Character>
    {
        Character GetCharacter(int id);
        List<Character> GetAllCharacters();
    }
}
