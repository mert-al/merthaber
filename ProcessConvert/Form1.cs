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
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.Diagnostics;

namespace ProcessConvert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static bool isHistoryActive = false;
        SqlConnection conn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand com = new SqlCommand();
        DataSet ds = new DataSet();
        private static string ConnectionString = @"Server=TESTWEBDB\QA;Database= HaberSitesi;User Id=db_testadmin;Password=sabahsoft";
        private async void Form1_Load(object sender, EventArgs e)
        {

            while (1 == 1)
            {

                if (isHistoryActive == false)
                {
                    NewMethod();
                }
                await Task.Delay(15000);

            }
            //System.Threading.Thread.Sleep(15000);


        }


        private async void NewMethod()
        {

            IEnumerable<Video> waitingForProcess = new List<Video>();


            using (var conn = new SqlConnection(ConnectionString))
            {
                waitingForProcess = conn.GetAll<Video>();
                waitingForProcess = waitingForProcess.Where(x => x.ProcessingStatus == 0).ToList();
            }

            dataGridView1.DataSource = waitingForProcess.Where(x => x.ProcessingStatus == 0 || x.ProcessingStatus == 1).ToList();


            foreach (var item in waitingForProcess)
            {

                var videoPath = @"C:\Users\enes.sara\Source\Repos\mert-al\merthaber\HaberSitesiAdmin" + item.EmbedUrl;
                string newVideoFilePath = @"C:\Users\enes.sara\Source\Repos\mert-al\merthaber\HaberSitesiAdmin\" + "Storage\\Video\\Videos\\" + Guid.NewGuid().ToString() + ".mp4";
                var updatevideopath = newVideoFilePath.Replace(@"C:\Users\enes.sara\Source\Repos\mert-al\merthaber\HaberSitesiAdmin\", "");
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
            var asd = "ffmpeg -i " + videoPath + " " + newVideoFilePath;
            Process.Start("cmd.exe", "/k" + "ffmpeg -i " + videoPath + " " + newVideoFilePath);




            var ffProbe = new NReco.VideoInfo.FFProbe();
            var videoInfo = ffProbe.GetMediaInfo(videoPath);
            var videoCodec = videoInfo.Streams[0].CodecLongName;

            //MessageBox.Show("Video İşleme Başlandı");
            using (var con = new SqlConnection(ConnectionString))
            {
                video.ProcessingStatus = 2;
                video.VideoTime = videoInfo.Duration.ToString();
                video.VideoCodec = videoCodec;
                video.EmbedUrl = updatevideopath;
                con.Update<Video>(video);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            IEnumerable<Video> waitingForProcess = new List<Video>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                waitingForProcess = conn.GetAll<Video>();
                waitingForProcess = waitingForProcess.Where(x => x.ProcessingStatus == 2).ToList();
            }
            Video video = new Video();
            if (radioButton2.Checked == true)
                isHistoryActive = true;
            dataGridView1.DataSource = waitingForProcess.ToList();

        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            IEnumerable<Video> waitingForProcesss = new List<Video>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                waitingForProcesss = conn.GetAll<Video>();
                waitingForProcesss = waitingForProcesss.Where(x => x.ProcessingStatus == 0 || x.ProcessingStatus == 1).ToList();
            }
            dataGridView1.DataSource = waitingForProcesss.ToList();
            if (radioButton2.Checked == true)
                isHistoryActive = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Video video = new Video();
            string newVideoFilePath = @"C:\Users\enes.sara\Source\Repos\mert-al\merthaber\HaberSitesiAdmin\" + @"Storage\Video\AudioFile\" + Guid.NewGuid().ToString() + ".mp3";
            var updatevideopaths = newVideoFilePath.Replace(@"C:\Users\enes.sara\Source\Repos\mert-al\merthaber\HaberSitesiAdmin\", "");

            foreach (DataGridViewRow drow in dataGridView1.SelectedRows)  //Seçili Satırları Silme
            {

                //var a = drow.Cells[0].Value;
                video.Id =(int)drow.Cells[0].Value;
                video.ProcessingStatus = (int)drow.Cells[1].Value;
                video.EmbedUrl = drow.Cells[2].Value.ToString();
                video.VideoTime = drow.Cells[3].Value.ToString();
                video.VideoCodec = drow.Cells[5].Value.ToString();
              
                string deneme = @"C:\Users\enes.sara\Source\Repos\mert-al\merthaber\HaberSitesiAdmin\" + drow.Cells[2].Value;
                Mp3Convert(video, deneme, newVideoFilePath, updatevideopaths);
            }





        }
        private static void Mp3Convert(Video video, string deneme, string newVideoFilePath, string updatevideopaths)
        {


            Process.Start("cmd.exe", "/k" + " ffmpeg -i " + deneme + " " + newVideoFilePath);

            // var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            //ffMpeg.ConvertMedia(videoPath, newVideoFilePath, "mp3");
            using (var con = new SqlConnection(ConnectionString))
            {
                video.AudioFile = updatevideopaths;
                con.Update<Video>(video);

            }


        }

    }
}