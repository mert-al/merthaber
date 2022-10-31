using DataAccess;
using DataAccess.Repositories;
using HaberSitesiAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesiAdmin.Services
{
    public class ReviewServices
    {
        UnitOfWork _unitOfWork;
        public ReviewServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<Review> GetAll()
        {
            try
            {
                return _unitOfWork.ReviewRepository.GetAll().OrderByDescending(model => model.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Review Get(int id)
        {
            try
            {
                return _unitOfWork.ReviewRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PageDTO<Review> GetPage(PageDTO<Review> pageDTO)
        {
            try
            {
                var searchStatus = String.IsNullOrWhiteSpace(pageDTO.SearchQuery);
                var items = _unitOfWork.ReviewRepository.GetAll().Where(model => (searchStatus || model.Title.Contains(pageDTO.SearchQuery)));
                pageDTO.Pager = new Pager(items.Count(), pageDTO.Index, pageDTO.PageSize, 10);
                if(items.Count() != 0)
                {
                    pageDTO.Items = items.OrderByDescending(model => model.CreatedDate).Skip(pageDTO.Pager.StartIndex).Take(pageDTO.PageSize).ToList();
                }
                return pageDTO;
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
                _unitOfWork.ReviewRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}