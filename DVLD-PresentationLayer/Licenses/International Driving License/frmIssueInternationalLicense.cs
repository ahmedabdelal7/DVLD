using DVLD.Common_Classes;
using DVLD.Licenses;
using DVLD.Licenses.International_Driving_License;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.International_Driving_License_Applications
{
    public partial class frmIssueInternationalLicense : Form
    {
        public frmIssueInternationalLicense()
        {
            InitializeComponent();
        }

        clsInternationalLicense _ActiveIntLicense;
        clsLicense _LocalLicense;
        private void ctrlFindLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            int LocalLicenseID = obj;
            //MessageBox.Show($"License ID = {obj}");

            btnIssue.Enabled = false;
            lnkShowLicenseInfo.Enabled = false;
            lnkShowLicensesHistory.Enabled = true;

            if (LocalLicenseID == -1) {
                lblLocalLicenseID.Text = "";
                lnkShowLicensesHistory.Enabled = false;
                return;
            }

            _LocalLicense = clsLicense.Find(LocalLicenseID);
            lblLocalLicenseID.Text = LocalLicenseID.ToString();

            _ActiveIntLicense =
                clsInternationalLicense.GetActiveInternationalLicenseByDriverID(_LocalLicense.DriverID);

            if (_ActiveIntLicense != null) {
                MessageBox.Show($"This person has already active international license with id = {_ActiveIntLicense.InternationalLicenseID.ToString()}," +
                    $" choose another license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_LocalLicense.IsActive)
            {
                MessageBox.Show($"This License is not active, choose another license!",
                    "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                

                return;
            }
            if (clsDetainedLicense.IsLicenseDetained(LocalLicenseID)) {
                MessageBox.Show($"This License is Detained, please pay fine fees first to be able to issue this license.",
                    "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            btnIssue.Enabled = true;
            //lnkShowLicenseInfo.Enabled = false;
            lblLocalLicenseID.Text = LocalLicenseID.ToString();

        }

        private void frmIssueInternationalLicense_Load(object sender, EventArgs e)
        {
            btnIssue.Enabled = false;
            lnkShowLicenseInfo.Enabled = false;
            lnkShowLicensesHistory.Enabled = false;
            
            lblAppDate.Text = clsUtil.CustomShortDate(DateTime.Now);
            lblIssueDate.Text = clsUtil.CustomShortDate(DateTime.Now);
            lblExpirationDate.Text = clsUtil.CustomShortDate(DateTime.Now.AddYears(1));
            lblCreatedByUserUD.Text = clsGlobalSettings.LoggedInUserName;
            lblFees.Text = clsApplicationType.GetApplicationFees(6).ToString();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            DialogResult msgRes =  MessageBox.Show("Are you sure you want to issue the license?","Confirm",MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (msgRes != DialogResult.Yes)
                return;


            _ActiveIntLicense = new clsInternationalLicense();

            _ActiveIntLicense.ApplicantPersonID = _LocalLicense.PersonID;
            _ActiveIntLicense.ApplicationDate = DateTime.Now;
            _ActiveIntLicense.ApplicationTypeID = clsApplication.enApplicationType.NewInternationalLicense;
            _ActiveIntLicense.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _ActiveIntLicense.LastStatusDate= DateTime.Now;
            _ActiveIntLicense.PaidFees = clsApplicationType.GetApplicationFees((int)_ActiveIntLicense.ApplicationTypeID);
            _ActiveIntLicense.CreatedByUserID = clsGlobalSettings.LoggedInUserID;
            _ActiveIntLicense.IssuedUsingLocalLicenseID = _LocalLicense.LicenseID;
            _ActiveIntLicense.IssueDate = DateTime.Now;
            _ActiveIntLicense.ExpirationDate = _ActiveIntLicense.IssueDate.AddYears(1);
            _ActiveIntLicense.CreatedByUserID = clsGlobalSettings.LoggedInUserID;
            _ActiveIntLicense.IsActive = true;
            _ActiveIntLicense.DriverID = _LocalLicense.DriverID;



            if(_ActiveIntLicense.Save())
            {
                MessageBox.Show($"License Issued successfully with ID = {_ActiveIntLicense.InternationalLicenseID} .",
                    "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnIssue.Enabled = false;
                lnkShowLicenseInfo.Enabled = true;
                
                ctrlFindLicenseWithFilter1.EnableFilter = false;

                lblIntLicenseAppID.Text = _ActiveIntLicense.ApplicationID.ToString();
                lblIntLicenseID.Text = _ActiveIntLicense.InternationalLicenseID.ToString();

            }


        }

        private void lnkShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseHistory frm = new frmDriverLicenseHistory(_LocalLicense.PersonID);
            frm.ShowDialog();
        }

        private void lnkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInternationalLicenseInfo frm = new frmInternationalLicenseInfo(_ActiveIntLicense.InternationalLicenseID);
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
