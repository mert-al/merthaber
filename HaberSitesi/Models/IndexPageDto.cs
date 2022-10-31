using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesi.Models
{
    public class IndexPageDto
    {
        public List<NewsAlbumMix> Items { get; set; }
        public List<NewsAlbumMix> TopHit { get; set; }
        public List<Video> Videos { get; set; }
        public List<News> TrendingNow { get; set; }
    }
}