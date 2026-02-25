using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace do_an_scada
{
    public partial class frLogin : Form
    {
        string id = "";
        string Pass = "";
        public frLogin()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Show()
        {
            Form1 _Show = new Form1();
            _Show.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            id = Properties.Settings.Default.ID;
            Pass = Properties.Settings.Default.Pass;
            if (id.Equals(txtID.Text) && Pass.Equals(txtPass.Text))
            {
                if (ckRemember.Checked)
                {
                    Properties.Settings.Default.Remember = true;
                }
                else
                {
                    Properties.Settings.Default.Remember = false;
                }
                Properties.Settings.Default.Save();

                Thread _Thread = new Thread(new ThreadStart(Show));
                _Thread.Start();
                this.Close();//dong form login 
            }
            else
            {
                MessageBox.Show("Login Error");
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            id = Properties.Settings.Default.ID;
            Pass = Properties.Settings.Default.Pass;
            if (btnChange.Text == "Change")
            {
                if (id.Equals(txtID.Text) && Pass.Equals(txtPass.Text))
                {
                    MessageBox.Show("Vui lòng nhập ID và Pass mới");
                    lbID.Text = "New ID";
                    lbPass.Text = "New Pass";
                    txtID.Text = "";
                    txtPass.Text = "";
                    btnChange.Text = "Save";
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
            else //save
            {
                Properties.Settings.Default.ID = txtID.Text;
                Properties.Settings.Default.Pass = txtPass.Text;
                Properties.Settings.Default.Save();

                lbID.Text = "ID";
                lbPass.Text = "Pass";
                txtID.Text = "";
                txtPass.Text = "";
                btnChange.Text = "Change";

                MessageBox.Show("Save completed!");
            }
        }

        private void frLogin_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Remember)
            {
                ckRemember.Checked = true;
                txtID.Text = Properties.Settings.Default.ID;
                txtPass.Text = Properties.Settings.Default.Pass;
            }
            else
            {
                ckRemember.Checked = false;
                txtID.Text = "";
                txtPass.Text = "";
            }
        }
    }
}
