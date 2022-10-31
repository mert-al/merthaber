using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Repositories;
using DataAccess;
using HaberSitesiAdmin.Models;

namespace HaberSitesiAdmin.Services
{
    public class RoleServices
    {
        UnitOfWork _unitOfWork;
        public RoleServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<SelectListItem> GetRolesSelectList()
        {
            List<SelectListItem> roleList = new List<SelectListItem>();
            List<Role> roles = _unitOfWork.RoleRepository.GetAll(true).ToList();

            foreach (Role item in roles)
            {
                roleList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            return roleList;
        }

        public List<SelectListItem> GetRolesSelectListByUser(User user)
        {
            List<SelectListItem> roleList = new List<SelectListItem>();
            List<Role> roles = _unitOfWork.RoleRepository.GetAll(true).ToList();

            foreach (Role item in roles)
            {
                roleList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = user.Role.Id == item.Id 
                });
            }
            return roleList;
        }

        public List<Role> GetAll()
        {
            try
            {
                return _unitOfWork.RoleRepository.GetAll().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Role Get(int id)
        {
            try
            {
                return _unitOfWork.RoleRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PageDTO<Role> GetPage(PageDTO<Role> pageDTO)
        {
            try
            {
                var searchStatus = String.IsNullOrWhiteSpace(pageDTO.SearchQuery);
                var items = _unitOfWork.RoleRepository.GetAll().Where(model => (searchStatus || model.Name.Contains(pageDTO.SearchQuery)));
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

        public void Create(Role role)
        {
            try
            {
                _unitOfWork.RoleRepository.Insert(role);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(Role role)
        {
            try
            {
                role.Users = _unitOfWork.RoleRepository.GetUsers(role.Id);
                _unitOfWork.RoleRepository.Update(role);
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
                _unitOfWork.RoleRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}