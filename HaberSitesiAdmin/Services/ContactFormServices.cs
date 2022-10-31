using DataAccess;
using DataAccess.Repositories;
using HaberSitesiAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesiAdmin.Services
{
    public class ContactFormServices
    {
        UnitOfWork _unitOfWork;
        public ContactFormServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<ContactForm> GetAll()
        {
            try
            {
                return _unitOfWork.ContactFormRepository.GetAll().OrderBy(model => model.isRead).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ContactForm Get(int id)
        {
            try
            {
                return _unitOfWork.ContactFormRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PageDTO<ContactForm> GetPage(PageDTO<ContactForm> pageDTO)
        {
            try
            {
                var searchStatus = String.IsNullOrWhiteSpace(pageDTO.SearchQuery);
                var items = _unitOfWork.ContactFormRepository.GetAll().Where(model => (searchStatus || model.Subject.Contains(pageDTO.SearchQuery)));
                pageDTO.Pager = new Pager(items.Count(), pageDTO.Index, pageDTO.PageSize, 10);
                if(items.Count() != 0)
                {
                    pageDTO.Items = items.OrderBy(model => model.isRead).Skip(pageDTO.Pager.StartIndex).Take(pageDTO.PageSize).ToList();
                }
                return pageDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ContactForm GetDetails(int id)
        {
            try
            {
                return _unitOfWork.ContactFormRepository.GetDetails(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _unitOfWork.ContactFormRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}