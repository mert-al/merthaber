using DataAccess;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesi.Services
{
    public class ReviewServices
    {
        UnitOfWork _unitOfWork;
        public ReviewServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public void Create(Review review)
        {
            try
            {
                review.isActive = true;
                _unitOfWork.ReviewRepository.Insert(review);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}