using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesiAdmin.Models
{
    public class PageDTO<T> where T: BaseEntity
    {
        public List<T> Items { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public SelectList PageSizeOptions = new SelectList(new[] { 1, 5, 10, 20, 50, 100, 200, 500 });
        public String SearchQuery { get; set; }
        public Pager Pager { get; set; }

        //public PageDTO(List<T> items = null)
        //{
        //    if (items != null)
        //    {
        //        Pager = new Pager(items.Count());
        //    }
        //}
    }
}