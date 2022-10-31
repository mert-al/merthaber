using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesiAdmin.Models
{
    public class UpdatedImages
    {
        public HttpPostedFileBase File { get; set; }
        public int AlbumImgId { get; set; }
    }
}