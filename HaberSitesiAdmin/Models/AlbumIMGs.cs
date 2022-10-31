using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesiAdmin.Models
{
    public class AlbumIMGs
    {
        public HttpPostedFileBase File { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
    }
}