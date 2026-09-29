using DVLD.Common_Classes;
using DVLD_BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD.Users.frmAddEditUser;

namespace DVLD.Applications
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {

        public delegate void DataBackEventHandler(bool IsUpdated);

        // 2. Declare event based on delegate
        public event DataBackEventHandler DataBack;

        bool _IsUpdated = false;

        enum enMode { AddNew, Update };
        enMode _Mode;

        int _PersonID = -1;
        string _NationalNo = "";

        int _LocalDrivingLicenseApplicationID = -1;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public frmAddUpdateLocalDrivingLicenseApplication(int ApplicationID)
        {
            _Mode = enMode.Update;
            _LocalDrivingLicenseApplicationID = ApplicationID;
            InitializeComponent();
        }

        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            _Mode = enMode.AddNew;
            InitializeComponent();
        }

        private void _FillComboBoxWithLicenseClasses()
        {
            DataTable dtLicenseClasses = clsLicenseClass.ListAllLicenseClasses();

            cbLicenseClasses.DataSource = dtLicenseClasses;
            cbLicenseClasses.DisplayMember = "ClassName";
            cbLicenseClasses.ValueMember = "LicenseClassID";

            //cbLicenseClasses.SelectedValue

            cbLicenseClasses.SelectedValue = 3;
        }

        private void _LoadInfo()
        {
            _FillComboBoxWithLicenseClasses();


            if (_Mode == enMode.AddNew)
            {
                pnlApplicationInfo.Enabled = false;
                btnSave.Enabled = false;

                lblAddEditApplication.Text = "Add New Local Driving License Application";

                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();

                lblFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString(); 
                lblCreatedBy.Text = clsGlobalSettings.LoggedInUserName.ToString() ;

                //btnNext.Enabled = false;
                return;
            }

            btnNext.Enabled = true;
            //tabControl1.SelectedIndex = 0;  
            lblAddEditApplication.Text = "Update Local Driving License Application";

            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_LocalDrivingLicenseApplicationID);

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);

            ctrlPersonCardWithFilter1.FilterEnabled = false;

            lblLocalApplicationID.Text = _LocalDrivingLicenseApplicationID.ToString();

            lblApplicationDate.Text = _LocalDrivingLicenseApplication.ApplicationDate.ToShortDateString();

            //class

            lblFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedBy.Text = _LocalDrivingLicenseApplication.CreatedByUserID.ToString();
            cbLicenseClasses.SelectedValue = (int)_LocalDrivingLicenseApplication.LicenseClassID;


        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _LoadInfo();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            
            _PersonID = ctrlPersonCardWithFilter1.PersonID;            

            if (_PersonID == -1 && _NationalNo == "")
            {
                MessageBox.Show("Please select person first!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            //if mode == update
            pnlApplicationInfo.Enabled = true;
            btnSave.Enabled = _Mode == enMode.AddNew;

            tabControl1.SelectedIndex = 1;
    
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            _IsUpdated = false; 
            _LocalDrivingLicenseApplication.LicenseClassID = (int)cbLicenseClasses.SelectedValue;

            clsLocalDrivingLicenseApplication ActiveApplication =
                clsLocalDrivingLicenseApplication.GetPersonActiveApplicationLicenseWithClass(_PersonID, _LocalDrivingLicenseApplication.LicenseClassID);

            if (_Mode == enMode.Update) {

                if (ActiveApplication != null && _LocalDrivingLicenseApplication.ID != ActiveApplication.ID)
                {
                    MessageBox.Show($"The selected person has already active application with same selected license class with ID = " +
                        $"{ActiveApplication.ID.ToString()}, choose another license class!","failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_LocalDrivingLicenseApplication.Save())
                {
                    MessageBox.Show($"Application Updated successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _IsUpdated = true;
                    return;
                }
              
                MessageBox.Show("Failed to updated this application", "failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If AddNew Mode

            

            if (ActiveApplication != null) {
                MessageBox.Show($"The selected person has already active application with same selected license class with ID = " +
                    $"{ActiveApplication.ID.ToString()}, choose another license class!","failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (clsLicense.DoesHaveLicenseOfLicenseClass(_PersonID, _LocalDrivingLicenseApplication.LicenseClassID))
            {
                MessageBox.Show($"The selected person has already license with the same license class, choose another license class!" ,
                    "failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _LocalDrivingLicenseApplication.ApplicantPersonID = _PersonID;
            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationTypeID = clsApplication.enApplicationType.NewLocalLicense;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            //_LocalDrivingLicenseApplication.PaidFees
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobalSettings.LoggedInUserID;


            if (_LocalDrivingLicenseApplication.Save())
            {
                MessageBox.Show($"Application saved successfully with ID = {_LocalDrivingLicenseApplication.ID}.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Mode = enMode.Update;
                _LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.ID;
                _LoadInfo();
                _IsUpdated = true;
            }
            else
            {
                MessageBox.Show("Failed to save this application", "failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DataBack?.Invoke(_IsUpdated);
            this.Close();
        }

        private void cbLicenseClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update && _LocalDrivingLicenseApplication != null)
            {
                btnSave.Enabled = (int)_LocalDrivingLicenseApplication.LicenseClassID != (int)cbLicenseClasses.SelectedValue;
            }
        }
    }
}
