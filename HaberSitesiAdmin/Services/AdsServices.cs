using DataAccess;
using DataAccess.Repositories;
using HaberSitesiAdmin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace HaberSitesiAdmin.Services
{
    public class AdsServices
    {
        UnitOfWork _unitOfWork;
        public AdsServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public void Create(Ad reklam)
        {
            try
            {
                User user = (User)HttpContext.Current.Session["User"];
               
                _unitOfWork.AdsRepository.Create(reklam,user.Id);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public String SaveVideo(String fileName, HttpPostedFileBase file)
        //{
        //    string _path = Path.Combine(HttpContext.Current.Server.MapPath("~/XMLFiles/"), file.FileName);
        //    string _url = Path.Combine("/XMLFiles/", file.FileName);
        //    file.SaveAs(_path);
        //    return _url;
        //}

        public Ad Get(int id)
        {
            try
            {
                return _unitOfWork.AdsRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(Ad reklam)
        {
            try
            {
                User user = (User)HttpContext.Current.Session["User"];
                _unitOfWork.AdsRepository.Update(reklam, user.Id);
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
                _unitOfWork.AdsRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public PageDTO<Ad> GetPage(PageDTO<Ad> pageDTO)
        {
            try
            {
                var searchStatus = String.IsNullOrWhiteSpace(pageDTO.SearchQuery);
                var items = _unitOfWork.AdsRepository.GetAll().Where(model => (searchStatus || model.PostrollTitle.Contains(pageDTO.SearchQuery)));
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

        //public void GeneraXML(Ad reklam)
        //{
        //    for (int i = 0; i <  i++)
        //    {
        //        GenerateXML(reklam);
        //    }
        //}



        public void GenerateXML(Ad reklam)
        {
            
            writeXml(reklam.Preroll,reklam.PrerolTitle);
            writeXml(reklam.Midroll,reklam.MidrollTitle);
            writeXml(reklam.Postroll, reklam.PostrollTitle);

        }

        private static void writeXml(string url, string name)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode xmlNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(xmlNode);
            XmlElement Vast = doc.CreateElement("VAST");
            (Vast).SetAttribute("version", "2.0");
            doc.AppendChild(Vast);



            XmlElement Ad = doc.CreateElement("Ad");
            (Ad).SetAttribute("id", "preroll-1");
            doc.DocumentElement.AppendChild(Ad);

            XmlNode InLine = doc.CreateElement("InLine");
            Ad.AppendChild(InLine);

            XmlNode AdSystem = doc.CreateElement("AdSystem");
            AdSystem.AppendChild(doc.CreateTextNode("2.0"));
            InLine.AppendChild(AdSystem);

            XmlNode AdTitle = doc.CreateElement("AdTitle");
            AdTitle.AppendChild(doc.CreateTextNode("5773100"));
            InLine.AppendChild(AdTitle);

            XmlNode Error = doc.CreateElement("Error");
            Error.AppendChild(doc.CreateTextNode("myErrorUrl.com"));
            InLine.AppendChild(Error);

            XmlNode Impression = doc.CreateElement("Impression");
            Impression.AppendChild(doc.CreateTextNode("http://example.com/trackingurl/impression"));
            InLine.AppendChild(Impression);


            XmlNode Creatives = doc.CreateElement("Creatives");
            InLine.AppendChild(Creatives);

            XmlNode Creative = doc.CreateElement("Creative");
            Creatives.AppendChild(Creative);

            XmlNode Linear = doc.CreateElement("Linear");
            Creative.AppendChild(Linear);


            XmlNode Duration = doc.CreateElement("Duration");
            Duration.AppendChild(doc.CreateTextNode("00:00:01"));
            Linear.AppendChild(Duration);


            XmlNode MediaFiles = doc.CreateElement("MediaFiles");
            Linear.AppendChild(MediaFiles);

            XmlElement MediaFile = doc.CreateElement("MediaFile");
            (MediaFile).SetAttribute("id", "5241");
            (MediaFile).SetAttribute("delivery", "progressive");
            (MediaFile).SetAttribute("type", "video/mp4");
            (MediaFile).SetAttribute("bitrate", "500");
            (MediaFile).SetAttribute("width", "400");
            (MediaFile).SetAttribute("height", "300");
            (MediaFile).SetAttribute("scalable", "1");
            (MediaFile).SetAttribute("maintainAspectRatio", "1");
            (MediaFile).SetAttribute("apiFramework", "VPAID");
            MediaFiles.AppendChild(MediaFile);

            XmlCDataSection CData1 = doc.CreateCDataSection(url);
            MediaFile.AppendChild(CData1);


            XmlNode CompanionAds = doc.CreateElement("CompanionAds");
            Creatives.AppendChild(CompanionAds);
            string data = $"<A onClick=\"var i= new Image(1,1); i.src=\"http://app.scanscout.com/ssframework/log/log.png?a=logitemaction&RI=573242&CbC=1&CbF=true&EC=0&RC=0&SmC=2&CbM=1.0E-5&VI=44cfc3b2382300cb751ba129fe51f46a&admode=preroll&PRI=7496075541100999745&RprC=5&ADsn=20&VcaI=192,197&RrC=1&VgI=44cfc3b2382300cb751ba129fe51f46a&AVI=142&Ust=ma&Uctry=us&CI=1247549&AC=4&PI=567&Udma=506&ADI=5773100&VclF=true \";\" HREF=\"http://vaseline.com\" target=\"_blank\"> <IMG SRC=\"http://media.scanscout.com/ads/vaseline300x250Companion.jpg \" BORDER=0 WIDTH=300 HEIGHT=250 ALT=\"Click Here\"> </A>";
            string data2 = $"<IMG src = \"http://app.scanscout.com/ssframework/log/log.png?a=logitemaction&RI=573242&CbC=1&CbF=true&EC=1&RC=0&SmC=2&CbM=1.0E-5&VI=44cfc3b2382300cb751ba129fe51f46a&admode=preroll&PRI=7496075541100999745&RprC=5&ADsn=20&VcaI=192,197&RrC=1&VgI=44cfc3b2382300cb751ba129fe51f46a&AVI=142&Ust=ma&Uctry=us&CI=1247549&AC=4&PI=567&Udma=506&ADI=5773100&VclF=true \" height = \"1\" width = \"1\" > ";

            string allData = data + data2;
            XmlElement Companion = doc.CreateElement("Companion");
            Companion.SetAttribute("height", "250");
            Companion.SetAttribute("width", "300");
            Companion.SetAttribute("id", "573242");
            CompanionAds.AppendChild(Companion);

            XmlNode HTMLResource = doc.CreateElement("HTMLResource");
            Companion.AppendChild(HTMLResource);

            XmlCDataSection CData = doc.CreateCDataSection(allData.ToString());
            HTMLResource.AppendChild(CData);
            Environment.CurrentDirectory = @"C:\Users\enes.sara\source\repos\mert-al\merthaber\HaberSitesi\";
        
            var basepath = Path.Combine(Environment.CurrentDirectory, @"XMLFiles\");
            if (!Directory.Exists(basepath))
            {
                Directory.CreateDirectory(basepath);
            }
            

            var newFileName = string.Format(name + ".xml");
            doc.Save(basepath + newFileName);
        }
    }

}
