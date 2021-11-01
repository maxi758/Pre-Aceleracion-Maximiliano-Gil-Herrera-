using Microsoft.EntityFrameworkCore;
using DisneyAPI.Contexts;
using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using DisneyAPI.Repositories;

namespace DisneyAPI.Repositories
{
    public class GenreRepository : BaseRepository<Genre, DisneyContext>, IGenreRepository
    {
        public GenreRepository(DisneyContext dbContext) : base(dbContext)
        {

        }

        public Genre GetGender(int id)
        {
            return DbSet.Include(x => x.MovieOrSeries).FirstOrDefault(x => x.Id == id);
        }
        public List<Genre> GetAllGenders()
        {
            return DbSet.Include(x => x.MovieOrSeries).ToList();
        }

    }
}
