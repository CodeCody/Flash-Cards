using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using ClassLibrary1;
using ClassLibrary2;


namespace WindowsFormsApplication2
{

   
    public partial class Form1 : Form
    {
        //private members of the form class that will be used
        //StreamWriter and StreamReader will be used to write and read a file
        private SqlCommand cmd;
        private SqlConnection conn;
        readonly public string connstr = "server=10.158.56.48;uid=net11;pwd=netword11;database=notebase11;";
        private FlashClass flashObject;
       
        //Graphics is for drawing the name of the software
        private Graphics title;
        bool[] clicked = new bool[3] {false,false,false};//Array for check clicked input text boxes
        public Form1()//Form constuctor intitiallizes the form1 object
        {
            CreateTable(2);
            flashObject = new FlashClass();
         
            InitializeComponent();//Initializes varias components within the form like buttons, and textboxes
            ResizeRedraw = true;
            //dictionaries for word entries
            conn = new SqlConnection(connstr);
            
            
            button4.Enabled = false;
            button7.Enabled = true;
            button8.Enabled = true;
            DestroyTable(0);
            DestroyTable(1);
          //check to see if AllWordTable exists
          //  DestroyTable(1);
            
            if (VerifyTableExistence1(1) == 0)
            {
                CreateTable(1);//insert 1 to signify AllWordTable
               
            }
            //check to see if FlashWordTable exists
            if(VerifyTableExistence1(2) == 0)
            {
                CreateTable(2);//insert 2 to signify FlashWordTable
                
            }
           
            display_table();//call display table function
        }

   

        //This method checks to see if a table exists it takes an int and returns an int
        public int VerifyTableExistence1(int num)
        {
            
                conn = new SqlConnection(connstr);//initialize connection
                cmd = new SqlCommand();//initialize sqlcommand

                cmd.Connection = conn;//initialize cmd.Connection
                conn.Open();//open connection
                StringBuilder text = new StringBuilder();//initialize string builder
                text.Append("Select Count(*) from sysObjects WHERE name =");//Append sql query to text

                if (num == 1)//if num is equal to 1
                    text.Append("'Customers'"); // append AllWordTable
                else
                    text.Append("'FlashWordTable'");//append FlashWordTable

                cmd.CommandText = text.ToString();//assign command text string
                int res = (int)cmd.ExecuteScalar();//execute command
                conn.Close();     //close connection
                if (res <= 0) //if result is equal or less than 0
                    return 0; //return 0;
                return 1;//otherwise return 1
            
        }

        protected void CreateTable(int num)
        {
            StringBuilder builder = new StringBuilder();//intialize StringBuilder
            string sql;//string containing sql statement
            if (num == 1)
            {
                //Append strings to form complete sql statement
                builder.Append("Alter Table Customers ADD CCN VARCHAR(50)");
            
               /* builder.Append("CCN VARCHAR(50),");
                builder.Append("IV VARCHAR(50),");
                builder.Append("Key VARCHAR(50))"); */
                sql = builder.ToString();
            }
            else
            {
                //Append strngs to form complete sql statement
                builder.Append("Create Table FlashWordTable (FWord VARCHAR(40) NOT NULL PRIMARY KEY,");
                builder.Append("FMeaning VARCHAR(100),");
                builder.Append("FExample VARCHAR(100))");
                sql = builder.ToString();
            }

            
            cmd = new SqlCommand();//initialize cmd object
            using (conn = new SqlConnection(connstr))//assign conn connection information
            {
                try
                {
                    conn.Open();//open connection
                    cmd.Connection = conn;//assign cmd field conn
                    cmd.CommandText = sql;//assign commandText sql statement
                    cmd.ExecuteNonQuery();//execute statement
                }
                catch (SqlException se)//catch sqlException
                {
                    MessageBox.Show(se.Message);//pop up messagebox displaying exception information
             
                }
            }           


        }

        /* testing purposes */
        protected void DestroyTable(int num)
        {

            StringBuilder builder = new StringBuilder();
            if (num == 1)
                builder.Append("Drop Table Customers");
            else
                builder.Append("Drop Table AllWordTable");
           
            string sql = builder.ToString();

            //cmd = new SqlCommand(sql);
            cmd = new SqlCommand();
            using (conn = new SqlConnection(connstr))
            {
                try
                {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException se)
                {
                    MessageBox.Show(se.Message);

                }
            }    
        } 
 
        /// <summary>
        /// This method takes the information from the database table and assigns them to the datagridview takes a defualt argument of 0. How the information is put onto 
        /// the table depends on whether 0 or 1 is assigned to the variable num.
        /// </summary>
        /// <param name="num"></param>
        public void display_table(int num=0)
        {
            string sqlstr;//string to contain sql statement
            if (num == 0)//if num is 0 select all from allwordtable
                sqlstr = "Select * from AllWordTable";
            else//if num is other than 0 select from allwordtable in ascending order
                sqlstr = "Select * from AllWordTable ORDER BY Word ASC";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlstr, connstr);//initialize adapter object with sql statement and connection information

            DataSet dataset = new DataSet();//intialize dataset object
            adapter.Fill(dataset, "AllWordTable");//fill adapter object with dataset object and table name
            adapter.Dispose();//free memory

            //assign table object rows from AllWordTable
            DataTable table = dataset.Tables["AllWordTable"];

