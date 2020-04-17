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

namespace Emissions
{
    public partial class Form1 : Form
    {
        string strCon = "Data Source=.;Initial Catalog=Emissions;Integrated Security=True";
        DataTable dt = new DataTable();
        string Duree = "", DateE  = "" ;
        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            DateE = dateTimePicker1.Value.Year.ToString();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Duree = "1";
          
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Duree = "2";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Duree = "3";
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" && !radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked && !radioButton4.Checked && !radioButton5.Checked && comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Aucun filtre selectione !!");
                return;
            }
            comboBox1.SelectedIndex = 0;
            using(SqlConnection cnx = new SqlConnection(strCon))
            {
                cnx.Open();
                using(SqlCommand cmd = new SqlCommand("Recherche", cnx))
                {
                    if (dt.Rows.Count != 0) dt.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Nom", textBox1.Text));
                    cmd.Parameters.Add(new SqlParameter("@DateE", DateE));
                    cmd.Parameters.Add(new SqlParameter("@Duree", Duree));
                    cmd.Parameters.Add(new SqlParameter("@TypeE", comboBox1.SelectedValue.ToString()));
                    dt.Load(cmd.ExecuteReader());
                    dataGridView1.DataSource = dt;
                    cmd.Parameters.Clear();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vider(this);
            Duree = ""; DateE = "";
        }

        

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked) DateE = dateTimePicker1.Value.Year.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (SqlConnection cnx = new SqlConnection(strCon))
            {
                SqlDataAdapter da = new SqlDataAdapter("Type_list", cnx);
                DataSet ds = new DataSet();
                da.Fill(ds, "Type_List");
                comboBox1.DataSource = ds.Tables["Type_List"];
                comboBox1.DisplayMember = "Value";
                comboBox1.ValueMember = "Key";
            }

            Vider(this);

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DateE = "" ;
        }
        public void Vider(Control c)
        {
            foreach (Control ctrl in c.Controls)
            {
                if (ctrl is TextBox) ((TextBox)ctrl).Text = String.Empty;
                if (ctrl is RadioButton) ((RadioButton)ctrl).Checked = false;
                if (ctrl is DateTimePicker) ((DateTimePicker)ctrl).Value = DateTime.Now;
                if (ctrl is DataGridView) ((DataGridView)ctrl).DataSource = null;
                if (ctrl is ComboBox) ((ComboBox)ctrl).SelectedIndex = -1;

                if (ctrl.Controls.Count != 0) Vider(ctrl);
            }
        }

    }

}
