using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{



    public partial class Form2 : Form
    {
        private SqlConnection conn;//sqlconnection variable
        private string date;//string for date timestamp
        private SqlCommand cmd;//sqlcommand object
        private static bool change;//variable to notify change in table
        private DataGridView dataGridCell2;//datagrid

        //Constructor accepts arguments for variables passed in on the main form
        public Form2(SqlConnection conn2, SqlCommand cmd2, DataGridView dataGridCell2)
        {
            //do proper assignment to have private variables initiailzed and assigned values.
            conn = conn2;
            this.cmd = cmd2;
            this.dataGridCell2 = dataGridCell2;
            InitializeComponent();
            change = false;
            initializeTextBox();

        }
      /// <summary>
      /// This method initializes text boxes with text instructins
      /// </summary>
        public void initializeTextBox()
        {
            textBox2.Text = "Enter Definition";
            textBox3.Text = "Enter Word";
            textBox1.Text = "Enter Example Sentence";
        }
       
        protected void InsertData()
        {
            string sql = "Insert into AllWordTable values (@Word, @Meaning, @Example,@Time)";

            //cmd = new SqlCommand(sql);
            cmd = new SqlCommand();
            cmd.CommandText = sql;
            try
            {
                date = DateTime.Now.ToString();//turn date into string
                Regex expr = new Regex("[a-zA-Z]");//use regular expressions to check for input validity
                if (!expr.IsMatch(textBox1.Text) || !expr.IsMatch(textBox3.Text) || !expr.IsMatch(textBox2.Text))
                {
                    MessageBox.Show("Invalid character(s) in one or more fields. Please use alphabetic characters only, and leave no field empty.");
                    return;
                }
                else
                {
                    conn.Open();//open connection
                    cmd.Connection = conn;//assign initialize cmd.Connection with conn
                    //Add new rows to AddWordTable
                    cmd.Parameters.AddWithValue("@Word", textBox3.Text.ToString());
                    cmd.Parameters.AddWithValue("@Meaning", textBox2.Text.ToString());
                    cmd.Parameters.AddWithValue("@Example", textBox1.Text.ToString());
                    cmd.Parameters.AddWithValue("@Time", date);
                    cmd.ExecuteNonQuery();//execute statement
                    change = true;//set change to true since table has been modified
                    
                }
            }
            catch (SqlException e)//catch sqlexception
            {
                MessageBox.Show(e.Message);//Pop messagebox detailing exception
            }
            finally
            {
                conn.Close();//close connection
            }
            if (change)//if change is true update datagrid
                InsertInGrid();
        }

        /// <summary>
        /// This function inserts the text from the textboxes and inserts it into the datagrid
        /// </summary>
        private void InsertInGrid()
        {
            int n = dataGridCell2.Rows.Add();//add row
            //add word,definition,sentence,date to cells in row
            dataGridCell2.Rows[n].Cells[0].Value = textBox3.Text.ToString();
            dataGridCell2.Rows[n].Cells[1].Value = textBox2.Text.ToString();
            dataGridCell2.Rows[n].Cells[2].Value = textBox1.Text.ToString();
            dataGridCell2.Rows[n].Cells[3].Value = date;
        }


        /// <summary>
        /// This is the save button on form2. When clicked a message box appears for confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Add word entry to database?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)//if yes is choosen
            {
                InsertData();//call insertdata function to insert into database
                textBox1.Clear();//clear textboxes
                textBox2.Clear();
                textBox3.Clear();
                initializeTextBox();//re insert text instructions
            }
            else
            {
                return;
            }

        }
        /// <summary>
        /// when text box is clicked the instruction text disappears
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox3_Enter(object sender, EventArgs e)
        {
            textBox3.Clear();
        }
        /// <summary>
        /// when text box is clicked the instruciton text disappears
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_Enter_1(object sender, EventArgs e)
        {
            textBox2.Clear();
        }
        /// <summary>
        /// when text box is clicked the instruction text disappears
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics title = e.Graphics;//Initilize title object
            //Font f = new Font("Arial", 10, FontStyle.Regular);
            //SizeF size = title.MeasureString("Vocabulary Builder", f);
            Pen p = new Pen(Color.Black); //pen is needed for drawing line
            int x1, y1, x2, y2;
            // left side border
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = ClientSize.Height;
            for (int i = 0; i < 10; i++)
            {
                title.DrawLine(p, x1, y1, x2, y2);
                x1 += 1;
                y1 += 1;
                x2 += 1;
                y2 -= 1;
            }
            // top border
            x1 = 0;
            y1 = 0;
            x2 = ClientSize.Width;
            y2 = 0;
            for (int i = 0; i < 10; i++)
            {
                title.DrawLine(p, x1, y1, x2, y2);
                x1 += 1;
                y1 += 1;
                x2 -= 1;
                y2 += 1;
            }
            p.Color = Color.Black;
            // Right side border
            x1 = ClientSize.Width;
            y1 = 0;
            x2 = ClientSize.Width;
            y2 = ClientSize.Height;
            for (int i = 0; i < 10; i++)
            {
                title.DrawLine(p, x1, y1, x2, y2);
                x1 -= 1;
                y1 += 1;
                x2 -= 1;
                y2 -= 1;
            }
            // Bottom border
            x1 = 0;
            y1 = ClientSize.Height;
            x2 = ClientSize.Width;
            y2 = ClientSize.Height;
            for (int i = 0; i < 10; i++)
            {
                title.DrawLine(p, x1, y1, x2, y2);
                x1 += 1;
                y1 -= 1;
                x2 -= 1;
                y2 -= 1;
            }




        }
    }

}