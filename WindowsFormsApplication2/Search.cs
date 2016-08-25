using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication2
{
    public partial class Search : Form
    {
        public Search()
        {
         
            InitializeComponent();
            //make text boxes read only
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
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
            for (int i = 0; i < 5; i++)
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
            for (int i = 0; i < 5; i++)
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
            for (int i = 0; i < 5; i++)
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
            for (int i = 0; i < 5; i++)
            {
                title.DrawLine(p, x1, y1, x2, y2);
                x1 += 1;
                y1 -= 1;
                x2 -= 1;
                y2 -= 1;
            }




        }
        //The search button that when clicked opens a connection to the database and performs a query based on user input.
        private void button1_Click(object sender, EventArgs e)
        {
            bool found = false;//bool variable to indicate a successful search
            //string containing sql statement.
            string sql = "SELECT Word,Meaning,Example FROM AllWordTable WHERE Word=" + "'" + textBox1.Text.ToString() + "'";
            //initialize SQLDataAdapter object with sql statement and connection information
            SqlDataAdapter adapter = new SqlDataAdapter(sql, "server=10.158.56.48;uid=net11;pwd=netword11;database=notebase11;");
            //initialize Dataset object
            DataSet dataset = new DataSet();
            //fill object with data set object and name of table
            adapter.Fill(dataset, "AllWordTable");
            adapter.Dispose();

            //DataTable tbl = dset.Tables[0];      // either indexer is ok
            DataTable table = dataset.Tables["AllWordTable"];

            foreach (DataRow row in table.Rows)
            {
                found = true;//mark true if found
                textBox2.Text = row["Meaning"].ToString();//get information from row
                textBox3.Text = row["Example"].ToString();//get information from row
            }
            if(!found)//if false
            {
                //display message box
                MessageBox.Show("Word not found.","No Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
    }
}
