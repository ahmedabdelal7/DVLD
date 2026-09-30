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
    public partial class frmReleaseDetainedLicense : Form
    {
        int _LicenseID = -1;
        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        public frmReleaseDetainedLicense(int LicenseID)
        {
            _LicenseID = LicenseID;   
            InitializeComponent();
        }
        clsLicense _License;
        clsDetainedLicense _DetainedInfo;
        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            //lblDetainDate.Text = clsUtil.CustomShortDate(DateTime.Now);

            

            lblCreatedByUserID.Text = clsGlobalSettings.LoggedInUserName;
            lblApplicationFees.Text = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.ReleaseDetainedLicense).ToString();

            if(_LicenseID != -1)
            {

                ctrlFindLicenseWithFilter1._LoadByLicenseID(_LicenseID);

                _License = clsLicense.Find(_LicenseID);

                lblLicenseID.Text = _License.LicenseID.ToString();

                if (!clsDetainedLicense.IsLicenseDetained(_LicenseID))
                {
                    MessageBox.Show($"This license is not detained, please choose another license.",
                       "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblDetainDate.Text = "??";
                    lblFineFees.Text = "[$$$]";
                    lblTotalFees.Text = "[$$$]";
                    lblDetainLicenseID.Text = "[???]";


                }
                else
                {
                    _DetainedInfo = clsDetainedLicense.FindByLicenseID(_LicenseID);
                    lblDetainLicenseID.Text = _DetainedInfo.DetainID.ToString();
                    lblDetainDate.Text = clsUtil.CustomShortDate(_DetainedInfo.DetainDate);
                    lblFineFees.Text = Convert.ToDouble(_DetainedInfo.FineFees).ToString();
                    lblTotalFees.Text = (Convert.ToDouble(lblFineFees.Text) + Convert.ToDouble(lblApplicationFees.Text)).ToString();

                }


            }
            else
            {
                btnRelease.Enabled = false;
                lnkShowLicenseInfo.Enabled = false;
                lnkShowLicensesHistory.Enabled = false;

                this.AcceptButton = ctrlFindLicenseWithFilter1.btnFindLicense;

            }


            


        }

        private void ctrlFindLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            
            int LicenseID = obj;
            btnRelease.Enabled = false;
            lnkShowLicenseInfo.Enabled = false;
            lnkShowLicensesHistory.Enabled = true;

            if (LicenseID == -1)
            {
                lnkShowLicensesHistory.Enabled = false;
                lblLicenseID.Text = "[???]";
                lblDetainDate.Text = "??";
                lblFineFees.Text = "[$$$]";
                lblTotalFees.Text = "[$$$]";

                return;
            }

            _License = clsLicense.Find(LicenseID);

            lblLicenseID.Text = _License.LicenseID.ToString();

            if (!clsDetainedLicense.IsLicenseDetained(LicenseID))
            {
                MessageBox.Show($"This license is not detained, please choose another license.",
                   "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblDetainDate.Text = "??";
                lblFineFees.Text = "[$$$]";
                lblTotalFees.Text = "[$$$]";
                lblDetainLicenseID.Text = "[???]";

                return;
            }

            _DetainedInfo = clsDetainedLicense.FindByLicenseID(LicenseID);
            lblDetainLicenseID.Text = _DetainedInfo.DetainID.ToString();
            lblDetainDate.Text = clsUtil.CustomShortDate(_DetainedInfo.DetainDate);
            lblFineFees.Text = Convert.ToDouble(_DetainedInfo.FineFees).ToString();
            lblTotalFees.Text = (Convert.ToDouble(lblFineFees.Text)+ Convert.ToDouble(lblApplicationFees.Text)).ToString();

            btnRelease.Enabled = true;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            DialogResult msgResult = MessageBox.Show($"Are you sure you want to Release this license [{_License.LicenseID}] ?",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msgResult == DialogResult.No)
                return;

            //create application with type releaseDetainedLicense

            clsApplication app = new clsApplication();
            app.ApplicantPersonID = _License.PersonID;
            app.ApplicationTypeID = clsApplication.enApplicationType.ReleaseDetainedLicense;
            app.ApplicationStatus= clsApplication.enApplicationStatus.Completed;
            app.PaidFees = clsApplicationType.GetApplicationFees((int)app.ApplicationTypeID);
            app.CreatedByUserID = clsGlobalSettings.LoggedInUserID;

            app.Save();

            //

            _DetainedInfo.ReleaseApplicationID = app.ApplicationID;

            if (_DetainedInfo.ReleaseDetainedLicense(clsGlobalSettings.LoggedInUserID, app.ApplicationID))
            {
                lblReleaseAppID.Text = _DetainedInfo.ReleaseApplicationID.ToString();

                MessageBox.Show($"License Released successfully with id [{_DetainedInfo.ReleaseApplicationID}] ?",
                    "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ctrlFindLicenseWithFilter1.EnableFilter = false;

                btnRelease.Enabled = false;
                lnkShowLicenseInfo.Enabled = true;

            }
            else
            {
                MessageBox.Show($"Failed",
                    "Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();   
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
