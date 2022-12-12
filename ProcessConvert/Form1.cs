using Dapper.Contrib.Extensions;
using Dapper;
using NReco.VideoConverter;
using ProcessConvert.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessConvert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand com = new SqlCommand();
        DataSet ds = new DataSet();
        private static string ConnectionString = @"Server=TESTWEBDB\QA;Database= HaberSitesi;User Id=db_testadmin;Password=sabahsoft";
        private void Form1_Load(object sender, EventArgs e)
        {
            while (1 == 1)
            {
                NewMethod();
                System.Threading.Thread.Sleep(15000);
            }
        }

        private async void NewMethod()
        {

            IEnumerable<Video> waitingForProcess = new List<Video>();


            using (var conn = new SqlConnection(ConnectionString))
            {
                waitingForProcess = conn.GetAll<Video>();
                waitingForProcess = waitingForProcess.Where(x => x.ProcessingStatus == 0).ToList();
            }

            string newVideoFilePath = @"C:\Users\mertali.cetin\Source\Repos\mert-al\merthaber\HaberSitesiAdmin\" + "Storage\\Video\\Videos\\" + Guid.NewGuid().ToString() + ".mp4";
            var updatevideopath = newVideoFilePath.Replace(@"C:\Users\mertali.cetin\Source\Repos\mert-al\merthaber\HaberSitesiAdmin\", "");


            foreach (var item in waitingForProcess)
            {
                var videoPath = @"C:\Users\mertali.cetin\Source\Repos\mert-al\merthaber\HaberSitesiAdmin\" + item.EmbedUrl;
                Task task = new Task(() => ProcessVideo(item, newVideoFilePath, videoPath, updatevideopath, conn));
                task.Start();
            }


        }

        private static void ProcessVideo(Video video, string newVideoFilePath, string videoPath, string updatevideopath, SqlConnection conn)
        {



            using (var con = new SqlConnection(ConnectionString))
            {
                video.ProcessingStatus = 1;
                con.Update<Video>(video);
            }
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            ffMpeg.ConvertMedia(videoPath, newVideoFilePath, Format.mp4);
            //MessageBox.Show("Video İşleme Başlandı");
            using (var con = new SqlConnection(ConnectionString))
            {
                video.ProcessingStatus = 2;
                con.Update<Video>(video);
            }
            //await conn.OpenAsync();
            //string sql = "update Videos set ProcessingStatus = 2 WHERE ProcessingStatus=1";
            //var resultss = conn.Execute(sql, new SqlConnection());
            //conn.Close();


        }



    }
}
