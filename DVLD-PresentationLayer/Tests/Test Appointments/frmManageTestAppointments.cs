using DVLD.Common_Classes;
using DVLD.People;
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
    public partial class frmManageTestAppointments : Form
    {
        enum enMode
        {
            VisionTest = 1, WrittenTest, PracticalTest

        }
        enMode _Mode;
        clsTestType.enTestType _TestType;

        int _LDLAppID = -1;
        DataTable _dtTestAppointments;
        public frmManageTestAppointments(clsTestType.enTestType testType, int LDLAppID)
        {
            _Mode = (enMode)testType;
            _TestType = testType;
            _LDLAppID = LDLAppID;
            InitializeComponent();
        }

        private void _FillDataViewWithTestAppointments()
        {
           // dgvTestAppointments.Columns.Clear();
           dgvTestAppointments.Rows.Clear();    
            _dtTestAppointments = clsTestAppointment.ListAllTestAppointments(_LDLAppID, _TestType);
            //dgvTestAppointments.Columns[0].Width = 100;

            foreach (DataRow row in _dtTestAppointments.Rows) { 
                dgvTestAppointments.Rows.Add(
                        row[0].ToString(),
                        clsUtil.CustomShortDate( (DateTime)row[1]),
                        row[2].ToString(),
                        row[3]
                    );
            }

            lblRecordsCount.Text = dgvTestAppointments.RowCount.ToString(); 

        }
        private void _LoadInfo()
        {
            ctrlLocalDrivingLicenseApplicationInfo1.LoadInfo(_LDLAppID);
            _dtTestAppointments = clsTestAppointment.ListAllTestAppointments(_LDLAppID, _TestType);
            _FillDataViewWithTestAppointments();

            if (_Mode == enMode.VisionTest)
            {
                lblTestTittle.Text = "Vision Test Appointments";
                ppTestPicture.Image = Resources.Vision_512;
                this.Text = "Vision Test Appointments";

                return;
            }
            if (_Mode == enMode.WrittenTest) {
                lblTestTittle.Text = "Written Test Appointments";
                ppTestPicture.Image = Resources.Written_Test_512;
                this.Text = "Written Test Appointments";
                return ;
            }
            if (_Mode == enMode.PracticalTest)
            {
                lblTestTittle.Text = "Practical Test Appointments";
                ppTestPicture.Image = Resources.driving_test_512;
                this.Text = "Practical Test Appointments";
                return;
            }

        }

        private void frmManageTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadInfo();

        }

        private void ctrlLocalDrivingLicenseApplicationInfo1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(clsLocalDrivingLicenseApplication.DoesHaveActiveTestAppointment(_LDLAppID,_TestType))
            {
                MessageBox.Show("Failed, this person already has an active appointment!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (clsLocalDrivingLicenseApplication.DoesPassTestType(_LDLAppID, _TestType))
            {
                MessageBox.Show("This person already passed this test before, you can only retake failed tests.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            frmScheduleTest frmScheduleTest = new frmScheduleTest(_LDLAppID, _TestType);
            frmScheduleTest.ShowDialog();
            _LoadInfo();
        }

        private int _GetSelectedTestAppointmentID()
        {
            int SelectedAppID = -1;
            try
            {
                SelectedAppID = int.Parse(dgvTestAppointments.SelectedCells[0].Value.ToString());
            }
            catch { SelectedAppID = -1; }
            return SelectedAppID;
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            frmScheduleTest frmScheduleTest = new frmScheduleTest(_GetSelectedTestAppointmentID());
            frmScheduleTest.ShowDialog();
            _LoadInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppID = _GetSelectedTestAppointmentID();

            clsTestAppointment TestApp = clsTestAppointment.Find(TestAppID);

            if (TestApp.IsLocked)
            {
                MessageBox.Show("This person already took this test before, the test is locked!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmTakeTest frmTakeTest = new frmTakeTest(TestAppID);
            frmTakeTest.ShowDialog();
            _LoadInfo();
        }
    }
}
