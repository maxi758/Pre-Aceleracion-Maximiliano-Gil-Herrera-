using Microsoft.EntityFrameworkCore;
using DisneyAPI.Contexts;
using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using DisneyAPI.Repositories;

namespace DisneyAPI.Repositories
{
    public class CharacterRepository : BaseRepository<Character, DisneyContext>, ICharacterRepository
    {
        public CharacterRepository(DisneyContext dbContext) : base(dbContext)
        {
        }
        
        public Character GetCharacter(int id)
        {
            return DbSet.Include(x => x.MovieOrSeries).FirstOrDefault(x => x.Id == id);
        }
        public List<Character> GetAllCharacters()
        {
            return DbSet.Include(x => x.MovieOrSeries).ToList();
        }
        
    }
}
