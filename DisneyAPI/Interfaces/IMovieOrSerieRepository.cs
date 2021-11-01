using DisneyAPI.Entities;
using System.Collections.Generic;

namespace DisneyAPI.Interfaces
{
    public interface IMovieOrSerieRepository : IRepository<MovieOrSerie>
    {
        MovieOrSerie GetMovieOrSerie(int id);
        List<MovieOrSerie> GetAllMoviesOrSeries();

    }
}
