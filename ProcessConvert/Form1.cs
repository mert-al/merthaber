using NReco.VideoConverter;
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
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand com = new SqlCommand();
        DataSet ds = new DataSet();
        private void Form1_Load(object sender, EventArgs e)
        {
            while (1 == 1)
            {
                NewMethod();
                System.Threading.Thread.Sleep(5000);
            }
        }

        private void NewMethod()
        {
            con = new SqlConnection(@"Server=TESTWEBDB\QA;Database= HaberSitesi;User Id=db_testadmin;Password=sabahsoft");
           
            da = new SqlDataAdapter("Select title,embedUrl,ProcessingStatus From Videos where ProcessingStatus=1 ", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "Videos");
            dataGridView1.DataSource = ds.Tables["Videos"];           
            string embedUrl = dataGridView1.Rows[0].Cells[1].Value.ToString();
            var process = dataGridView1.Rows[0].Cells[2].Value.ToString();
            label1.Text = dataGridView1.Rows[0].Cells[2].Value.ToString();
            
            string newVideoFilePath = @"C:\Users\enes.sara\source\repos\mert-al\merthaber\HaberSitesiAdmin\" + "Storage\\Video\\Videos\\" + Guid.NewGuid().ToString() + ".mp4";
           
            var videoPath = @"C:\Users\enes.sara\source\repos\mert-al\merthaber\HaberSitesiAdmin\" + embedUrl;

                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                Task task = new Task(() => ffMpeg.ConvertMedia(videoPath, newVideoFilePath, Format.mp4));
                task.Start();

            if (process == "0")
            {

                var procesbir = new SqlCommand("UPDATE Videos SET ProcessingStatus = 1 where ProcessingStatus=0",con);
                procesbir.ExecuteNonQuery();

            }
            MessageBox.Show("connect with sql server");

            var updatevideopath = newVideoFilePath.Replace(@"C:\Users\enes.sara\source\repos\mert-al\merthaber\HaberSitesiAdmin\", "");

            if (process=="1")
            {
                var processiki = new SqlCommand("UPDATE Videos SET ProcessingStatus = 2, EmbedUrl ='" + updatevideopath + "' WHERE ProcessingStatus= 1 ", con);
                processiki.ExecuteNonQuery();
                MessageBox.Show("convert edildi");

            }


            con.Close();
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
           
        //}
    }
}
