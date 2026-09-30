using DVLD.Licenses.International_Driving_License;
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

namespace DVLD.Licenses.Controls
{
    public partial class ctrListDriverLicenses : UserControl
    {
        public ctrListDriverLicenses()
        {
            InitializeComponent();
        }

        public void _LoadDriverLicenses(int DriverID)
        {
            _LoadLocalLicenses(DriverID);

            _LoadInternationalLicenses(DriverID);


        }
        private void _LoadLocalLicenses(int DriverID)
        {

            //dgvLocalLicensesHistory.Columns.Clear();
            DataTable dtLocalLicenses = clsDriver.ListAllLocalLicenses(DriverID);
            dgvLocalLicensesHistory.DataSource = dtLocalLicenses;     
            
            lblLocalLicensesCount.Text = dgvLocalLicensesHistory.RowCount.ToString();

            if (dgvInternationalLicensesHistory.Columns.Count == 0)
                return;


            dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic. ID";
            dgvLocalLicensesHistory.Columns[0].Width = 180;

            dgvLocalLicensesHistory.Columns[1].HeaderText = "Application ID";
            dgvLocalLicensesHistory.Columns[1].Width = 150;

            dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
            dgvLocalLicensesHistory.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgvLocalLicensesHistory.Columns[2].Width = 150;

            dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
            dgvLocalLicensesHistory.Columns[3].Width = 180;

            dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
            dgvLocalLicensesHistory.Columns[4].Width = 180;

            dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
            dgvLocalLicensesHistory.Columns[5].Width = 150;


        }
        private void _LoadInternationalLicenses(int DriverID)
        {
            DataTable dtInternationalLicenses = clsDriver.ListAllInternationalLicenses(DriverID);
            dgvInternationalLicensesHistory.DataSource = dtInternationalLicenses;
            lblInternationalLicensesCount.Text = dgvInternationalLicensesHistory.RowCount.ToString();

            if(dgvInternationalLicensesHistory.Columns.Count == 0)
                return;

            dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
            //dgvInternationalLicensesHistory.Columns[0].Width = 180;
            dgvInternationalLicensesHistory.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
            dgvInternationalLicensesHistory.Columns[1].Width = 150;

            dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
            dgvInternationalLicensesHistory.Columns[2].Width = 150;

            dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
            dgvInternationalLicensesHistory.Columns[3].Width = 190;

            dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
            dgvInternationalLicensesHistory.Columns[4].Width = 190;

            dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
            dgvInternationalLicensesHistory.Columns[5].Width = 150;



        }
        private void ctrlDriverLicenses_Load(object sender, EventArgs e)
        {

        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDriverLicenseInfo frm = new frmDriverLicenseInfo((int)dgvLocalLicensesHistory.SelectedCells[0].Value);
            frm.ShowDialog();
        }

        private void showLicenseInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmInternationalLicenseInfo frm = new frmInternationalLicenseInfo((int)dgvInternationalLicensesHistory.SelectedCells[0].Value);
            frm.ShowDialog();
        }
    }
}
