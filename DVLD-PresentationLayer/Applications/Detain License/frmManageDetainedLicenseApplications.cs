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

namespace DVLD.Applications.Detain_License
{
    public partial class frmManageDetainedLicenseApplications : Form
    {
        DataTable _dtDetainedLicenses = new DataTable();
        public frmManageDetainedLicenseApplications()
        {
            InitializeComponent();
        }

        enum enFindBy
        {
            None = 0, LicenseID, DetainID, ReleaseAppID, NationalNo, IsReleased
        }
        enum enIsReleased
        {
            All = 0, Yes, No
        }

        //License ID, Detain ID, National No, Is Released
        private void frmManageDetainedLicenseApplications_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 0;
            cbIsReleased.SelectedIndex = 0;
            txtFilterValue.Visible = false;
            cbIsReleased.Visible = false;
            lblNoApp.Visible = false;

            _dtDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();
            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;

            if (_dtDetainedLicenses.Rows.Count == 0)
            {
                lblNoApp.Visible = true;
                cbFilter.Enabled = false;
                lblRecordsCount.Text = "0";
                return;
            }

            


            dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
            dgvDetainedLicenses.Columns[0].Width = 80;

            dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
            dgvDetainedLicenses.Columns[1].Width = 80;

            dgvDetainedLicenses.Columns[2].HeaderText = "D.Date";
            dgvDetainedLicenses.Columns[2].Width = 170;

            dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
            dgvDetainedLicenses.Columns[3].Width = 95;

            dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
            dgvDetainedLicenses.Columns[4].Width = 110;

            dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
            dgvDetainedLicenses.Columns[5].Width = 170;

            dgvDetainedLicenses.Columns[6].HeaderText = "N.No.";
            dgvDetainedLicenses.Columns[6].Width = 80;

            dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
            dgvDetainedLicenses.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvDetainedLicenses.Columns[8].HeaderText = "Release App.ID";
            dgvDetainedLicenses.Columns[8].Width = 120;

            lblRecordsCount.Text = dgvDetainedLicenses.RowCount.ToString();

            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            frmDetainLicense frmDetainLicense = new frmDetainLicense();
            frmDetainLicense.ShowDialog();
            _RefreshDetainedLicenses();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
           frmReleaseDetainedLicense frmReleaseLicense = new frmReleaseDetainedLicense();
            frmReleaseLicense.ShowDialog();
            _RefreshDetainedLicenses();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";

            if (cbFilter.SelectedIndex == (int)enFindBy.None) { 
                txtFilterValue.Visible=false;
                cbIsReleased.Visible=false;
                return;
            }

            if (cbFilter.SelectedIndex == (int)enFindBy.IsReleased) {
                txtFilterValue.Visible = false;
                cbIsReleased.Visible = true;
                return;
            }

            txtFilterValue.Visible = true;
            cbIsReleased.Visible = false;
            cbIsReleased.SelectedIndex = 0;


        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.SelectedIndex != (int)enFindBy.NationalNo)
            {
                e.Handled = !clsValidation.IsValidInteger(sender, e);
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            

            if (string.IsNullOrEmpty(txtFilterValue.Text))
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                return;
            }

            string FilterColumn = "";


            switch ((enFindBy)cbFilter.SelectedIndex)
            {
                case enFindBy.LicenseID:
                    FilterColumn = "LicenseID";
                    break;
                case enFindBy.DetainID:
                    FilterColumn = "DetainID";
                    break;
                case enFindBy.ReleaseAppID:
                    FilterColumn = "ReleaseApplicationID";
                    break;
                case enFindBy.NationalNo:
                    FilterColumn = "NationalNo";
                    break;
                case enFindBy.None:
                    FilterColumn = "None";
                    break;
            }

            if(cbFilter.SelectedIndex != (int)enFindBy.NationalNo)
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format(" [{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            }
            else
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format(" [{0}] LIKE '%{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            }



        }

        private DataTable Get_dtDetainedLicenses()
        {
            return _dtDetainedLicenses;
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {

            string FilterColumn = "IsReleased";

            if (cbIsReleased.SelectedIndex == (int)enIsReleased.Yes)
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, 1);
                return;
            }

            if (cbIsReleased.SelectedIndex == (int)enIsReleased.No)
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, 0);
                return;
            }

            _dtDetainedLicenses.DefaultView.RowFilter = "";


        }

        private void _RefreshDetainedLicenses()
        {
            _dtDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();
            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frmPersonDetails   = new frmShowPersonInfo(clsLicense.Find((int)dgvDetainedLicenses.SelectedCells[1].Value).PersonID);
            frmPersonDetails.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDriverLicenseHistory frm = new frmDriverLicenseHistory(clsLicense.Find((int)dgvDetainedLicenses.SelectedCells[1].Value).PersonID);
            frm.ShowDialog();

        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDriverLicenseInfo frm = new frmDriverLicenseInfo(clsLicense.Find((int)dgvDetainedLicenses.SelectedCells[1].Value).LicenseID);
            frm.ShowDialog();
        }

        private void releaseDetinedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(clsLicense.Find((int)dgvDetainedLicenses.SelectedCells[1].Value).LicenseID);
            frm.ShowDialog();
            _RefreshDetainedLicenses();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int LicenseID = clsLicense.Find((int)dgvDetainedLicenses.SelectedCells[1].Value).LicenseID;

            releaseDetinedLicenseToolStripMenuItem.Enabled = clsDetainedLicense.IsLicenseDetained(LicenseID);
        }
    }
}
