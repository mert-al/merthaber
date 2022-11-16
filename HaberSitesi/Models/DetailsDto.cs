using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesi.Models
{
    public class DetailsDto<T> where T: BaseEntity
    {
        public T Item { get; set; }
        public List<T> Sidebar { get; set; }
        public List<T> TrendingNow { get; set; }
        public Ad Ads { get; set; }
    }
}