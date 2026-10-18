using System;
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
    public partial class QuestionDialog : Form
    {
        public QuestionDialog()
        {
            InitializeComponent();
            counter = 30; //Variable for the timer to countdown from
            lbltimer.Text = counter.ToString(); //display the timer's number
        }

        private void QuestionDialog_Load(object sender, EventArgs e)
        {
            
        }
 
        string questionReceived; //string for the question
        
        public void ReceiveQandA(string qReceived, string ansReceived, string score)
        {
            lblQuestion.Text = qReceived; //Display question on label
            lblAnsw.Text = ansReceived; //Display answ on label
            lblPoints.Text = score; //display score on lbl
        }

        private void btnReveal_Click(object sender, EventArgs e)
        {
            lblAnsw.Visible = true; //Make label visible
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK; //This will close the window when pressed
        }

        private int counter; //variable for the counter the timer uses
        private void timer1_Tick(object sender, EventArgs e)
        {
            counter--; //decrease the counter on 1
            lbltimer.Text = counter.ToString(); //display time on label
            if (counter == 0) { timer1.Stop(); } //stop timer if it reaches 0
        }

        private void btnStartTimer_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void btnStopTimer_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer1.Stop();
        }

        private void QuestionDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
