using DVLD.Common_Classes;
using DVLD.Properties;
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
    public partial class ctrlLicenseDetails : UserControl
    {
        public ctrlLicenseDetails()
        {
            InitializeComponent();
        }

        public int LicenseID
        {
            get; set;
        }

        public void LoadLicenseInfo(int licenseID)
        {
            clsLicense license = clsLicense.Find(licenseID);
            
            LicenseID = license.LicenseID;

            clsPerson personInfo = clsPerson.Find(license.PersonID);

            lblClassName.Text = clsLicenseClass.GetLicenseClassName(license.LicenseClassID);
            lblPersonName.Text = personInfo.FullName;
            lblLicenseID.Text = license.LicenseID.ToString();
            lblNationalNo.Text = personInfo.NationalNo;
            lblGender.Text = personInfo.Gender.ToString();
            lblIssueDate.Text = clsUtil.CustomShortDate(license.IssueDate);
            lblIssueReason.Text = license.GetIssueReasonText();
            if(license.Notes == "")
                lblNotes.Text = "No Notes";
            else
                lblNotes.Text = license.Notes;

            lblIsActive.Text = license.IsActive == true ? "Yes" : "No";
            lblDateOfBirth.Text = clsUtil.CustomShortDate(personInfo.DateOfBirth);
            lblDriverID.Text = license.DriverID.ToString();
            lblExpirationDate.Text = clsUtil.CustomShortDate(license.ExpirationDate);
            //we will handle it later

            
            lblIsDetained.Text = clsDetainedLicense.IsLicenseDetained(licenseID) ?  "Yes" : "No";

            if (string.IsNullOrEmpty(personInfo.ImagePath))
            {
                ppPersonImage.Image = personInfo.Gender == clsPerson.enGender.Male ? Resources.man : Resources.woman;
            }
            else
                ppPersonImage.ImageLocation = personInfo.ImagePath;
        }

        private void ctrlLicenseDetails_Load(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void lblDriverID_Click(object sender, EventArgs e)
        {

        }
    }
}
