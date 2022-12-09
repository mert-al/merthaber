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
        private void Form1_Load(object sender, EventArgs e)
        {
            while (1 == 1)
            {
                NewMethod();
                System.Threading.Thread.Sleep(50000);
            }
        }

        private async void NewMethod()
        {

            //da = new SqlDataAdapter("Select title,embedUrl,ProcessingStatus From Videos where ProcessingStatus=0  ", con);
            //ds = new DataSet();
            IEnumerable<Video> waitingForProcess = new List<Video>();
            
         
            using (var conn = new SqlConnection(@"Server=TESTWEBDB\QA;Database= HaberSitesi;User Id=db_testadmin;Password=sabahsoft"))

            {
               
                waitingForProcess = conn.GetAll<Video>();
                waitingForProcess = waitingForProcess.Where(x => x.ProcessingStatus == 0).ToList();

                await conn.OpenAsync();
                string sql = "update Videos set ProcessingStatus = 1 WHERE ProcessingStatus=0";
                var results = conn.ExecuteAsync(sql, new SqlConnection());

            }
            
            string newVideoFilePath = @"C:\Users\enes.sara\source\repos\mert-al\merthaber\HaberSitesiAdmin\" + "Storage\\Video\\Videos\\" + Guid.NewGuid().ToString() + ".mp4";
            var updatevideopath = newVideoFilePath.Replace(@"C:\Users\enes.sara\source\repos\mert-al\merthaber\HaberSitesiAdmin\", "");

            //var FirstUpdate = new SqlCommand("UPDATE Videos SET ProcessingStatus = 1 where ProcessingStatus=0");
            //FirstUpdate.ExecuteNonQuery();
            
            foreach (var item in waitingForProcess)
            {

                var videoPath = @"C:\Users\enes.sara\source\repos\mert-al\merthaber\HaberSitesiAdmin\" + item.EmbedUrl;
                Task task = new Task(() => ProcessVideo(newVideoFilePath, videoPath, updatevideopath,conn));
                task.Start();

            }
            
            //con.conn.Close();Open();
            //da.Fill(ds, "Videos");
            //dataGridView1.DataSource = ds.Tables["Videos"];

            //string embedUrl = dataGridView1.Rows[0].Cells[1].Value.ToString();
            //var process = dataGridView1.Rows[0].Cells[2].Value.ToString();
            //var video Id = dataGridView1.Rows[0].Cells[0].Value.ToString();

            //  Tanımlamalar 
            //string newVideoFilePath = @"C:\Users\enes.sara\source\repos\mert-al\merthaber\HaberSitesiAdmin\" + "Storage\\Video\\Videos\\" + Guid.NewGuid().ToString() + ".mp4";




            //if (da != null)
            //{
            //    Task task = new Task(() => ProcessVideo(newVideoFilePath, videoPath));
            //    task.Start();

            //}

            //if (process == "0")
            //{

            //    var procesbir = new SqlCommand("UPDATE Videos SET ProcessingStatus = 1 where ProcessingStatus=0", con);
            //    procesbir.ExecuteNonQuery();

            //}
            //MessageBox.Show("Video İşleme Başlandı");

            //if (process=="1")
            //{
            //    var processiki = new SqlCommand("UPDATE Videos SET ProcessingStatus = 2,EmbedUrl ='" + updatevideopath + "'  WHERE ProcessingStatus=1 ", con);
            //    processiki.ExecuteNonQuery();
            //    MessageBox.Show("Video Dönüştürüldü");

            //}


     
        }

        private static void ProcessVideo(string newVideoFilePath, string videoPath, string updatevideopath,SqlConnection conn)
        {
            

            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            ffMpeg.ConvertMedia(videoPath, newVideoFilePath, Format.mp4);
            MessageBox.Show("Video İşleme Başlandı");
            string sql = "update Videos set ProcessingStatus = 2 WHERE ProcessingStatus=1";
            var resultss = conn.Execute(sql, new SqlConnection());



        }

        //public async void ConvertVideo(string videoPath, string newVideoFilePath,string process)
        //{

        //    var updatevideopath = newVideoFilePath.Replace(@"C:\Users\enes.sara\source\repos\mert-al\merthaber\HaberSitesiAdmin\", "");
        //    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();         
        //    ffMpeg.ConvertMedia(videoPath, newVideoFilePath, Format.mp4);         
        //    var processiki = new SqlCommand("UPDATE Videos SET ProcessingStatus = 2, EmbedUrl ='" + updatevideopath + "' WHERE ProcessingStatus= 1 ", con);
        //    processiki.ExecuteNonQuery();
        //    MessageBox.Show("convert edildi");


        //    con.Close();

    }
}
