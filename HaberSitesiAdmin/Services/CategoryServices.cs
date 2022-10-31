using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using DataAccess.Repositories;
using HaberSitesiAdmin.Models;

namespace HaberSitesiAdmin.Services
{
    public class CategoryServices
    {
        UnitOfWork _unitOfWork;
        public CategoryServices()
        {
            _unitOfWork = new UnitOfWork();
        }


        public List<SelectListItem> GetCategorySelectList()
        {
            List<Category> categories = _unitOfWork.CategoryRepository.GetAll(true).ToList();
            List<SelectListItem> categorySelectList = new List<SelectListItem>();
            foreach (Category item in categories)
            {
                categorySelectList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                });
            }
            return categorySelectList;
        }

        public List<SelectListItem> GetSelectListByVideo(Video video)
        {
            List<Category> categories = _unitOfWork.CategoryRepository.GetAll(true).ToList();
            List<SelectListItem> categorySelectList = new List<SelectListItem>();
            foreach (Category item in categories)
            {
                categorySelectList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = video.Categories.Where(x => x.Id == item.Id).Any()
                });
            }
            return categorySelectList;
        }

        public List<Category> GetAll()
        {
            try
            {
                return _unitOfWork.CategoryRepository.GetAll().ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Category Get(int id)
        {
            try
            {
                return _unitOfWork.CategoryRepository.Get(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PageDTO<Category> GetPage(PageDTO<Category> pageDTO)
        {
            try
            {
                var searchStatus = String.IsNullOrWhiteSpace(pageDTO.SearchQuery);
                var items = _unitOfWork.CategoryRepository.GetAll().Where(model => (searchStatus || model.Name.Contains(pageDTO.SearchQuery)));
                pageDTO.Pager = new Pager(items.Count(), pageDTO.Index, pageDTO.PageSize, 10);
                if (items.Count() != 0)
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

        public void Create(Category category)
        {
            try
            {
                category.url = Regex.Replace(category.Name, @"[^0-9a-zA-Z\._ŞşığüçöİÜĞÇÖI]", "-");
                _unitOfWork.CategoryRepository.Insert(category);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(Category category)
        {
            try
            {
                category.url = Regex.Replace(category.Name, @"[^0-9a-zA-Z\._ŞşığüçöİÜĞÇÖI]", "-");
                _unitOfWork.CategoryRepository.Update(category);
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
                _unitOfWork.CategoryRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}