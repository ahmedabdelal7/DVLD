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


namespace DVLD.Tests
{
    public partial class frmTakeTest : Form
    {

        
        clsTestAppointment _TestAppointment;
        clsLocalDrivingLicenseApplication _LDLApplication;
        clsTestType.enTestType _TestType;
        public frmTakeTest(int TestAppointmentID)
        {
            _TestAppointment =  clsTestAppointment.Find(TestAppointmentID);
            _LDLApplication = clsLocalDrivingLicenseApplication.Find(_TestAppointment.LDLAppID);
            _TestType = _TestAppointment.TestTypeID;
            
            InitializeComponent();
        }
        private void _LoadTestNameAndImage(clsTestType.enTestType TestType)
        {
            ppTestPicture.Image =
                TestType == clsTestType.enTestType.Vision ? Resources.Vision_512 :
                (TestType == clsTestType.enTestType.Written ? Resources.Written_Test_512 : Resources.driving_test_512);

            gbTestName.Text = TestType.ToString() + " Test";

        }
        private void frmTakeTest_Load(object sender, EventArgs e)
        {

            lblLDLAppID.Text = _TestAppointment.LDLAppID.ToString();
            lblTestAppointmentDate.Text = _TestAppointment.AppointmentDate.ToString();
            lblTestFees.Text = _TestAppointment.PaidFees.ToString();

            lblApplicantName.Text = _LDLApplication.ApplicantFullName;
            lblLicenseClass.Text = _LDLApplication.LicenseClassName;
            lblTrial.Text = clsLocalDrivingLicenseApplication.GetTestTrialCount(_LDLApplication.ID, _TestType).ToString();
            rbPass.Checked = true;


            gbTestName.Text = _TestAppointment.TestTypeID.ToString() + " Test";


            _LoadTestNameAndImage(_TestType);


        }

        private bool _UpdateRetakeTestAppInfo(clsApplication RetakeTestApp)
        {
            if (RetakeTestApp != null)
            {
                RetakeTestApp.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                RetakeTestApp.LastStatusDate = DateTime.Now;
                return RetakeTestApp.Save();
            }return false;

        }
        private bool _UpdateLDLAppInfo()
        {
            _LDLApplication.LastStatusDate = DateTime.Now;
            return _LDLApplication.Save();

        }
        private void _FillTestData(clsTest Test)
        {
            bool IsPass = rbPass.Checked;
            Test.TestAppointmentID = _TestAppointment.TestAppointmentID;
            Test.TestResult = IsPass;
            Test.Notes = txtNotes.Text.Trim();
            Test.CreatedByUserID = clsGlobalSettings.LoggedInUserID;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            

            if (_TestAppointment.RetakeTestApplicationID != -1) {
                clsApplication RetakeTestApp = clsApplication.Find(_TestAppointment.RetakeTestApplicationID);
                if (!_UpdateRetakeTestAppInfo(RetakeTestApp))
                {
                    MessageBox.Show("Failed to Save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            if (!_UpdateLDLAppInfo())
            {
                MessageBox.Show("Failed to Save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //created test
            clsTest Test = new clsTest();
            _FillTestData(Test);


            if (Test.Save())
            {
                _TestAppointment.IsLocked = true;
                if (_TestAppointment.Save())
                {
                    MessageBox.Show("Data saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else
                    MessageBox.Show("Failed to Save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }           
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
