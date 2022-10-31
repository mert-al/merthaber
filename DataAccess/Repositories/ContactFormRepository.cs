using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ContactFormRepository : GenericRepository<ContactForm>
    {
        NewsDBContext _db;
        public ContactFormRepository(NewsDBContext context)
        {
            _db = context;
        }

        public ContactForm GetDetails(int id)
        {
            try
            {
                ContactForm contactForm = _db.ContactForms.Where(model => !model.isDeleted && model.Id == id).FirstOrDefault();
                if (!contactForm.isRead)
                {
                    contactForm.isRead = true;
                    contactForm.UpdatedDate = DateTime.Now;
                    _db.SaveChanges();
                }
                return contactForm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ContactForm> Search(string query, bool? isActive = false)
        {
            try
            {
                return _db.ContactForms.Where(model => !model.isDeleted && (model.isActive == isActive || model.isActive) && model.Subject.Contains(query)).OrderByDescending(model => model.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
