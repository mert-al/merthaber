using DataAccess;
using DataAccess.Repositories;
using HaberSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesi.Services
{
    public class AdServices
    {
        UnitOfWork _unitOfWork;
        public AdServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public DetailsDto<Ad> UptadetAds(Ad reklam)
        {
            try
            {
                DetailsDto<Ad> dtoNewsDetails = new DetailsDto<Ad>();
                if (dtoNewsDetails.Item != null)
                {
                    _unitOfWork.AdsRepository.UpdateHit(reklam);

                }
                return dtoNewsDetails;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           

        }


    }
}