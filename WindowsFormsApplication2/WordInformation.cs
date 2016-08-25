using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    //class for displaying word information
    public partial class WordInformation : Form
    {
        public WordInformation(string word,string def,string example)//constructor accepting 3 strings
        {
            InitializeComponent();
               //assign string to text boxes
            textBox1.Text = word;
            textBox2.Text = def;
            textBox3.Text = example;
            
        }
    }
}
