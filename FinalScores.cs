using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeopardy
{
    public partial class FinalScores : Form
    {
        string videoPath;
        string fileName;
        string path1 = @"c:\temp\MyTest.txt";
        public FinalScores()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //vlcControl1.Stop();
            this.Close();
        }

        public void ReceiveScores(string[,] scores)
        {
            //Thread.Sleep(1000);
            //videoPath = @"C:\Users\Mauricio\Documents\Csharp_VisualStudio_mamadas\Jeopardy\FireworksBckgnd.mp4";
            //fileName = "FireworksBckgnd.mp4";
            ////OpenFileDialog oFile1 = new OpenFileDialog();
            ////oFile1.Filter = "( *.mp4) | *.mp4";
            ////if (oFile1.ShowDialog() == DialogResult.OK)
            ////{
            ////    axVLCPlugin21.playlist.add(oFile1.FileName, oFile1.SafeFileName, null);
            ////}
            //axVLCPlugin21.playlist.add(videoPath, fileName, null);
            //MessageBox.Show("Filename: "+videoPath);
            //MessageBox.Show("SafeFileName: " + fileName);

            label1.Text = scores[5, 0];
            label2.Text = scores[5, 1];
            label3.Text = scores[4, 0];
            label4.Text = scores[4, 1];
            label5.Text = scores[3, 0];
            label6.Text = scores[3, 1];
            label7.Text = scores[2, 0];
            label8.Text = scores[2, 1];
            label9.Text = scores[1, 0];
            label10.Text = scores[1, 1];
            label11.Text = scores[0, 0];
            label12.Text = scores[0, 1];

            //axVLCPlugin21.playlist.play();
            //vlcControl1.Play(new Uri("https://www.learningcontainer.com/wp-content/uploads/2020/05/sample-mp4-file.mp4"));
            //vlcControl1.Play(Path.GetFullPath(videoPath));
            //for(int i = 5; i >= 0; i--)
            //{
            //    MessageBox.Show(scores[i, 0] + " " + scores[i, 1]);
            //}
            Task.Delay(3000); 
            //Thread.Sleep(5000);
            MessageBox.Show("wrecker");
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void FinalScores_Load(object sender, EventArgs e)
        {

        }
    }
}
