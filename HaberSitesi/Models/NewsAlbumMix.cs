using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesi.Models
{
    public class NewsAlbumMix
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Img { get; set; } = @"/Storage\defaultNews.jpg";
        public DateTime PublishDate { get; set; }
        public String path { get; set; }
        public int Hit { get; set; } = 0;
        public string url { get; set; }
        public string MainSliderIMG { get; set; }
        public string SidebarIMG { get; set; }
        public string SliderBottomIMG { get; set; }
        public string BestWeeklyIMG { get; set; }
        public string BestWeeklySmIMG { get; set; }
        public string DetailsIMG { get; set; }
        public string OtherIMG { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

    }
}