using DVLD.Applications.International_Driving_License_Applications;
using DVLD.Common_Classes;
using DVLD.Licenses;
using DVLD.Licenses.International_Driving_License;
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

namespace DVLD.Applications.International_License_Applications
{
    public partial class frmManageInternationalLicenseApplications : Form
    {
        public frmManageInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        DataTable _dtIntLicenseApplications;
        enum enFilterBy
        {
            None, IntLicenseID, DriverID, LocalLicenseID, NationalNo, IsActive
        }
        private void frmManageInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            dgvIntLicenseApplications.Columns.Clear();

            _dtIntLicenseApplications = clsInternationalLicense.ListAllInternationalLicenses();

            dgvIntLicenseApplications.DataSource = _dtIntLicenseApplications;

            cbFilter.SelectedIndex = 0;
            cbIsActive.Visible = false;
            txtFilterValue.Visible = false;


        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.SelectedIndex != (int)enFilterBy.None &&
                cbFilter.SelectedIndex != (int)enFilterBy.IsActive &&
                cbFilter.SelectedIndex != (int)enFilterBy.NationalNo)
            {
                e.Handled = !clsValidation.IsValidInteger(sender, e);
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch ((enFilterBy)cbFilter.SelectedIndex)
            {
                case enFilterBy.IntLicenseID:
                    FilterColumn = "InternationalLicenseID";
                    break;
                case enFilterBy.DriverID:
                    FilterColumn = "DriverID";
                    break;
                case enFilterBy.NationalNo:
                    FilterColumn = "NationalNo";
                    break;
                case enFilterBy.LocalLicenseID:
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;
                case enFilterBy.IsActive:
                    FilterColumn = "IsActive";
                    break;
                case enFilterBy.None:
                    FilterColumn = "None";
                    break;
            }

            if(string.IsNullOrEmpty(txtFilterValue.Text))
            {
                _dtIntLicenseApplications.DefaultView.RowFilter = "";
                return;
            }


            if(FilterColumn == "NationalNo")
            {
                _dtIntLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] like '%{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            }
            else
            {
                _dtIntLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());

            }

            lblRecordsCount.Text = dgvIntLicenseApplications.RowCount.ToString();



        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            cbIsActive.SelectedItem = "All";

            if (cbFilter.SelectedIndex == (int)enFilterBy.None)
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = false;
                return;

            }

            if (cbFilter.SelectedIndex == (int)enFilterBy.IsActive)
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                return;
            }

            txtFilterValue.Visible = true;
            cbIsActive.Visible = false;
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIsActive.SelectedItem.ToString() == "Yes")
            {
                _dtIntLicenseApplications.DefaultView.RowFilter = "IsActive = 1";
                lblRecordsCount.Text = dgvIntLicenseApplications.RowCount.ToString();
                return;
            }
            if(cbIsActive.SelectedItem.ToString() == "No")
            {
                _dtIntLicenseApplications.DefaultView.RowFilter = "IsActive = 0";
                lblRecordsCount.Text = dgvIntLicenseApplications.RowCount.ToString();
                return;
            }

            _dtIntLicenseApplications.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvIntLicenseApplications.RowCount.ToString();
            return ;
        }

        private void _RefreshDataGridView()
        {
            _dtIntLicenseApplications = clsInternationalLicense.ListAllInternationalLicenses();
            dgvIntLicenseApplications.DataSource = _dtIntLicenseApplications;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmIssueInternationalLicense frm = new frmIssueInternationalLicense();
            frm.ShowDialog();
            _RefreshDataGridView();

        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(clsDriver.FindByDriverID((int)dgvIntLicenseApplications.SelectedCells[1].Value).PersonID);
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInternationalLicenseInfo frm = new frmInternationalLicenseInfo((int)dgvIntLicenseApplications.SelectedCells[0].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDriverLicenseHistory frm = new frmDriverLicenseHistory(clsDriver.FindByDriverID((int)dgvIntLicenseApplications.SelectedCells[1].Value).PersonID);
            frm.ShowDialog();
        }
    }
}
