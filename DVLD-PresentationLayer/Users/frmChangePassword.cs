using DVLD.Common_Classes;
using DVLD_BusinessLayer;
using Microsoft.VisualBasic.ApplicationServices;
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
    public partial class frmChangePassword : Form
    {
        int _UserID = -1;
        clsUser _User;
        public frmChangePassword(int UserID)
        {
            _UserID = UserID;
            InitializeComponent();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (!clsUser.IsExist(_UserID))
            {
                MessageBox.Show($"User does not exist with ID: {_UserID}", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            ctrlUserInformation1.LoadUserInfo(_UserID);
            _User = clsUser.Find(_UserID);
        }
        private void _ResetDefaultValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";

        }
        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text)) { 
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current password should not blank!");
                return;
            } 

            //using Compute hash to compute hashed password
            //
            if(clsUtil.ComputeHash(txtCurrentPassword.Text.Trim()) !=  _User.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Invalid current password!");
                return;
            }
            errorProvider1.SetError(txtCurrentPassword, null);

        }
        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "Current password should not be blank!");
                return;
            }

            if(clsUtil.ComputeHash(txtNewPassword.Text.Trim() )== _User.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "New password should not be same as current password");
                return;
            }    
            errorProvider1.SetError(txtNewPassword, null);
        }
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Confirm password should not be blank!");
                return;
            }

            if(txtNewPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Confirm password does not match new password!");
                return;

            }
            errorProvider1.SetError(txtConfirmPassword, null);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields not filled right!","Invalid",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _User.Password = clsUtil.ComputeHash(txtNewPassword.Text.Trim());
            if (_User.Save())
            {
                MessageBox.Show("Password changed successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ResetDefaultValues();
                return;
            }

            MessageBox.Show("Failed to update password", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);            

        }
    }
}
