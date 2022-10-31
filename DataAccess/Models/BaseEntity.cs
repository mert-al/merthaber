using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Son Güncelleme Tarihi")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Aktif")]
        [DefaultValue(true)]
        public bool isActive { get; set; } = false;
        public bool isDeleted { get; set; } = false;
    }
}
