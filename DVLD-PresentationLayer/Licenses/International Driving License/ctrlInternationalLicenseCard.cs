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

namespace DVLD.Licenses.International_Driving_License
{
    public partial class ctrlInternationalLicenseCard : UserControl
    {
        public ctrlInternationalLicenseCard()
        {
            InitializeComponent();
        }

        public void LoadIntLicenseInfo(int IntLicenseID)
        {
            clsInternationalLicense IntLicense = clsInternationalLicense.Find(IntLicenseID);
            clsPerson PersonInfo =  clsPerson.Find(IntLicense.ApplicantPersonID);

            if (IntLicense != null) { 
                
                lblIntLicenseID.Text = IntLicenseID.ToString();
                lblLicenseID.Text = IntLicense.IssuedUsingLocalLicenseID.ToString();    
                lblName.Text = PersonInfo.FullName;
                lblNationalNo.Text = PersonInfo.NationalNo;
                lblGender.Text =  (PersonInfo.Gender == clsPerson.enGender.Male ? "Male" : "Female");
                lblIssueDate.Text = clsUtil.CustomShortDate(IntLicense.IssueDate);
                lblApplicationID.Text = IntLicense.ApplicationID.ToString();
                lblIsActive.Text = IntLicense.IsActive == true ? "true" : "false";
                lblDateOfBirth.Text = clsUtil.CustomShortDate(PersonInfo.DateOfBirth);
                lblDriverID.Text = IntLicense.DriverID.ToString();
                lblExpirationDate.Text = clsUtil.CustomShortDate(IntLicense.ExpirationDate);

                if (string.IsNullOrEmpty(PersonInfo.ImagePath))
                {
                    ppPersonImage.Image = PersonInfo.Gender == clsPerson.enGender.Male ? Resources.man : Resources.woman;
                }
                else
                    ppPersonImage.ImageLocation = PersonInfo.ImagePath;

            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
