using DataAccess;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesi.Services
{
    public class ContactFormServices
    {
        private UnitOfWork _unitOfWork;
        public ContactFormServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public void Create(ContactForm contactForm)
        {
            try
            {
                contactForm.isActive = true;
                _unitOfWork.ContactFormRepository.Insert(contactForm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}