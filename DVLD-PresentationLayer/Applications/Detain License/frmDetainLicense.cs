using DVLD.Common_Classes;
using DVLD.Licenses;
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
    public partial class frmDetainLicense : Form
    {
        public frmDetainLicense()
        {
            InitializeComponent();
        }

        clsLicense _License;
       

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = clsUtil.CustomShortDate(DateTime.Now);
            
            lblCreatedByUserID.Text = clsGlobalSettings.LoggedInUserName;
            btnDetain.Enabled = false;
            lnkShowLicenseInfo.Enabled = false;
            lnkShowLicensesHistory.Enabled = false;

            this.AcceptButton = ctrlFindLicenseWithFilter1.btnFindLicense;
        }

        private void ctrlFindLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            int LicenseID = obj;
            btnDetain.Enabled = false;
            lnkShowLicenseInfo.Enabled = false;
            lnkShowLicensesHistory.Enabled = true;

            if (LicenseID == -1)
            {
                lnkShowLicensesHistory.Enabled = false;
                lblLicenseID.Text = "[???]";

                return;
            }

            _License = clsLicense.Find(LicenseID);

            lblLicenseID.Text = _License.LicenseID.ToString();

            //check if license expired
            //int IsExpired = DateTime.Compare(_License.ExpirationDate, DateTime.Now);

            //if (IsExpired <= 0)
            //{
            //    the license is expired.
            //    MessageBox.Show($"This license is Expired, Please choose another license", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //    return;
            //}

            if (!_License.IsActive)
            {
                MessageBox.Show($"This license is not active, please choose another license.",
                   "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (clsDetainedLicense.IsLicenseDetained(LicenseID))
            {
                MessageBox.Show($"This license is already detained, please choose another license.",
                   "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            btnDetain.Enabled = true;
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !clsValidation.IsValidInteger(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Fine Fees should not be empty!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFineFees, null);
            }
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
           

            DialogResult msgResult = MessageBox.Show($"Are you sure you want to Detain this license [{_License.LicenseID}] ?",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msgResult == DialogResult.No)
                return;


            clsDetainedLicense DetainLicense = new clsDetainedLicense();
            DetainLicense.FineFees = Convert.ToDouble( txtFineFees.Text);
            DetainLicense.LicenseID = _License.LicenseID;
            DetainLicense.DetainDate = DateTime.Now;
            DetainLicense.CreatedByUserID = clsGlobalSettings.LoggedInUserID;

            if (DetainLicense.Save())
            {
                lblDetainLicenseID.Text = DetainLicense.DetainID.ToString();
                MessageBox.Show($"License Detained successfully with id [{DetainLicense.DetainID}] ?",
                    "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ctrlFindLicenseWithFilter1.EnableFilter = false;
                txtFineFees.Enabled = false;

                btnDetain.Enabled = false;
                lnkShowLicenseInfo.Enabled = true;

            }
            else
            {
                MessageBox.Show($"Failed",
                    "Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void lnkShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseHistory frm = new frmDriverLicenseHistory(_License.PersonID);
            frm.ShowDialog();
        }

        private void lnkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseInfo frm = new frmDriverLicenseInfo(_License.LicenseID);
            frm.ShowDialog();
        }
    }
}
