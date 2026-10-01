using DVLD.Common_Classes;
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

namespace DVLD.Licenses.Renew_Local_Driving_License
{
    public partial class frmRenewLocalDrivingLicense : Form
    {
        public frmRenewLocalDrivingLicense()
        {
            InitializeComponent();
        }

        clsLicense _OldLocalLicense;
        clsLicense _NewLocalLicense;
        private void frmRenewLocalDrivingLicense_Load(object sender, EventArgs e)
        {
            lblAppDate.Text = clsUtil.CustomShortDate(DateTime.Now);
            lblIssueDate.Text = clsUtil.CustomShortDate(DateTime.Now);
            lblApplicationFees.Text = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.RenewLicense).ToString();
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.UserName;
            btnRenew.Enabled = false;
            lnkShowLicenseInfo.Enabled = false;
            lnkShowLicensesHistory.Enabled = false;

            this.AcceptButton = ctrlFindLicenseWithFilter1.btnFindLicense;
            
        }

        private void ctrlFindLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            int LicenseID = obj;
            btnRenew.Enabled = false;
            lnkShowLicenseInfo.Enabled = false;
            lnkShowLicensesHistory.Enabled= true;

            if(LicenseID == -1)
            {
                lnkShowLicensesHistory.Enabled = false;
                lblOldLicenseID.Text = "??";
                lblLicenseFees.Text = "??";
                lblTotalFees.Text = "??";
                lblExpirationDate.Text = "??";

                return;
            }

            _OldLocalLicense = clsLicense.Find(LicenseID);

            lblOldLicenseID.Text = _OldLocalLicense.LicenseID.ToString();
            lblLicenseFees.Text = _OldLocalLicense.LicenseClassInfo.ClassFees.ToString();
            lblTotalFees.Text = (Convert.ToDouble(lblApplicationFees.Text) + Convert.ToDouble(lblLicenseFees.Text)).ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(_OldLocalLicense.LicenseClassInfo.DefaultValidityLength).ToString();


            //check if license expired
            int IsExpired = DateTime.Compare(_OldLocalLicense.ExpirationDate, DateTime.Now);

            if(IsExpired > 0)
            {
                //the license is expired.
                MessageBox.Show($"This license is not expired yet, expiration date is '{_OldLocalLicense.ExpirationDate}'.",
                    "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (!_OldLocalLicense.IsActive)
            {
                MessageBox.Show($"This license is not active, please choose another license.",
                   "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            btnRenew.Enabled = true;

        }

        private void lnkShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseHistory frm = new frmDriverLicenseHistory(_OldLocalLicense.PersonID);
            frm.ShowDialog();
        }

        private void lnkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseInfo frm = new frmDriverLicenseInfo(_NewLocalLicense.LicenseID);
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            DialogResult msgResult =  MessageBox.Show($"Are you sure you want to renew this license [{_OldLocalLicense.LicenseID}] ?",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msgResult == DialogResult.No)
                return;


            //Deactivate old License.
            _OldLocalLicense.Deactivate();

            //create new application with status completed
            clsApplication application = new clsApplication();
            application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            application.ApplicantPersonID = _OldLocalLicense.PersonID;
            application.ApplicationTypeID = clsApplication.enApplicationType.RenewLicense;
            application.PaidFees = Convert.ToDouble(lblApplicationFees.Text);
            application.ApplicationDate = DateTime.Now;
            application.LastStatusDate = DateTime.Now;
            application.ApplicantPersonID = _OldLocalLicense.PersonID;
            application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            application.Save();


            //create ne license
            _NewLocalLicense = new clsLicense();

            _NewLocalLicense.ApplicationID = application.ApplicationID;
            _NewLocalLicense.IssueDate = DateTime.Now;
            _NewLocalLicense.ExpirationDate = DateTime.Now.AddYears(_OldLocalLicense.LicenseClassInfo.DefaultValidityLength);
            _NewLocalLicense.IsActive = true;
            _NewLocalLicense.DriverID = _OldLocalLicense.DriverID;
            _NewLocalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _NewLocalLicense.IssueReason = clsLicense.enIssueReason.Renew;
            _NewLocalLicense.PaidFees = Convert.ToDouble(lblLicenseFees.Text);
            _NewLocalLicense.Notes = txtNotes.Text;
            _NewLocalLicense.LicenseClassID = _OldLocalLicense.LicenseClassID;

            if(_NewLocalLicense.Save())
            {
                MessageBox.Show($"License Renewed successfully with id [{_NewLocalLicense.LicenseID}] ?",
                    "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ctrlFindLicenseWithFilter1.EnableFilter = false;
                txtNotes.Enabled = false;

                btnRenew.Enabled = false;
                lnkShowLicenseInfo.Enabled = true;

                lblRenewLicenseAppID.Text = application.ApplicationID.ToString();
                lblRenewedLicenseID.Text = _NewLocalLicense.LicenseID.ToString();
            }
            else
            {
                MessageBox.Show($"Failed",
                    "Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }

        }
    }
}
