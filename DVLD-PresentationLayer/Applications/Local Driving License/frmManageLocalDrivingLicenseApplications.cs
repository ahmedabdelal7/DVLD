using DVLD.Applications.Local_Driving_License_Applications;
using DVLD.Common_Classes;
using DVLD.License;
using DVLD.Licenses;
using DVLD.Tests;
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
    public partial class frmManageLocalDrivingLicenseApplications : Form
    {
        public frmManageLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }
        DataTable _dtApplications;

        enum enFilterBy
        {
            None = 0, LDLAppID, NationalNo, FullName, Status
        }

        enum enStatus
        {
            All = 0, New, Cancelled, Completed
        }


        private void _FillDataViewWithApplications(DataTable dataTable)
        {
            dgvLDLApplications.Rows.Clear();
            foreach (DataRow row in dataTable.Rows)
            {

                dgvLDLApplications.Rows.Add(
                    row["LocalDrivingLicenseApplicationID"],
                    row["ClassName"],
                    row["NationalNo"],
                    row["FullName"],
                    clsUtil.CustomShortDate((DateTime)row["ApplicationDate"]),
                    row["PassedTestCount"],
                    row["Status"]
                    );

            }
            lblRecordsCount.Text = dataTable.Rows.Count.ToString();
        }
        private void _LoadInfo()
        {
            _dtApplications = clsLocalDrivingLicenseApplication.ListAllLocalDrivingLicenseApplications();

            _FillDataViewWithApplications(_dtApplications);

            cbStatus.Visible = false;
            cbStatus.SelectedIndex = (int)enStatus.All;

            cbFilter.SelectedIndex = (int)enFilterBy.None;
            txtFilterValue.Visible = false;

            
        }
        private int _GetSelectedLicenseApplicationID()
        {
            int ID;
            try
            {
                ID = Convert.ToInt32(dgvLDLApplications.SelectedCells[0].Value);

            }
            catch
            {
                ID = -1;
            }

            return ID;
        }
        private void frmManageLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _LoadInfo();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilter.SelectedIndex == (int)enFilterBy.LDLAppID)
            {
                e.Handled = !clsValidation.IsValidInteger(sender, e);
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            cbStatus.SelectedIndex = (int)enStatus.All;

            if (cbFilter.SelectedIndex == (int)enFilterBy.None) { 
                txtFilterValue.Visible = false;
                cbStatus.Visible=false;
                return;

            }

            if (cbFilter.SelectedIndex == (int)enFilterBy.Status) { 
                txtFilterValue.Visible=false;
                cbStatus.Visible=true;
                return;
            }

            txtFilterValue.Visible = true;
            cbStatus.Visible = false;

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string findValue = txtFilterValue.Text.Trim().ToString();

            DataView dv = _dtApplications.DefaultView;

            if (dv == null) return;
            if (string.IsNullOrEmpty(findValue))
            {
                _FillDataViewWithApplications(_dtApplications);
                return;
            }
                

            if(cbFilter.SelectedIndex == (int)enFilterBy.LDLAppID)
            {
                if(int.TryParse(findValue, out int ID))
                {
                    dv.RowFilter = $"LocalDrivingLicenseApplicationID = {ID}";

                    _FillDataViewWithApplications( dv.ToTable());

                }
                return;                

            }


            if (cbFilter.SelectedIndex == (int)enFilterBy.NationalNo)
            {
                
                dv.RowFilter = $"NationalNo = '{findValue}'";

                _FillDataViewWithApplications(dv.ToTable());

                
                return;

            }

            if (cbFilter.SelectedIndex == (int)enFilterBy.FullName)
            {

                dv.RowFilter = $"FullName LIKE '%{findValue}%'";

                _FillDataViewWithApplications(dv.ToTable());


                return;

            }


        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dv = _dtApplications.DefaultView;
            if (dv == null || cbStatus.SelectedIndex == (int)enStatus.All)
            {
                _FillDataViewWithApplications(_dtApplications);
                return;
            }

            if (cbStatus.SelectedIndex == (int)enStatus.New)
            {
                dv.RowFilter = $"Status = '{enStatus.New.ToString()}'";
                _FillDataViewWithApplications(dv.ToTable());
                return;
            }

            if (cbStatus.SelectedIndex == (int)enStatus.Cancelled) {
                dv.RowFilter = $"Status = '{enStatus.Cancelled.ToString()}'";
                _FillDataViewWithApplications(dv.ToTable());
                return;
            }
            if (cbStatus.SelectedIndex == (int)enStatus.Completed)
            {
                dv.RowFilter = $"Status = '{enStatus.Completed.ToString()}'";
                _FillDataViewWithApplications(dv.ToTable());
                return;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frmAdd = new frmAddUpdateLocalDrivingLicenseApplication();
            frmAdd.DataBack += _RefreshScreen;
            frmAdd.ShowDialog();

        }

        private void _RefreshScreen(bool IsRequireRefresh)
        {
            if (IsRequireRefresh) _LoadInfo();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedID = _GetSelectedLicenseApplicationID();
            if (SelectedID == -1)
                return;

            frmAddUpdateLocalDrivingLicenseApplication frmAddUpdate = new frmAddUpdateLocalDrivingLicenseApplication(SelectedID);
            //subscribe in delegate:
            frmAddUpdate.DataBack += _RefreshScreen;
            frmAddUpdate.ShowDialog();
        }

        private void _ChangeContextUpdatePermission(bool perm)
        {
            deleteApplicationToolStripMenuItem.Enabled = perm;
            cancelApplicationToolStripMenuItem.Enabled = perm;
            scheduleTestToolStripMenuItem.Enabled = perm;
            editApplicationToolStripMenuItem.Enabled = perm;
           
        }
        private void _EnableAllContextMenuItems()
        {
            _ChangeContextUpdatePermission(true);
            showLicenseToolStripMenuItem.Enabled = true;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
            visionTestToolStripMenuItem.Enabled = true;
            practicalTestToolStripMenuItem.Enabled = true;
            writtenTestToolStripMenuItem.Enabled = true;
            showApplicationDetailsToolStripMenuItem.Enabled = true;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = true;
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
            _EnableAllContextMenuItems();

            int SelectedAppID = _GetSelectedLicenseApplicationID();
            clsLocalDrivingLicenseApplication licenseApplication  = clsLocalDrivingLicenseApplication.Find(SelectedAppID);


            
            showPersonLicenseHistoryToolStripMenuItem.Enabled = clsDriver.IsExistByPersonID(licenseApplication.ApplicantPersonID);
            

            if(licenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.Completed)
            {
                _ChangeContextUpdatePermission(false);

                showLicenseToolStripMenuItem.Enabled =//if completed and passed tests = 3 then true
                    clsLocalDrivingLicenseApplication.DoesPassTestType(SelectedAppID, clsTestType.enTestType.Practical);
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            }
            if(licenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.Cancelled)
            {
                _ChangeContextUpdatePermission(false );
                //issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                showLicenseToolStripMenuItem .Enabled = false;
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;  

            }
            if(licenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New)
            {

                //Should pass 3 tests to open issue license
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = 
                    clsLocalDrivingLicenseApplication.DoesPassTestType(SelectedAppID,clsTestType.enTestType.Practical);

                //if he has license show it 
                showLicenseToolStripMenuItem.Enabled = false;

                //handle tests

                bool PassedVision = clsLocalDrivingLicenseApplication.DoesPassTestType(SelectedAppID, clsTestType.enTestType.Vision);
                bool PassedWritten = clsLocalDrivingLicenseApplication.DoesPassTestType(SelectedAppID, clsTestType.enTestType.Written);
                bool PassedPractical = clsLocalDrivingLicenseApplication.DoesPassTestType(SelectedAppID, clsTestType.enTestType.Practical);

                visionTestToolStripMenuItem.Enabled = !PassedVision;
                writtenTestToolStripMenuItem.Enabled = (PassedVision) && !PassedWritten ;
                practicalTestToolStripMenuItem.Enabled = (PassedVision && PassedWritten) && !PassedPractical;

            }

                        
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            clsLocalDrivingLicenseApplication Application = clsLocalDrivingLicenseApplication.Find(_GetSelectedLicenseApplicationID());

            if (Application == null) {
                MessageBox.Show($"Error, application is not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LoadInfo();
                return;
            }

            DialogResult msgResult =
                MessageBox.Show($"Are you sure you want to delete this application ID = {Application.ID}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (msgResult == DialogResult.No)
                return;

            if(Application.Delete())
            {
                MessageBox.Show($"Application deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _LoadInfo();
                return;
            }

            MessageBox.Show($"Failed to delete this application, because it related to other information in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication Application = clsLocalDrivingLicenseApplication.Find(_GetSelectedLicenseApplicationID());

            if (Application == null)
            {
                MessageBox.Show($"Error, application is not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LoadInfo();
                return;
            }

            DialogResult msgResult =
                MessageBox.Show($"Are you sure you want to cancel this application ID = {Application.ID}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (msgResult == DialogResult.No)
                return;

            if (Application.CancelApplication())
            {
                MessageBox.Show($"Application cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _LoadInfo();
                return;
            }

            MessageBox.Show($"Failed to cancel this application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLAppID =  _GetSelectedLicenseApplicationID();
            frmManageTestAppointments frm = new frmManageTestAppointments(clsTestType.enTestType.Vision, LDLAppID);
            frm.ShowDialog();
            _LoadInfo();

        }

        private void writtenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLAppID = _GetSelectedLicenseApplicationID();
            frmManageTestAppointments frm = new frmManageTestAppointments(clsTestType.enTestType.Written, LDLAppID);
            frm.ShowDialog();
            _LoadInfo();
        }

        private void practicalTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLAppID = _GetSelectedLicenseApplicationID();
            frmManageTestAppointments frm = new frmManageTestAppointments(clsTestType.enTestType.Practical, LDLAppID);
            frm.ShowDialog();
            _LoadInfo();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueLicense frmIssueLicense = new frmIssueLicense(_GetSelectedLicenseApplicationID());
            frmIssueLicense.ShowDialog();
            _LoadInfo();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DLAppID = _GetSelectedLicenseApplicationID();

            frmDriverLicenseInfo frm = new frmDriverLicenseInfo(clsLocalDrivingLicenseApplication.GetIssuedLicenseID(DLAppID));
            frm.ShowDialog();

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication Application = clsLocalDrivingLicenseApplication.Find(_GetSelectedLicenseApplicationID());
            
            frmDriverLicenseHistory frm = new frmDriverLicenseHistory(Application.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseAppInfo frm = new frmLocalDrivingLicenseAppInfo(_GetSelectedLicenseApplicationID());
            frm.ShowDialog();
        }
    }
}
