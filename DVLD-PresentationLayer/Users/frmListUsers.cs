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
    public partial class frmListUsers : Form
    {
        public frmListUsers()
        {
            InitializeComponent();
            cbFilter.SelectedIndexChanged += FilterByChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        enum enFilterBy { None = 0, UserID, UserName, PersonID, FullName, IsActive };

        static DataTable _dtUsers;
        string FilterColumn = "";
        private void FilterByChanged(object sender, EventArgs e)
        {
            switch ((enFilterBy)cbFilter.SelectedIndex)
            {
                case enFilterBy.PersonID:
                    FilterColumn = "PersonID";
                    break;
                case enFilterBy.UserID:
                    FilterColumn = "UserID";
                    break;
                case enFilterBy.UserName:
                    FilterColumn = "UserName";
                    break;
                case enFilterBy.FullName:
                    FilterColumn = "FullName";
                    break;
                case enFilterBy.IsActive:
                    FilterColumn = "IsActive";
                    break;
                default:
                    FilterColumn = "";
                    break;
            }
        }
        private int _GetSelectedUserID()
        {
            int UserID;
            try
            {
                UserID = Convert.ToInt32(dgvUsers.CurrentRow.Cells[0].Value);

            }
            catch {
                UserID = -1;
            }

            return UserID;  

        } 
        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            _dtUsers = clsUser.ListAllUsers();

            dgvUsers.Columns.Clear();   
            
            dgvUsers.DataSource = _dtUsers;


            cbFilter.SelectedIndex = (int)enFilterBy.None;
            cbIsActive.SelectedIndex = 0;

            lblRecordsCount.Text = dgvUsers.RowCount.ToString();

            if(dgvUsers.RowCount == 0) return;

            dgvUsers.Columns[0].HeaderText = "User ID";
            dgvUsers.Columns[0].Width = 135;

            dgvUsers.Columns[1].HeaderText = "Person ID";
            dgvUsers.Columns[1].Width = 135;

            dgvUsers.Columns[2].HeaderText = "Full Name";
            dgvUsers.Columns[2].Width = 400;
            dgvUsers.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvUsers.Columns[3].HeaderText = "User Name";
            dgvUsers.Columns[3].Width = 200;

            dgvUsers.Columns[4].HeaderText = "Is Active";
            dgvUsers.Columns[4].Width = 135;

        }
        public void _RefreshUsers()
        {
            frmManageUsers_Load(null, null);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditUser frmAddNewUser = new frmAddEditUser();
            frmAddNewUser.DataBack += _RefreshUsers;
            frmAddNewUser.ShowDialog();
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible =
                cbFilter.SelectedIndex != (int)enFilterBy.None && cbFilter.SelectedIndex != (int)enFilterBy.IsActive;

            cbIsActive.Visible =
                 cbFilter.SelectedIndex == (int)enFilterBy.IsActive;


        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtFilterValue.Text) || FilterColumn == "")
            {
                _dtUsers.DefaultView.RowFilter = "";
                return;
            }

            if(FilterColumn == "PersonID" || FilterColumn == "UserID")
                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, int.Parse(txtFilterValue.Text.Trim()));

            else
                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] like '%{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            return;

        }
        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterValue = "";

            if (cbIsActive.SelectedIndex == 0)
                FilterValue = ""; // all

            else if (cbIsActive.SelectedIndex == 1)
                FilterValue = "1"; // active

            else FilterValue = "0"; // not active


         
            if (FilterValue != "")

                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue); 

            else _dtUsers.DefaultView.RowFilter = "";
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.SelectedIndex == (int)enFilterBy.PersonID || cbFilter.SelectedIndex == (int)enFilterBy.UserID)
            {
                e.Handled = !clsValidation.IsValidInteger(sender, e);
            }
        }
        private void msDelete_Click(object sender, EventArgs e)
        {
            int UserID = _GetSelectedUserID();

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
        private void msEdit_Click(object sender, EventArgs e)
        {
           
            frmAddEditUser frmAddNewUser = new frmAddEditUser(_GetSelectedUserID());
            frmAddNewUser.DataBack += _RefreshUsers;
            frmAddNewUser.ShowDialog();

        }
        private void msShowDetails_Click(object sender, EventArgs e)
        {
            
            frmUserInfo frmUserInfo = new frmUserInfo(_GetSelectedUserID());
            frmUserInfo.ShowDialog();
        }
        private void msChangePassword_Click(object sender, EventArgs e)
        {
            frmChangePassword frmChangePassword = new frmChangePassword(_GetSelectedUserID());
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
        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmUserInfo frmUserInfo = new frmUserInfo(_GetSelectedUserID());
            frmUserInfo.ShowDialog();
        }

    }
}
