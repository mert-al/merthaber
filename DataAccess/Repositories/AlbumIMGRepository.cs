using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AlbumIMGRepository : GenericRepository<AlbumIMG>
    {
        NewsDBContext _db;
        public AlbumIMGRepository(NewsDBContext context)
        {
            _db = context;
        }
    }
}
