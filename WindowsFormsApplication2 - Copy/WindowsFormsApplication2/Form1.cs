using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary1;
/*
 * Cody Hammond & Jason Palokoff
 * ASSIGNMMENT 1
 * DUE: 1/29/15 11:59PM
 * Constributions:
 * Cody wrote the class library,textbox,and button actions,input and output.
 * Jason documented the code, formated the Form and inserted the timer,and made listbox functionality.
 */
namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        //private members of the form class that will be used
        //StreamWriter and StreamReader will be used to write and read a file
        private StreamWriter writer;
        private StreamReader reader;
        //StreamBuilder is for getting the path of the file to be used
        private StringBuilder path;
        //Graphics is for drawing the name of the software
        private Graphics title;
        bool[] clicked = new bool[3] {false,false,false};//Array for check clicked input text boxes
        public Form1()//Form constuctor intitiallizes the form1 object
        {
            InitializeComponent();//Initializes varias components within the form like buttons, and textboxes
            initializeTextBox();//Initializes the text boxs with instructions
           path=new StringBuilder();//initialize the string builder object
            string file="\\dictionary.txt";//assigns file name to string object
            path.Append(Directory.GetCurrentDirectory()).Append(file);//Append file name to end of current directory path
            if(File.Exists(path.ToString()))        
                reader = File.OpenText(path.ToString());//Open file for reading
            else//if file does not exist create one and open it for reading.
            {
                writer=File.CreateText(path.ToString());
                writer.Close();
                reader = File.OpenText(path.ToString());
            }

            while (!reader.EndOfStream)//while not at the end of the file
                listBox1.Items.Add(reader.ReadLine());//add the content to the list box

            reader.Close();//close file
        }

     //Method for the button "Save". Method is invoked when clicked
        private void button1_Click(object sender, EventArgs e)
        {
         
            //To make sure that the instructions don't get inserted into dictionary
            if (!clicked[0] || !clicked[1] || !clicked[2])
            {
                MessageBox.Show("You must have a complete entry to insert into the dictionary!");
                return;
            }

            string check=textBox1.Text;//Assign to text to a string object
            if(check==string.Empty)
            {
                MessageBox.Show("Empty Field!");
                return;
            }  
            else
            {             
            Regex expr = new Regex("[a-zA-Z]");//use regular expressions to check for input validity
            if(!expr.IsMatch(check) || !expr.IsMatch(textBox3.Text)|| !expr.IsMatch(textBox4.Text))
            {
                MessageBox.Show("Invalid character(s) in one or more fields. Please use alphabetic characters only and leave no field empty.");
                return;
            }
           
            }

            if (WordSearch.search(check.ToLower()))//Use method in the class library to look up word in the file.
            {
                MessageBox.Show("That word is already in the list!");//If word exists display message box saying so
            }
            else//If the word was not found then
            {
                //Clear all the items in the list box
                //And prepare to update the list box
                StringBuilder entry = new StringBuilder();
                listBox1.Items.Clear();
                listBox1.BeginUpdate();
                //Open file
                writer = File.AppendText(path.ToString());
                //Take content into buffer
                entry.Append(check.ToLower()).Append('*').Append(textBox3.Text).Append('*').Append(textBox4.Text);
                writer.WriteLine(entry.ToString());
                //Then write to file and close the file
                writer.Flush();
                
                writer.Close();
                
                //Open the file for reading
                reader = File.OpenText(path.ToString());
                //Read till end of file and add to list
                while (!reader.EndOfStream)
                    listBox1.Items.Add(reader.ReadLine());
                //End list box update close file and clear text box
                listBox1.EndUpdate();
                reader.Close();
                
            }
            //clear all three textboxes
            textBox1.Clear();
            textBox4.Clear();
            textBox3.Clear();
        }

        //The method for the Quit Button. Invokes method when clicked
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();//End program
        }

        //The method for the Cancel button. Invokes method when clicked
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();//clear textbox
            textBox3.Clear();
            textBox4.Clear();
        }

        //Timer click to method to invoke every 1000 milliseconds
        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox2.Text = DateTime.Now.ToString("T");//Every 1000th millisecond will write the current time into textbox2
        }
        
    //The Form1_Paint_1 method drews the title of the software
        private void Form1_Paint_1(object sender, PaintEventArgs e)
        {   
            title = e.Graphics;//Initilize title object
            Font f = new Font("Arial", 10, FontStyle.Regular);
            SizeF size = title.MeasureString("Vocabulary Builder", f);
            
            //Draw string with the font type,size,color and location.
            title.DrawString("Vocabulary Builder", new Font("Arial", 20), new SolidBrush(Color.Black), new Point((ClientSize.Width/2)-120, ClientSize.Height-440));
        }

    

        //Method that inserts text into the corresponding text boxes.
        private void initializeTextBox()
         {
             textBox1.Text = "Enter Word";
             textBox3.Text = "Enter Definition";
             textBox4.Text = "Enter Sentence";
         }

        /*
        The following methods get rid of the instructions in the 3 textboxes when the they are clicked.
         */

        //When the texbox is active on the form this method is invoked
         private void textBox1_Focus(object sender, EventArgs e)
         {
             
             if(!clicked[0])//if false
             {
                 textBox1.Text = String.Empty;//set the texbox to an empty string
                 clicked[0] = true;//set clicked to true
             }
         }
        //When this textbox is active on the form this method is invoked
         private void textBox3_Focus(object sender, EventArgs e)
         {
             
             if (!clicked[1])//if false
             {
                 textBox3.Text = String.Empty;//set textbox to an empty string
                 clicked[1] = true;//set clicked to true
             }
         }

        //When this textbox is active on the form this method is invoked
         private void textBox4_Focus(object sender, EventArgs e)
         {
            
             if (!clicked[2])//if false
             {
                 textBox4.Text = String.Empty;//set textbox to empty
                 clicked[2] = true;//set clicked to true
             }
         }

      
       
    }
}


