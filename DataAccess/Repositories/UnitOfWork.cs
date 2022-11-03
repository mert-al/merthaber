using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private NewsDBContext context;
        private UserRepository userRepository;
        private RoleRepository roleRepository;
        private NewsRepository newsRepository;
        private VideoRepository videoRepository;
        private CategoryRepository categoryRepository;
        private ReviewRepository reviewRepository;
        private ContactFormRepository contactFormRepository;
        private AlbumRepository albumRepository;
        private AlbumIMGRepository albumIMGRepository;
        private AdsRepository adsRepository;

        private NewsDBContext Context
        {
            get {
                if(this.context == null)
                {
                    this.context = new NewsDBContext();
                }
                return context;
            }
        }
        public UserRepository UserRepository
        {
            get
            {

                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(Context);
                }
                return userRepository;
            }
        }

        public RoleRepository RoleRepository
        {
            get
            {

                if (this.roleRepository == null)
                {
                    this.roleRepository = new RoleRepository(Context);
                }
                return roleRepository;
            }
        }
        public AdsRepository AdsRepository
        {
            get
            {

                if (this.adsRepository == null)
                {
                    this.adsRepository = new AdsRepository(Context);
                }
                return adsRepository;
            }
        }

        public NewsRepository NewsRepository
        {
            get
            {

                if (this.newsRepository == null)
                {
                    this.newsRepository = new NewsRepository(Context);
                }
                return newsRepository;
            }
        }

        public VideoRepository VideoRepository
        {
            get
            {

                if (this.videoRepository == null)
                {
                    this.videoRepository = new VideoRepository(Context);
                }
                return videoRepository;
            }
        }

        public CategoryRepository CategoryRepository
        {
            get
            {

                if (this.categoryRepository == null)
                {
                    this.categoryRepository = new CategoryRepository(Context);
                }
                return categoryRepository;
            }
        }

        public ReviewRepository ReviewRepository
        {
            get
            {

                if (this.reviewRepository == null)
                {
                    this.reviewRepository = new ReviewRepository(Context);
                }
                return reviewRepository;
            }
        }

        public ContactFormRepository ContactFormRepository
        {
            get
            {

                if (this.contactFormRepository == null)
                {
                    this.contactFormRepository = new ContactFormRepository(Context);
                }
                return contactFormRepository;
            }
        }

        public AlbumRepository AlbumRepository
        {
            get
            {

                if (this.albumRepository == null)
                {
                    this.albumRepository = new AlbumRepository(Context);
                }
                return albumRepository;
            }
        }

        public AlbumIMGRepository AlbumIMGRepository
        {
            get
            {

                if (this.albumIMGRepository == null)
                {
                    this.albumIMGRepository = new AlbumIMGRepository(Context);
                }
                return albumIMGRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
