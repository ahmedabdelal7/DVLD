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

namespace DVLD.License
{
    public partial class frmIssueLicense : Form
    {
        int _LDLAppID = -1;
        clsLocalDrivingLicenseApplication _LDLApp;
        clsDriver _Driver;
        int _LicenseID = -1;
        public frmIssueLicense(int LDLAppID)
        {
            _LDLAppID = LDLAppID;
            InitializeComponent();
        }

        private void frmIssueLicense_Load(object sender, EventArgs e)
        {
            ctrlLocalDrivingLicenseApplicationInfo1.LoadInfo(_LDLAppID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }


        private bool _CreateNewDriver(clsDriver clsDriver)
        {
            clsDriver.PersonID = _LDLApp.ApplicantPersonID;
            clsDriver.CreatedByUserID = _LDLApp.CreatedByUserID;
            clsDriver.CreatedDate = DateTime.Now;

            return clsDriver.Save();
        }
        private bool _CreateNewLicense(clsLicense license)
        {
            clsLicenseClass licenseClass = clsLicenseClass.Find(_LDLApp.LicenseClassID);

            license.ApplicationID = _LDLApp.ApplicationID;
            license.DriverID = _Driver.DriverID;
            license.LicenseClassID = _LDLApp.LicenseClassID;
            license.IssueDate = DateTime.Now;
            license.ExpirationDate =license.IssueDate.AddYears(licenseClass.DefaultValidityLength);
            license.Notes = txtNotes.Text.Trim();
            license.PaidFees = licenseClass.ClassFees;
            license.IsActive = true;
            license.IssueReason = clsLicense.enIssueReason.FirstTime;
            license.CreatedByUserID = clsGlobalSettings.LoggedInUserID;
            _LicenseID = license.LicenseID;
            return license.Save();
            
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            _LDLApp =  clsLocalDrivingLicenseApplication.Find(_LDLAppID);

            //if driver not exist by person id

            _Driver = clsDriver.FindByPersonID(_LDLApp.ApplicantPersonID);

            if(_Driver == null)
            {
                //person is not a driver, create new driver

                _Driver = new clsDriver();
                _CreateNewDriver(_Driver);
            }

            clsLicense License = new clsLicense();

            if (_CreateNewLicense(License))
            {
                _LDLApp.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                _LDLApp.LastStatusDate = DateTime.Now;
                
                _LDLApp.Save();

                MessageBox.Show($"License issued successfully with ID = {License.LicenseID}",
                    "Successful",MessageBoxButtons.OK,MessageBoxIcon.Information);

                //...created event on license issued and send licenseID
                this.Close();
                frmDriverLicenseInfo card = new frmDriverLicenseInfo(License.LicenseID);
                card.ShowDialog();
            }
        }
    }
}
