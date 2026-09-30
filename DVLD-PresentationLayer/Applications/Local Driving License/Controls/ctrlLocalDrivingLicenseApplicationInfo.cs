using DVLD.Common_Classes;
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

namespace DVLD.Applications
{
    public partial class ctrlLocalDrivingLicenseApplicationInfo : UserControl
    {
        public ctrlLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        clsLocalDrivingLicenseApplication _DrivingLicenseApp;

        public void LoadInfo(int LocalDrivingLicenseID)
        {
            //Load Driving License Application Info
            _DrivingLicenseApp =
                clsLocalDrivingLicenseApplication.Find(LocalDrivingLicenseID);
            lblLocalLicenseID.Text = LocalDrivingLicenseID.ToString();
            lblLicenseClass.Text = clsLicenseClass.GetLicenseClassName(_DrivingLicenseApp.LicenseClassID);
            lblPassedTestsCount.Text = _DrivingLicenseApp.GetPassedTestsCount() + "/3";

            //Load Application basic info
            lblApplicationID.Text = _DrivingLicenseApp.ApplicationID.ToString();
            lblStatus.Text = _DrivingLicenseApp.GetStatusText();
            lblFees.Text = _DrivingLicenseApp.PaidFees.ToString();
            lblType.Text = clsApplicationType.GetApplicationTypeTitle((int)_DrivingLicenseApp.ApplicationTypeID).ToString();
            lblApplicant.Text = _DrivingLicenseApp.ApplicantFullName.ToString();
            lblApplicationDate.Text = clsUtil.CustomShortDate(_DrivingLicenseApp.ApplicationDate);
            lblStatusDate.Text = clsUtil.CustomShortDate(_DrivingLicenseApp.LastStatusDate);
            lblCreatedBy.Text = clsUser.Find(_DrivingLicenseApp.CreatedByUserID).UserName;


        }

        private void ctrlLocalDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {
            
        }

        private void lblViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            frmShowPersonInfo frm = new frmShowPersonInfo(_DrivingLicenseApp.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}
