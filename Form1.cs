using ExcelDataReader;
using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeopardy
{
    public partial class Form1 : Form
    {
        //Controls added programmatically
        private Label lblCount = new Label(); //label1
        private TextBox txtBox = new TextBox(); //txtbox1
        private Button btnOK1 = new Button(); //button OK1
        private Button btnFail = new Button(); //button fail1
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                  
        }
        
        public void OpenFile()
        {
            
        }

        Random rand = new Random(); //Create an object for random numbers
        List<int> listQuestions = new List<int>(); //Create a new list where the numbers will be saved
        List<int> listAnswers = new List<int>(); //Create a new list where the numbers will be saved
        DataTableCollection dataTableCollection; //Create object for opening excel file
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            int number; //variable for every time a new rand number is generated

            //For cicle to determine the amount of numbers in the list
            for (int i = 0; i < 59; i++)
            {
                //do while cycle to execute rand generator and not repeat values
                do
                {
                    number = rand.Next(0, 59);
                } while (listQuestions.Contains(number) | listAnswers.Contains(number));

                if (number % 2 == 0)
                    listQuestions.Add(number);
                else
                    listAnswers.Add(number);
            }

            
            //Open dialog to select excel files
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel 97-2003 Workbook|*.xls|Excel Workbook|*.xlsx" })
            {
                if (ofd.ShowDialog() == DialogResult.OK) //if there's a selected file & click OK
                {
                    txtFileName.Text = ofd.FileName; //Set file name to the text box in GUi
                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))//Parameters for opening a file. File.open opens a filestream with read/write access
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream)) //Read data from excel file using IExcelDataReader library
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() //Converts all sheets to a dataset
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            }
                            );
                            dataTableCollection = result.Tables; //Gets the collection of tables contained in the DataSet
                            comboData.DataSource = null;
                            comboSheet.Items.Clear(); //clear sheet combo box items
                            comboSheet.Items.AddRange(dataTableCollection.Cast<DataTable>().Select(t => t.TableName).ToArray<string>()); //Set sheets to arrays and add to sheet combo box

                            dt = dataTableCollection[comboSheet.Items[0].ToString()];//Get items from first array on sheet combo box and convert tot table

                            //dt = dataTableCollection[comboSheet.SelectedItem.ToString()];
                            var columnNames = (from c in dt.Columns.Cast<DataColumn>()
                                               select c.ColumnName).ToArray();
                                                        
                            label1.Text = columnNames[listQuestions[index0]]; //set excel column0 value to label1
                            label2.Text = columnNames[listQuestions[index1]]; //set excel column2 value to label2
                            label3.Text = columnNames[listQuestions[index2]]; //set excel column4 value to label3
                            label4.Text = columnNames[listQuestions[index3]]; //set excel column6 value to label4
                            label5.Text = columnNames[listQuestions[index4]]; //set excel column8 value to label5

                            //Get columns for questions
                            var data1 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index0]]); //Convert excel column to table

                            var data2 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index1]]); //Convert excel column to table
                            var data3 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index2]]); //Convert excel column to table
                            var data4 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index3]]); //Convert excel column to table
                            var data5 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index4]]); //Convert excel column to table
                            
                            //Get columns for answers
                            var data6 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index0] + 1]); //Convert excel column1 to table

                            var data7 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index1] + 1]); //Convert excel column1 to table
                            var data8 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index2] + 1]); //Convert excel column1 to table
                            var data9 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index3] + 1]); //Convert excel column1 to table
                            var data10 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index4] + 1]); //Convert excel column1 to table

                            //Set data to each cell
                            rowQuestion = data1.Select(); //Get rows from column table1
                            rowAnswer = data6.Select(); //Get rows from column table1
                            rowQuestion2 = data2.Select();
                            rowAnswer2 = data7.Select();
                            rowQuestion3 = data3.Select();
                            rowAnswer3 = data8.Select();
                            rowQuestion4 = data4.Select();
                            rowAnswer4 = data9.Select();
                            rowQuestion5 = data5.Select();
                            rowAnswer5 = data10.Select();
                                  
                        }
                    }
                }
            }
        }
        int index0 = 0;
        int index1 = 1;
        int index2 = 2;
        int index3 = 3;
        int index4 = 4;
        DataTable dt;
        DataColumn column;
        DataRow[] rowQuestion, rowQuestion2, rowQuestion3, rowQuestion4, rowQuestion5; //variable to save the rows with all the questions
        DataRow[] rowAnswer, rowAnswer2, rowAnswer3, rowAnswer4, rowAnswer5; //variable to save the rows with all the answers
        string question;
        string teamname; //vriable for saving the team name when writing it
        string score; //Variables for keeping score
        int scoreInt; //Variable for adding score

        private void btnClear_Click(object sender, EventArgs e)
        {
            comboTeams.Items.Clear(); //Delete all items on teams combobox
            //comboTeams.Items.Add("Teams"); //Rewrite header Team Name
            comboTeams.Text = "Teams"; //Set header again
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            teamname = txtTeamName.Text; //Get text from textbox
            comboTeams.Items.Add(teamname); //Add text to Teams comboBox
            txtTeamName.Clear(); //Clear team name txtBox
            comboTeams.Text = teamname; //Set the last teamname added to the comboTeams box display
        }
        int teamsArraylenght; //Variable for gtting the amount of items in teamscomboBox
        string text;
        private void btnSave_Click(object sender, EventArgs e)
        {
            teamsArraylenght = comboTeams.Items.Count; //Get the lenght of the array in comboTeams control
            for(int i = 0; i < teamsArraylenght; i++)
            {
                switch (i) 
                {
                    case 0: 
                        lblTeam1.Text = comboTeams.Items[i].ToString();
                        break;
                    case 1:
                        lblTeam2.Text = comboTeams.Items[i].ToString();
                        break;
                    case 2:
                        lblTeam3.Text = comboTeams.Items[i].ToString();
                        break;
                    case 3:
                        lblTeam4.Text = comboTeams.Items[i].ToString();
                        break;
                    case 4:
                        lblTeam5.Text = comboTeams.Items[i].ToString();
                        break;
                    case 5:
                        lblTeam6.Text = comboTeams.Items[i].ToString();
                        break;
                }
            }
        }
        
        private void btnPlus_Click(object sender, EventArgs e)
        {
            score = score1.Text; //Get text from scorebox
            scoreInt = AddPoints(score,scoreObtained); //Send text to function and score
            score1.Text = scoreInt.ToString(); //Convert int to string and display
            QuestionDialog qdialog = new QuestionDialog();   
        }

        //Function to add points
        private int AddPoints(string score, int scoreselected) //Get string of existing score, and selected score 
        {
            int x = int.Parse(score); 
            x = x + scoreselected;
            return x;
        }

        //Function to substract points
        private int SubstractPoints(string score, int scoreselected)
        {
            int y = int.Parse(score);
            y = y - scoreselected;
            return y;
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            score = score1.Text; //Get text from scorebox
            scoreInt = SubstractPoints(score,scoreObtained); //Send text to function and score obtained from button pressed
            score1.Text = scoreInt.ToString(); //Convert int to string and display
        }
        int scoreObtained;
        bool flag, flag2, flag3, flag4, flag5, flag6, flag7, flag8, flag9, flag10, flag11, flag12, flag13; //Flag to activate or deactivate the background image when button is pressed
        bool flag14, flag15, flag16, flag17, flag18, flag19, flag20, flag21, flag22, flag23, flag24, flag25;
        private void button1_Click(object sender, EventArgs e)
        {
            Buttons(100, rowQuestion[0][0], rowAnswer[0][0], button1);
            flag = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Buttons(100, rowQuestion2[0][0], rowAnswer2[0][0], button2);
            flag2 = true;   
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Buttons(100, rowQuestion3[0][0], rowAnswer3[0][0], button3);
            flag3 = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Buttons(100, rowQuestion4[0][0], rowAnswer4[0][0], button4);
            flag4 = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Buttons(100, rowQuestion5[0][0], rowAnswer5[0][0], button5);
            flag5 = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Buttons(200, rowQuestion[1][0], rowAnswer[1][0], button6);
            flag6 = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Buttons(200, rowQuestion2[1][0], rowAnswer2[1][0], button7);
            flag7 = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Buttons(200, rowQuestion3[1][0], rowAnswer3[1][0], button8);
            flag8 = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Buttons(200, rowQuestion4[1][0], rowAnswer4[1][0], button9);
            flag9 = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Buttons(200, rowQuestion5[1][0], rowAnswer5[1][0], button10);
            flag10 = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Buttons(300, rowQuestion[2][0], rowAnswer[2][0], button11);
            flag11 = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Buttons(300, rowQuestion2[2][0], rowAnswer2[2][0], button12);
            flag12 = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Buttons(300, rowQuestion3[2][0], rowAnswer3[2][0], button13);
            flag13 = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Buttons(300, rowQuestion4[2][0], rowAnswer4[2][0], button14);
            flag14 = true;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Buttons(300, rowQuestion5[2][0], rowAnswer5[2][0], button15);
            flag15 = true;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Buttons(400, rowQuestion[3][0], rowAnswer[3][0], button16);
            flag16 = true;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Buttons(400, rowQuestion2[3][0], rowAnswer2[3][0], button17);
            flag17 = true;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Buttons(400, rowQuestion3[3][0], rowAnswer3[3][0], button18);
            flag18 = true;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Buttons(400, rowQuestion4[3][0], rowAnswer4[3][0], button19);
            flag19 = true;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Buttons(400, rowQuestion5[3][0], rowAnswer5[3][0], button20);
            flag20 = true;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Buttons(500, rowQuestion[4][0], rowAnswer[4][0], button21);
            flag21 = true;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Buttons(500, rowQuestion2[4][0], rowAnswer2[4][0], button22);
            flag22 = true;
        }        

        private void button23_Click(object sender, EventArgs e)
        {
            Buttons(500, rowQuestion3[4][0], rowAnswer3[4][0], button23);
            flag23 = true;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Buttons(500, rowQuestion4[4][0], rowAnswer4[4][0], button24);
            flag24 = true;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Buttons(500, rowQuestion5[4][0], rowAnswer5[4][0], button25);
            flag25 = true;
        }

        private void btnMinus2_Click(object sender, EventArgs e)
        {
            score = score2.Text; //Get text from scorebox
            scoreInt = SubstractPoints(score, scoreObtained); //Send text to function and score obtained from button pressed
            score2.Text = scoreInt.ToString(); //Convert int to string and display
        }

        private void btnPlus2_Click(object sender, EventArgs e)
        {
            score = score2.Text; //Get text from scorebox
            scoreInt = AddPoints(score, scoreObtained); //Send text to function and score obtained from button pressed
            score2.Text = scoreInt.ToString(); //Convert int to string and display
        }
               
        private void button31_Click(object sender, EventArgs e)
        {
            score = score3.Text; //Get text from scorebox
            scoreInt = AddPoints(score, scoreObtained); //Send text to function and score obtained from button pressed
            score3.Text = scoreInt.ToString(); //Convert int to string and display
        }

        private void btnMinus3_Click(object sender, EventArgs e)
        {
            score = score3.Text; //Get text from scorebox
            scoreInt = SubstractPoints(score, scoreObtained); //Send text to function and score obtained from button pressed
            score3.Text = scoreInt.ToString(); //Convert int to string and display
        }

        private void btnPlus3_Click(object sender, EventArgs e)
        {
            score = score3.Text; //Get text from scorebox
            scoreInt = AddPoints(score, scoreObtained); //Send text to function and score obtained from button pressed
            score3.Text = scoreInt.ToString(); //Convert int to string and display
        }

        private void btnMinus4_Click(object sender, EventArgs e)
        {
            score = score4.Text; //Get text from scorebox
            scoreInt = SubstractPoints(score, scoreObtained); //Send text to function and score obtained from button pressed
            score4.Text = scoreInt.ToString(); //Convert int to string and display
        }

        private void btnPlus4_Click(object sender, EventArgs e)
        {
            score = score4.Text; //Get text from scorebox
            scoreInt = AddPoints(score, scoreObtained); //Send text to function and score obtained from button pressed
            score4.Text = scoreInt.ToString(); //Convert int to string and display
        }

        private void btnMinus5_Click(object sender, EventArgs e)
        {
            score = score5.Text; //Get text from scorebox
            scoreInt = SubstractPoints(score, scoreObtained); //Send text to function and score obtained from button pressed
            score5.Text = scoreInt.ToString(); //Convert int to string and display
        }

        private void btnPlus5_Click(object sender, EventArgs e)
        {
            score = score5.Text; //Get text from scorebox
            scoreInt = AddPoints(score, scoreObtained); //Send text to function and score obtained from button pressed
            score5.Text = scoreInt.ToString(); //Convert int to string and display
        }

        private void btnMinus6_Click(object sender, EventArgs e)
        {
            score = score6.Text; //Get text from scorebox
            scoreInt = SubstractPoints(score, scoreObtained); //Send text to function and score obtained from button pressed
            score6.Text = scoreInt.ToString(); //Convert int to string and display
        }

        private void btnPlus6_Click(object sender, EventArgs e)
        {
            score = score6.Text; //Get text from scorebox
            scoreInt = AddPoints(score, scoreObtained); //Send text to function and score obtained from button pressed
            score6.Text = scoreInt.ToString(); //Convert int to string and display
        }

        private void btnStart_MouseEnter(object sender, EventArgs e)
        {
            btnStart.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void btnStart_MouseLeave(object sender, EventArgs e)
        {
            btnStart.BackgroundImage = null;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            if(flag==false)
                button1.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {

            if (flag2 == false)
                button2.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            if (flag3 == false)
                button3.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            if (flag4 == false)
                button4.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            button5.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            if (flag5 == false)
                button5.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            button6.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            if (flag6 == false)
                button6.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            button7.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            if (flag7 == false)
                button7.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button8_MouseEnter(object sender, EventArgs e)
        {
            button8.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button8_MouseLeave(object sender, EventArgs e)
        {
            if (flag8 == false)
                button8.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button9_MouseEnter(object sender, EventArgs e)
        {
            button9.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button9_MouseLeave(object sender, EventArgs e)
        {
            if (flag9 == false)
                button9.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button10_MouseEnter(object sender, EventArgs e)
        {
            button10.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button10_MouseLeave(object sender, EventArgs e)
        {
            if (flag10 == false)
                button10.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button11_MouseEnter(object sender, EventArgs e)
        {
            button11.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button11_MouseLeave(object sender, EventArgs e)
        {
            if (flag11 == false)
                button11.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button12_MouseEnter(object sender, EventArgs e)
        {
            button12.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button12_MouseLeave(object sender, EventArgs e)
        {
            if (flag12 == false)
                button12.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button13_MouseEnter(object sender, EventArgs e)
        {
            button13.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button13_MouseLeave(object sender, EventArgs e)
        {
            if (flag13 == false)
                button13.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button14_MouseEnter(object sender, EventArgs e)
        {
            button14.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button14_MouseLeave(object sender, EventArgs e)
        {
            if (flag14 == false)
                button14.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button15_MouseEnter(object sender, EventArgs e)
        {
            button15.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button15_MouseLeave(object sender, EventArgs e)
        {
            if (flag15 == false)
                button15.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button16_MouseEnter(object sender, EventArgs e)
        {
            button16.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button16_MouseLeave(object sender, EventArgs e)
        {
            if (flag16 == false)
                button16.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button26_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button17_MouseEnter(object sender, EventArgs e)
        {
            button17.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button17_MouseLeave(object sender, EventArgs e)
        {
            if (flag17 == false)
                button17.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button18_MouseEnter(object sender, EventArgs e)
        {
            button18.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button18_MouseLeave(object sender, EventArgs e)
        {
            if (flag18 == false)
                button18.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button19_MouseEnter(object sender, EventArgs e)
        {
            button19.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button19_MouseLeave(object sender, EventArgs e)
        {
            if (flag19 == false)
                button19.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button20_MouseEnter(object sender, EventArgs e)
        {
            button20.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button20_MouseLeave(object sender, EventArgs e)
        {
            if (flag20 == false)
                button20.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button21_MouseEnter(object sender, EventArgs e)
        {
            button21.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button21_MouseLeave(object sender, EventArgs e)
        {
            if (flag21 == false)
                button21.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button22_MouseEnter(object sender, EventArgs e)
        {
            button22.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button22_MouseLeave(object sender, EventArgs e)
        {
            if (flag22 == false)
                button22.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button23_MouseEnter(object sender, EventArgs e)
        {
            button23.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button23_MouseLeave(object sender, EventArgs e)
        {
            if (flag23 == false)
                button23.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button24_MouseEnter(object sender, EventArgs e)
        {
            button24.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button24_MouseLeave(object sender, EventArgs e)
        {
            if (flag24 == false)
                button24.BackgroundImage = Properties.Resources.blue_button2;
        }

        private void button25_MouseEnter(object sender, EventArgs e)
        {
            button25.BackgroundImage = Properties.Resources.blue_button3;
        }

        private void button25_MouseLeave(object sender, EventArgs e)
        {
            if (flag25 == false)
                button25.BackgroundImage = Properties.Resources.blue_button2;
        }      

        private void btnStart_Click(object sender, EventArgs e)
        {
            //Random rand = new Random(); //Create an object for random numbers
            //List<int> listNumbers = new List<int>(); //Create a new list where the numbers will be saved
            //int number; //variable for every time a new rand number is generated

            ////For cicle to determine the amount of numbers in the list
            //for (int i = 0; i < 10; i++) 
            //{
            //    //do while cycle to execute rand generator and not repeat values
            //    do 
            //    {
            //        while (true) //infinite while to get only uneven numbers
            //        {
            //            number = rand.Next(0, 19);
            //            if (number % 2 == 1)
            //                break;
            //        }
            //    } while (listNumbers.Contains(number));
            //    listNumbers.Add(number);
            //}

            

            //for (int j = 0; j <= 9; j++)
            //{
            //    listBox1.Items.Add(listNumbers[j]);
            //}
            //listBox1.Items.Add(listNumbers);
        }
        
        int cont = 1; //variable to keep counting the rounds 
        private void btnShuffle_Click(object sender, EventArgs e)
        {
            //set buttons color & enable them again
            button1.BackgroundImage = Properties.Resources.blue_button2;
            button1.Enabled = true;
            flag = false;
            button2.BackgroundImage = Properties.Resources.blue_button2;
            button2.Enabled = true;
            flag2 = false;
            button3.BackgroundImage = Properties.Resources.blue_button2;
            button3.Enabled = true;
            flag3 = false;
            button4.BackgroundImage = Properties.Resources.blue_button2;
            button4.Enabled = true;
            flag4 = false;
            button5.BackgroundImage = Properties.Resources.blue_button2;
            button5.Enabled = true;
            flag5 = false;
            button6.BackgroundImage = Properties.Resources.blue_button2;
            button6.Enabled = true;
            flag6 = false;
            button7.BackgroundImage = Properties.Resources.blue_button2;
            button7.Enabled = true;
            flag7 = false;
            button8.BackgroundImage = Properties.Resources.blue_button2;
            button8.Enabled = true;
            flag8 = false;
            button9.BackgroundImage = Properties.Resources.blue_button2;
            button9.Enabled = true;
            flag9 = false;
            button10.BackgroundImage = Properties.Resources.blue_button2;
            button10.Enabled = true;
            flag10 = false;
            button11.BackgroundImage = Properties.Resources.blue_button2;
            button11.Enabled = true;
            flag11 = false;
            button12.BackgroundImage = Properties.Resources.blue_button2;
            button12.Enabled = true;
            flag12 = false;
            button13.BackgroundImage = Properties.Resources.blue_button2;
            button13.Enabled = true;
            flag13 = false;
            button14.BackgroundImage = Properties.Resources.blue_button2;
            button14.Enabled = true;
            flag14 = false;
            button15.BackgroundImage = Properties.Resources.blue_button2;
            button15.Enabled = true;
            flag15 = false;
            button16.BackgroundImage = Properties.Resources.blue_button2;
            button16.Enabled = true;
            flag16 = false;
            button17.BackgroundImage = Properties.Resources.blue_button2;
            button17.Enabled = true;
            flag17 = false;
            button18.BackgroundImage = Properties.Resources.blue_button2;
            button18.Enabled = true;
            flag18 = false;
            button19.BackgroundImage = Properties.Resources.blue_button2;
            button19.Enabled = true;
            flag19 = false;
            button20.BackgroundImage = Properties.Resources.blue_button2;
            button20.Enabled = true;
            flag20 = false;
            button21.BackgroundImage = Properties.Resources.blue_button2;
            button21.Enabled = true;
            flag21 = false;
            button22.BackgroundImage = Properties.Resources.blue_button2;
            button22.Enabled = true;
            flag22 = false;
            button23.BackgroundImage = Properties.Resources.blue_button2;
            button23.Enabled = true;
            flag23 = false;
            button24.BackgroundImage = Properties.Resources.blue_button2;
            button24.Enabled = true;
            flag24 = false;
            button25.BackgroundImage = Properties.Resources.blue_button2;
            button25.Enabled = true;
            flag25 = false;
        
            //MessageBox.Show(cont.ToString()); //mesage box to print 
            cont++; //counter increments every time a new round is played
            index0 = index0 + 5;
            index1 = index1 + 5;
            index2 = index2 + 5;
            index3 = index3 + 5;
            index4 = index4 + 5;

            dt = dataTableCollection[comboSheet.Items[0].ToString()];

            //dt = dataTableCollection[comboSheet.SelectedItem.ToString()];
            var columnNames = (from c in dt.Columns.Cast<DataColumn>()
                               select c.ColumnName).ToArray();

            int totalElements = columnNames.Count(); //get the number of columns in the excel sheet (array previously declared)
            int limitRounds = totalElements / 10; //get the limit of rounds to play before error

            //if (cont > limitRounds) { MessageBox.Show("eres basura brock"); }
            if (cont > limitRounds) //if the counter reaches the limit of rounds
            {
                MessageBox.Show("GAME OVER"); //Mesagebox displaying game over

                int finalscore1 = int.Parse(score1.Text); //get final score from each textbox
                int finalscore2 = int.Parse(score2.Text);
                int finalscore3 = int.Parse(score3.Text);
                int finalscore4 = int.Parse(score4.Text);
                int finalscore5 = int.Parse(score5.Text);
                int finalscore6 = int.Parse(score6.Text);
                //int[] finalscores = { finalscore1, finalscore2, finalscore3, finalscore4, finalscore5, finalscore6 }; //save scores in one array
                string[,] finalScores = { { score1.Text, lblTeam1.Text }, { score2.Text, lblTeam2.Text }, { score3.Text, lblTeam3.Text }, { score4.Text, lblTeam4.Text }, { score5.Text, lblTeam5.Text }, { score6.Text, lblTeam6.Text } }; //save scores in one array

                    // 0 - dont sort
                    // 1 - sort ascending
                    // -1 - sort descendi
                string[,] sortedTable = new string[6, 2]; //array in which sorted values will be saved
                    //Array.Sort(finalscores); //sort array ascending
                    //Array.Reverse(finalscores); //reverse array to sort descending



                //CODE FOR OPENING THE FINAL SCORES WINDOW
                //
                //
                sortedTable = Sort_String(finalScores);  //Send scores to sort function
                FinalScores fscores = new FinalScores(); //create instance for final scores dialog
                fscores.Show();                          //Show new window with final scores
                fscores.ReceiveScores(sortedTable);      //send sorted table to the final scores script

                this.Close(); //close winforms
                Thread.Sleep(500);
            }

            label1.Text = columnNames[listQuestions[index0]]; //set excel column0 value to label1
            label2.Text = columnNames[listQuestions[index1]]; //set excel column2 value to label2
            label3.Text = columnNames[listQuestions[index2]]; //set excel column4 value to label3
            label4.Text = columnNames[listQuestions[index3]]; //set excel column6 value to label4
            label5.Text = columnNames[listQuestions[index4]]; //set excel column8 value to label5

            //Get columns for questions
            var data1 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index0]]); //Convert excel column to table

            var data2 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index1]]); //Convert excel column to table
            var data3 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index2]]); //Convert excel column to table
            var data4 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index3]]); //Convert excel column to table
            var data5 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index4]]); //Convert excel column to table

            //Get columns for answers
            var data6 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index0] + 1]); //Convert excel column1 to table
            var data7 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index1] + 1]); //Convert excel column1 to table
            var data8 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index2] + 1]); //Convert excel column1 to table
            var data9 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index3] + 1]); //Convert excel column1 to table
            var data10 = dt.DefaultView.ToTable(false, columnNames[listQuestions[index4] + 1]); //Convert excel column1 to table

            //Set data to each cell
            rowQuestion = data1.Select(); //Get rows from column table1
            rowAnswer = data6.Select(); //Get rows from column table1
            rowQuestion2 = data2.Select();
            rowAnswer2 = data7.Select();
            rowQuestion3 = data3.Select();
            rowAnswer3 = data8.Select();
            rowQuestion4 = data4.Select();
            rowAnswer4 = data9.Select();
            rowQuestion5 = data5.Select();
            rowAnswer5 = data10.Select();
        }
        
        //function to execute every time a button is pressed
        private void Buttons(int score, object dRow, object ansRow, Control buttonNumber)
        {
            QuestionDialog qdialog = new QuestionDialog(); //Create instance for question dialog
            scoreObtained = score; //score 500
            qdialog.ReceiveQandA(dRow.ToString(), ansRow.ToString(), scoreObtained.ToString()); //Send cell value to ReceiveQandA function in questionDialog form
            buttonNumber.BackColor = Color.SteelBlue;
            buttonNumber.Enabled = false;
            buttonNumber.BackgroundImage = Properties.Resources.blue_button3; //change bckgnd image
            //flagsent = 1; //activate flag
            qdialog.Show(); //show question dialog
            AnswerDialog aDialog = new AnswerDialog(); //Create instance for answ dialog
            aDialog.ReceiveAns(ansRow.ToString()); //Send answer to answer forms
            aDialog.Show(); //Show answ dialog
        }

        //Class for sorting the keys in the array values
        public string [,] Sort_String(string[,] array)
        {
            //var dict = new
            //SortedDictionary<int, string>(); //create a new sorted dictionary
            var dict = new SortedDictionary<int, string>(new DuplicateKeyComparator<int>()); //create a new sorted dictionary & call for duplicate comparator class

            dict.Add(int.Parse(array[0, 0]), array[0, 1]);
            dict.Add(int.Parse(array[1, 0]), array[1, 1]);
            dict.Add(int.Parse(array[2, 0]), array[2, 1]);
            dict.Add(int.Parse(array[3, 0]), array[3, 1]);
            dict.Add(int.Parse(array[4, 0]), array[4, 1]);
            dict.Add(int.Parse(array[5, 0]), array[5, 1]);
            //dict.Add(int.Parse(array[6, 0]), array[6, 1]);

            List<string> listkeys = new List<string>(); //create new list for numbers
            List<string> listvalues = new List<string>(); //create new list for names
            foreach(var st in dict)
            {
                listkeys.Add(st.Key.ToString()); //add the key to the list
                listvalues.Add(st.Value); //add name on the same row to the names list
            }

            for(int i = 0; i<array.GetLength(0); i++)
            {
                array[i, 0] = listkeys[i];
                array[i, 1] = listvalues[i];
            }

            return array;
        }

        //class to compare duplicate values in the sortdictionary key array
        public class DuplicateKeyComparator<TKey> : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y); //function to compare values and save it in variable
                if (result == 0) //if not equals
                {
                    return 1; //true
                }
                else
                {
                    return result; 
                }
            }
        }


    }
}
