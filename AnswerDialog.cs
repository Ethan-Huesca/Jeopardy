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
    public partial class AnswerDialog : Form
    {
        public AnswerDialog()
        {
            InitializeComponent();
        }

        private void AnswerDialog_Load(object sender, EventArgs e)
        {

        }

        string answerReceived;
        public void ReceiveAns(string ansReceived)
        {
            answerReceived = ansReceived.ToString(); //Receive string
            lblAnswer.Text = answerReceived; //Display question on label
            //lblAnsw.Text = ansReceived; //Display answ on label
            //lblPoints.Text = score;
        }
    }
}
