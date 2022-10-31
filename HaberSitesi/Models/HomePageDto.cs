using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;

namespace HaberSitesi.Models
{
    public class HomePageDto<T>
    {
        public List<T> Items { get; set; } 
        public List<T> TopHit { get; set; }
        public List<Video> Videos { get; set; }
        public List<T> TrendingNow { get; set; }
    }
}
