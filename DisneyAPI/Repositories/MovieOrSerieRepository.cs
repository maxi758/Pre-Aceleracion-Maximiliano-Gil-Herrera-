using Microsoft.EntityFrameworkCore;
using DisneyAPI.Contexts;
using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using DisneyAPI.Repositories;

namespace DisneyAPI.Repositories
{
    public class MovieOrSerieRepository : BaseRepository<MovieOrSerie, DisneyContext>, IMovieOrSerieRepository
    {
        public MovieOrSerieRepository(DisneyContext dbContext) : base(dbContext)
        {

        }

        public MovieOrSerie GetMovieOrSerie(int id)
        {
            return DbSet.Include(x => x.Characters).Include(x => x.Genre).FirstOrDefault(x => x.Id == id);
        }
        public List<MovieOrSerie> GetAllMoviesOrSeries()
        {
            return DbSet.Include(x => x.Characters).Include(x => x.Genre).ToList();
        }

    }
}
