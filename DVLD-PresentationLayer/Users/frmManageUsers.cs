using DVLD.Common_Classes;
using DVLD.People.Controls;
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

namespace DVLD.Users
{
    public partial class frmManageUsers : Form
    {
        public frmManageUsers()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        enum enFilterBy { None = 0, UserID, UserName, PersonID, FullName, IsActive };
        enum enIsActive { All = 0, Yes, No };

        DataTable _dtUsers;

        private void _FillGridWithUsers(DataTable dtUsers)
        {
            dgvUsers.Rows.Clear();
            foreach (DataRow row in dtUsers.Rows)
            {
                
                dgvUsers.Rows.Add
                    (
                        row["UserID"],
                        row["PersonID"],
                        row["FullName"],
                        row["UserName"],
                        row["IsActive"]
                    );
            }
            lblRecordsCount.Text = dtUsers.Rows.Count.ToString();

        }
        private void _LoadUsers()
        {
            
            _dtUsers = clsUser.ListAllUsers();
            _FillGridWithUsers(_dtUsers);
            //dgvUsers.ClearSelection();
 
            cbFilter.SelectedIndex = (int)enFilterBy.None;
            cbIsActive.SelectedIndex = (int)enIsActive.All;

        }

        private int _GetSelectedUserID()
        {
            int UserID;
            try
            {
                UserID = Convert.ToInt32(dgvUsers.SelectedCells[0].Value);

            }
            catch {
                UserID = -1;
            }

            return UserID;  

        } 
        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            _LoadUsers();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditUser frmAddNewUser = new frmAddEditUser();
            frmAddNewUser.DataBack += _RefreshUsers;
            frmAddNewUser.ShowDialog();
        }

        private void msEdit_Click(object sender, EventArgs e)
        {
            //dgvUsers.ClearSelection();
            int UserID = _GetSelectedUserID();

            //MessageBox.Show($"Selected UserID = {UserID}");

            if (UserID == -1) { 
                MessageBox.Show("Please Select User First!","Invalid",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            frmAddEditUser frmAddNewUser = new frmAddEditUser(UserID);
            frmAddNewUser.DataBack += _RefreshUsers;
            frmAddNewUser.ShowDialog();

        }
        public void _RefreshUsers()
        {
            _LoadUsers();
        }

        private void msDelete_Click(object sender, EventArgs e)
        {
            int UserID = _GetSelectedUserID();

            if (UserID == -1)
            {
                MessageBox.Show("Please Select User First!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult msgResult = MessageBox.Show($"Are you sure you want to delete user ID = {UserID} ?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (msgResult == DialogResult.Yes)
            {

                if (clsUser.Delete(UserID))
                {
                    MessageBox.Show($"User with ID = {UserID} is delete successfully.", "Done",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshUsers();
                }
                else
                {
                    MessageBox.Show($"Failed to delete this User with ID = {UserID}, because he related to information in the system ", "Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible =
                cbFilter.SelectedIndex != (int)enFilterBy.None && cbFilter.SelectedIndex != (int)enFilterBy.IsActive;

            cbIsActive.Visible =
                 cbFilter.SelectedIndex == (int)enFilterBy.IsActive;

            _FillGridWithUsers(_dtUsers);
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            DataView dvUsers = _dtUsers.DefaultView;
            string FilterValue =  txtFilterValue.Text.ToString().Trim();
            if (FilterValue == "")
            {
                _FillGridWithUsers(_dtUsers);
                return;
            }

            if(cbFilter.SelectedIndex == (int)enFilterBy.UserID)
            {
                if(int.TryParse(FilterValue.ToString(), out int InsertedID))
                {
                    dvUsers.RowFilter = $"UserID = {InsertedID}";
                    _FillGridWithUsers(dvUsers.ToTable());
                    return;

                }

            }

            if(cbFilter.SelectedIndex == (int)enFilterBy.PersonID)
            {
                if (int.TryParse(FilterValue, out int InsertedID))
                {
                    dvUsers.RowFilter = $"PersonID = {InsertedID}";
                    _FillGridWithUsers(dvUsers.ToTable());
                    return;

                }

                return;
            }

            if (cbFilter.SelectedIndex == (int)enFilterBy.UserName)
            {
                
                dvUsers.RowFilter = $"UserName LIKE '%{FilterValue}%'";
                _FillGridWithUsers(dvUsers.ToTable());
                return;
                
            }
            if (cbFilter.SelectedIndex == (int)enFilterBy.FullName)
            {
                dvUsers.RowFilter = $"FullName LIKE '%{FilterValue}%'";
                _FillGridWithUsers(dvUsers.ToTable());
                return;
            }


        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.SelectedIndex == (int)enFilterBy.PersonID || cbFilter.SelectedIndex == (int)enFilterBy.UserID)
            {
                e.Handled = !clsValidation.IsValidInteger(sender, e);
            }
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {

            if(cbIsActive.SelectedIndex == (int)enIsActive.All)
            {
                _FillGridWithUsers(_dtUsers);
                return;
            }

            DataView dvUsers = _dtUsers.DefaultView;

            if (cbIsActive.SelectedIndex == (int)enIsActive.Yes)
            {
                dvUsers.RowFilter = "IsActive = 1";
            }
            else
            {
                dvUsers.RowFilter = "IsActive = 0";
            }
            
                _FillGridWithUsers(dvUsers.ToTable());
        }

        private void msShowDetails_Click(object sender, EventArgs e)
        {
            int UserID = _GetSelectedUserID();
            
            frmUserInfo frmUserInfo = new frmUserInfo(UserID);
            frmUserInfo.ShowDialog();
        }

        private void msChangePassword_Click(object sender, EventArgs e)
        {
            int UserID = _GetSelectedUserID() ;
            frmChangePassword frmChangePassword = new frmChangePassword(UserID);
            frmChangePassword.ShowDialog();
        }

        private void msSendEmail_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The feature is not implemented yet.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void msPhoneCall_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The feature is not implemented yet.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
