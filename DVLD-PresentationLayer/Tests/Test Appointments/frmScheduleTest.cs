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
using static DVLD_BusinessLayer.clsTestType;

namespace DVLD.Tests
{
    public partial class frmScheduleTest : Form
    {
        int _LDLAppID;
        clsTestType.enTestType _TestType;
        clsLocalDrivingLicenseApplication _LDLApplication;
        bool _DoesFailPrevTest = false;
        clsApplication _RetakeTestApp;

        int _TestAppointmentID = -1;
        clsTestAppointment _TestAppointment;
        enum enMode
        {
            AddNew = 0, Update
        }
        enMode _Mode;
        public frmScheduleTest(int LDLAppID, clsTestType.enTestType TestType)
        {
            //add new
            InitializeComponent();
            _LDLAppID = LDLAppID;
            _TestType = TestType;
            _Mode = enMode.AddNew;
        }

        public frmScheduleTest(int TestAppointmentID) {
            //Update
            InitializeComponent();
            _TestAppointmentID = TestAppointmentID;
            _Mode = enMode.Update;
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);
            _TestType = _TestAppointment.TestTypeID;
            _LDLAppID = _TestAppointment.LDLAppID;
        }

        private void _LoadRetakeTestInfo()
        {
            _DoesFailPrevTest = clsLocalDrivingLicenseApplication.DoesFailPrevTest(_LDLAppID, _TestType);
            if (!_DoesFailPrevTest)
            {
                gbRetakeTestInfo.Enabled = false;
                lblTotalFees.Text = lblTestFees.Text;
                return;
            }



            gbRetakeTestInfo.Enabled = true;

            lblTestTittle.Text = "Schedule Retake Test";

            if (_Mode == enMode.AddNew) {
                _RetakeTestApp = new clsApplication();
                lblRetakeAppFees.Text = clsApplicationType.GetApplicationFees(7).ToString();
                lblTotalFees.Text
                     = (Convert.ToDouble( lblTestFees.Text) + Convert.ToDouble(lblRetakeAppFees.Text)).ToString() ;
            }
            else
            {
                //update mode

                if (_TestAppointment.RetakeTestApplicationID != -1)
                {
                    _RetakeTestApp = clsApplication.Find(_TestAppointment.RetakeTestApplicationID);

                    //should get fees from Application type table to be uptodated;
                    lblRetakeTestAppID.Text = _RetakeTestApp.ApplicationID.ToString();
                    lblRetakeAppFees.Text = _RetakeTestApp.PaidFees.ToString();

                    lblTotalFees.Text = (_TestAppointment.PaidFees + _RetakeTestApp.PaidFees).ToString();

                    return;
                }
                lblRetakeAppFees.Text = "0";
                lblTotalFees.Text = _TestAppointment.PaidFees.ToString();
                lblRetakeTestAppID.Text = "N/A";

            }


        }
        private void _LoadApplicationInfo()
        {
            
            _LDLApplication = clsLocalDrivingLicenseApplication.Find(_LDLAppID);
            lblLDLAppID.Text = _LDLAppID.ToString();
            lblLicenseClass.Text = _LDLApplication.LicenseClassName;
            lblApplicantName.Text = _LDLApplication.ApplicantFullName;
            lblTrial.Text = clsLocalDrivingLicenseApplication.GetTestTrialCount(_LDLAppID, _TestType).ToString();


            if(_Mode == enMode.AddNew)
            {
                lblTestFees.Text = clsTestType.GetTestFees(_TestType).ToString();
                dateTimePicker1.Value = DateTime.Now;
            }
            else
            {
                lblTestFees.Text = _TestAppointment.PaidFees.ToString();
                dateTimePicker1.Value = _TestAppointment.AppointmentDate;
            }
        }
        private void _LoadTestNameAndImage(clsTestType.enTestType TestType)
        {
            ppTestPicture.Image =
                TestType == clsTestType.enTestType.Vision ? Resources.Vision_512 :
                (TestType == clsTestType.enTestType.Written ? Resources.Written_Test_512 : Resources.driving_test_512);

            gbTestName.Text = TestType.ToString() + " Test";

        }
        private void ScheduleTest_Load(object sender, EventArgs e)
        {

            _LoadApplicationInfo();

            _LoadRetakeTestInfo();

            if (_Mode == enMode.Update && _TestAppointment.IsLocked)
            {               

                btnSave.Enabled = false;
                dateTimePicker1.Enabled = false;

                lblAlert.Text = "This person already sat for this test, appointment is locked!";
                lblAlert.Visible = true;

            }
            if(_Mode == enMode.AddNew)
            {
                _TestAppointment = new clsTestAppointment();
            }


            _LoadTestNameAndImage(_TestType);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _FillRetakeTestAppWithData()
        {
            _RetakeTestApp.ApplicantPersonID = _LDLApplication.ApplicantPersonID;
            _RetakeTestApp.ApplicationDate = DateTime.Now;
            _RetakeTestApp.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _RetakeTestApp.ApplicationTypeID = clsApplication.enApplicationType.RetakeTest;
            _RetakeTestApp.CreatedByUserID = clsGlobalSettings.LoggedInUserID;
            _RetakeTestApp.LastStatusDate = DateTime.Now;
            _RetakeTestApp.PaidFees = clsApplicationType.GetApplicationFees(7);
        }
        private void _FillTestAppointmentWithData()
        {
            _TestAppointment.LDLAppID = _LDLAppID;
            _TestAppointment.AppointmentDate = dateTimePicker1.Value;
            _TestAppointment.CreatedByUserID = clsGlobalSettings.LoggedInUserID;
            _TestAppointment.PaidFees = clsTestType.GetTestFees(_TestType);
            _TestAppointment.TestTypeID = _TestType;
        }
        private void _ConnectRetakeTestAppWithTestAppointment()
        {
            _TestAppointment.RetakeTestApplicationID = _RetakeTestApp.ApplicationID;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            _LDLApplication.LastStatusDate = DateTime.Now;
            _LDLApplication.Save();

            if (_Mode == enMode.AddNew)
            {
                _FillTestAppointmentWithData();

                if (_DoesFailPrevTest)
                {
                    _FillRetakeTestAppWithData();

                    if (_RetakeTestApp.Save())
                        _ConnectRetakeTestAppWithTestAppointment();

                }



                if(_TestAppointment.Save())
                {
                    MessageBox.Show("Appointment saved successfully.","Done",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    this.Close();
                }else
                    MessageBox.Show("Failed to save this appointment!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            //update mode

            if (_DoesFailPrevTest && _TestAppointment.RetakeTestApplicationID != -1)
            {
                //if he has RetakeTestApplication 
                _RetakeTestApp.LastStatusDate = DateTime.Now;

                //here should updated application test fees from TestType table

            }
            _TestAppointment.AppointmentDate = dateTimePicker1.Value;

            if (_TestAppointment.Save())
            {
                MessageBox.Show("Appointment updated successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
                
        }
    }
}
