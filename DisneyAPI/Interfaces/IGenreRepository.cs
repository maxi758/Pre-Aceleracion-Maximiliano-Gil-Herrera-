using DisneyAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Genre GetGender(int id);
        List<Genre> GetAllGenders();
    }
}
