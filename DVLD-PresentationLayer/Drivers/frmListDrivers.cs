using DVLD.Common_Classes;
using DVLD.Licenses;
using DVLD.People;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Drivers
{
    public partial class frmListDrivers : Form
    {
        DataTable _dtDrivers;
        public frmListDrivers()
        {
            InitializeComponent();
        }

        enum enFilterBy
        {
            None = 0, DriverID, PersonID, NationalNo, FullName
        }

       
        private void frmManageDrivers_Load(object sender, EventArgs e)
        {
            _dtDrivers = clsDriver.ListAllDrivers();
            dgvDrivers.Columns.Clear();
            dgvDrivers.DataSource = _dtDrivers;

            dgvDrivers.Columns[0].HeaderText = "Driver ID";
            dgvDrivers.Columns[0].Width = 120;

            dgvDrivers.Columns[1].HeaderText = "Person ID";
            dgvDrivers.Columns[1].Width = 120;

            dgvDrivers.Columns[2].HeaderText = "National No";
            dgvDrivers.Columns[2].Width = 150;

            dgvDrivers.Columns[3].HeaderText = "Full Name";
            dgvDrivers.Columns[3].Width = 340;

            dgvDrivers.Columns[4].HeaderText = "Date";
            dgvDrivers.Columns[4].Width = 180;

            dgvDrivers.Columns[5].HeaderText = "Active Licenses";
            dgvDrivers.Columns[5].Width = 150;

            
            cbFilter.SelectedIndex = (int)enFilterBy.None;
            txtFilterValue.Visible = false;
            lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilter.SelectedIndex != (int) enFilterBy.NationalNo && cbFilter.SelectedIndex != (int)enFilterBy.FullName)
            {
                e.Handled = !clsValidation.IsValidInteger(sender, e);
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbFilter.SelectedIndex == (int)enFilterBy.None)
            {
                txtFilterValue.Visible = false;
            }
            else
                txtFilterValue.Visible=true;

            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch ((enFilterBy)cbFilter.SelectedIndex) {
                case enFilterBy.DriverID:
                    FilterColumn = enFilterBy.DriverID.ToString();
                    break;
                case enFilterBy.PersonID:
                    FilterColumn = enFilterBy.PersonID.ToString();
                    break;
                case enFilterBy.NationalNo:
                    FilterColumn = enFilterBy.NationalNo.ToString();
                    break;
                case enFilterBy.FullName:   
                    FilterColumn = enFilterBy.FullName.ToString();
                    break;
                case enFilterBy.None:
                    FilterColumn = enFilterBy.None.ToString();
                    break;
            }



            if(txtFilterValue.Text.Trim() == "" ||  cbFilter.SelectedIndex == (int)enFilterBy.None)
            {
                _dtDrivers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvDrivers.RowCount.ToString();
                return;
            }

            if(cbFilter.SelectedIndex == (int)enFilterBy.NationalNo || cbFilter.SelectedIndex == (int)enFilterBy.FullName)
            {
                _dtDrivers.DefaultView.RowFilter = string.Format("[{0}] like '%{1}%' ", FilterColumn, txtFilterValue.Text.Trim());

            }
            else
            {
                _dtDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1} ", FilterColumn, txtFilterValue.Text.Trim());

            }


            lblRecordsCount.Text = dgvDrivers.RowCount.ToString();

        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((int)dgvDrivers.SelectedCells[1].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDriverLicenseHistory frm = new frmDriverLicenseHistory((int)dgvDrivers.SelectedCells[1].Value);
            frm.ShowDialog();
        }
    }
}
