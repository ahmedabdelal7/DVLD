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

namespace DVLD.Applications.Replacement_For_Damaged_Or_Lost_License
{
    public partial class frmReplacementForDamageOrLostLicense : Form
    {

        enum enReplacementFor
        {
            DamagedLicense, LostLicense
        }

        enReplacementFor _ReplacementFor = enReplacementFor.DamagedLicense;

        clsLicense _OldLicense;
        clsLicense _ReplacedLicense;

        public frmReplacementForDamageOrLostLicense()
        {
            InitializeComponent();
        }

        private void rbDamaged_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDamaged.Checked)
            {
                lblTitle.Text = "Replacement For Damaged";
                _ReplacementFor = enReplacementFor.DamagedLicense;
                lblApplicationFees.Text = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.ReplaceDamagedLicense).ToString();
            }
            else
            {
                lblTitle.Text = "Replacement For Lost";
                _ReplacementFor = enReplacementFor.LostLicense;
                lblApplicationFees.Text = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.ReplaceLostLicense).ToString();
            }
        }

        private void frmReplacementForDamageOrLostLicense_Load(object sender, EventArgs e)
        {
            lblAppDate.Text = clsUtil.CustomShortDate(DateTime.Now);
            lblIssueDate.Text = clsUtil.CustomShortDate(DateTime.Now);
            lblApplicationFees.Text = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.ReplaceDamagedLicense).ToString();
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.UserName;
            btnIssueReplacement.Enabled = false;
            lnkShowLicenseInfo.Enabled = false;
            lnkShowLicensesHistory.Enabled = false;

            this.AcceptButton = ctrlFindLicenseWithFilter1.btnFindLicense;
            rbDamaged.Checked = true;


        }

        private void ctrlFindLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            int LicenseID = obj;
            btnIssueReplacement.Enabled = false;
            lnkShowLicenseInfo.Enabled = false;
            lnkShowLicensesHistory.Enabled = true;

            if (LicenseID == -1)
            {
                lnkShowLicensesHistory.Enabled = false;
                lblOldLicenseID.Text = "??";
                lblExpirationDate.Text = "??";
                return;
            }

            _OldLicense = clsLicense.Find(LicenseID);

            lblOldLicenseID.Text = _OldLicense.LicenseID.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(_OldLicense.LicenseClassInfo.DefaultValidityLength).ToString();


            //check if license expired
            int IsExpired = DateTime.Compare(_OldLicense.ExpirationDate, DateTime.Now);

            if (IsExpired <= 0)
            {
                //the license is expired.
                MessageBox.Show($"This license is Expired ,Please choose another license.",
                    "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (!_OldLicense.IsActive)
            {
                MessageBox.Show($"This license is not active, please choose another license.",
                   "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            btnIssueReplacement.Enabled = true;
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            DialogResult msgResult = MessageBox.Show($"Are you sure you want to Replace this license [{_OldLicense.LicenseID}] ?",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msgResult == DialogResult.No)
                return;


            //Deactivate old License.
            _OldLicense.Deactivate();

            //create new application with status completed
            clsApplication application = new clsApplication();
            application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            application.ApplicantPersonID = _OldLicense.PersonID;
            if (_ReplacementFor ==  enReplacementFor.LostLicense)
                application.ApplicationTypeID = clsApplication.enApplicationType.ReplaceLostLicense;
            else
                application.ApplicationTypeID = clsApplication.enApplicationType.ReplaceDamagedLicense;
        
            application.PaidFees = Convert.ToDouble(lblApplicationFees.Text);
            application.ApplicationDate = DateTime.Now;
            application.LastStatusDate = DateTime.Now;
            application.ApplicantPersonID = _OldLicense.PersonID;
            application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            application.Save();


            //create ne license
            _ReplacedLicense = new clsLicense();

            _ReplacedLicense.ApplicationID = application.ApplicationID;
            _ReplacedLicense.IssueDate = DateTime.Now;
            _ReplacedLicense.ExpirationDate = DateTime.Now.AddYears(_OldLicense.LicenseClassInfo.DefaultValidityLength);
            _ReplacedLicense.IsActive = true;
            _ReplacedLicense.DriverID = _OldLicense.DriverID;
            _ReplacedLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            _ReplacedLicense.IssueReason =
                (_ReplacementFor == enReplacementFor.LostLicense ?
                clsLicense.enIssueReason.ReplacementForLost : clsLicense.enIssueReason.ReplacementForDamaged);
            _ReplacedLicense.PaidFees = _OldLicense.PaidFees;
            _ReplacedLicense.Notes = _OldLicense.Notes;
            _ReplacedLicense.LicenseClassID = _OldLicense.LicenseClassID;

            if (_ReplacedLicense.Save())
            {
                MessageBox.Show($"License Replaced successfully with id [{_ReplacedLicense.LicenseID}] ?",
                    "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ctrlFindLicenseWithFilter1.EnableFilter = false;

                btnIssueReplacement.Enabled = false;
                lnkShowLicenseInfo.Enabled = true;
                gbReplacementFor.Enabled = false;

                lblReplaceLicenseAppID.Text = application.ApplicationID.ToString();
                lblReplacedLicenseID.Text = _ReplacedLicense.LicenseID.ToString();
            }
            else
            {
                MessageBox.Show($"Failed",
                    "Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void lnkShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseHistory frm = new frmDriverLicenseHistory(_OldLicense.PersonID);
            frm.ShowDialog();
        }

        private void lnkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseInfo frm = new frmDriverLicenseInfo(_ReplacedLicense.LicenseID);
            frm.ShowDialog();
        }
    }
}