            foreach (DataRow row in table.Rows)//for each statement to get each row
            {
                int n = dataGridView2.Rows.Add();//add row in datagridview and assign the values obtained from the database table
                dataGridView2.Rows[n].Cells[0].Value=row["Word"].ToString();
                dataGridView2.Rows[n].Cells[1].Value=row["Meaning"].ToString();
                dataGridView2.Rows[n].Cells[2].Value=row["Example"].ToString();
                dataGridView2.Rows[n].Cells[3].Value=row["Time"].ToString();
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            title = e.Graphics;//Initilize title object
            Font f = new Font("Arial", 10, FontStyle.Regular);
            SizeF size = title.MeasureString("Vocabulary Builder", f);
            Pen p = new Pen(Color.Black); //pen is needed for drawing line
            int x1,y1,x2,y2;
            // left side border
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = ClientSize.Height;
            for (int i = 0; i < 10; i++){
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
            

            //Draw string with the font type,size,color and location.
            title.DrawString("Vocabulary Builder", new Font("Arial", 25), new SolidBrush(Color.Black), new Point((ClientSize.Width / 2) - 125, ClientSize.Height - (ClientSize.Height-20)));
   
        } 

          

        

        //The method for the Quit Button. Invokes method when clicked
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();//End program
        }

        //Timer click to method to invoke every 1000 milliseconds
        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox2.ReadOnly = true;
            textBox2.Text = DateTime.Now.ToString("T");//Every 1000th millisecond will write the current time into textbox2
        }
       
   
        //search button brings up another form
         private void button6_Click(object sender, EventArgs e)
         {
             new Search().ShowDialog(); //call serach form and show it
           
         }
        //sort by word button clears the database and sorts rows.
         private void button5_Click(object sender, EventArgs e)
         {
             dataGridView2.Rows.Clear();//clear datagridview rows
             display_table(1);//display table in ascending order
           
         }

   
        //delete button
         private void button4_Click(object sender, EventArgs e)
         {
             
             //if statement that brings up message box to confirm choice
              if (MessageBox.Show("Delete highlighted word entry from database?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              {
              int i = dataGridView2.SelectedCells[0].RowIndex;//get index of selected cell
              string sql = "DELETE FROM AllWordTable WHERE Word=" + "'" + dataGridView2.Rows[i].Cells[0].Value.ToString()+"'";//sql statement to delete what was entered in textbox
              cmd.CommandText = sql;//assign commandText sql statement
              try
              {
                  conn.Open();//open connection
                  cmd.Connection = conn;//initialize cmd.Connection with conn
                  cmd.ExecuteScalar();//execute statement
                  dataGridView2.Rows.RemoveAt(i);//remove row from datagridview
              }
              catch(SqlException se)//catch sql exception
              {
                  MessageBox.Show(se.Message);//dislay message box showing details of the exception
              }
        
     
              }
              else//return
                  return; 
         }

        //Method that enables the delete button when a cell is clicked.
         private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
         {
             button4.Enabled = true;//enable delete button
             button7.Enabled = true;
             button11.Enabled = true;
         }
        //Add button to add a flash card word to list.
         private void button7_Click(object sender, EventArgs e)
         {
             new Form2(conn,cmd,dataGridView2).ShowDialog();
         }
        //Remove button to remove a word from the flash cards. Only active when the words are turned off.
         private void button8_Click(object sender, EventArgs e)
         {
             flashObject.Remove_Flash_Word(textBox9.Text);//Get text from text box and remove word from flash cards
             textBox9.Text = string.Empty;//empty out the text box
         }
        //This method writes the words from lstFlashWords object onto the target file
         private void button9_Click(object sender, EventArgs e)
         {
            flashObject.Write_Flash_Words();//Invoke WriteFlashWords
         }
        //This is the event handler for the flashEvent,takes an object and wordEventArgs for arguments
        public void flashEventHandler(object sender,wordEventArgs e)
        {
                dataGridView3.Rows[0].Cells[0].Value = e.Spelling;//insert spelling in cell
                dataGridView3.Rows[0].Cells[1].Value = e.Definition;//insert definition in cell
                dataGridView3.Rows[0].Cells[2].Value = e.Sentence;//insert example in cell         
        }

        //This method controls the button which turns the flashcard words off and on
        private void button10_Click(object sender, EventArgs e)
        {
            if(button10.Text=="Turn Words On")//If words are off
            {
                button10.Text = "Turn Words Off";//Change button label
                flashObject.flashEvent += flashEventHandler;//Add method to event
                button8.Enabled = true;//Disable the remove word button
                button7.Enabled = true;//Disable the add word button
            }
            else
            {
                button10.Text = "Turn Words On";//If words are on change button label
                flashObject.flashEvent -= flashEventHandler;//remove method from event
                button8.Enabled = true;//Enable the remove button
                button7.Enabled=true;//Enable the add button
            }
        }

        
        //Add word to flash card
        private void button1_Click(object sender, EventArgs e)
        {
            int n = dataGridView2.SelectedCells[0].RowIndex;//get selected row index
            //add to the flash word card
            flashObject.Add_Flash_Word(dataGridView2.Rows[n].Cells[0].Value.ToString(), dataGridView2.Rows[n].Cells[1].Value.ToString(), dataGridView2.Rows[n].Cells[2].Value.ToString());
            
            
            
        }
        //sort by time button
        private void button2_Click(object sender, EventArgs e)
        {
            //clear rows
            dataGridView2.Rows.Clear();
            display_table();//display table by time inserted
        }

        /// <summary>
        ///Show information button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            int n = dataGridView2.SelectedCells[0].RowIndex;//get selected row index
            //call word information form and pass in cell details.
            new WordInformation(dataGridView2.Rows[n].Cells[0].Value.ToString(), dataGridView2.Rows[n].Cells[1].Value.ToString(), dataGridView2.Rows[n].Cells[2].Value.ToString()).ShowDialog();
        }

      
     }
    


}


