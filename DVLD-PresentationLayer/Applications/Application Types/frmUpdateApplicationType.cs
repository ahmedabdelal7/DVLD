using DVLD.Common_Classes;
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
    public partial class frmUpdateApplicationType : Form
    {
        int _ApplicationTypeID = -1;
        clsApplicationType _ApplicationType;
        bool _IsUpdated = false;

        public delegate void DataBackEventHandler(bool IsUpdated);

        // 2. Declare event based on delegate
        public event DataBackEventHandler DataBack;
        public frmUpdateApplicationType(int ApplicationTypeID)
        {
            _ApplicationTypeID = ApplicationTypeID;
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DataBack?.Invoke(_IsUpdated);
            this.Close();
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);
            lblApplicationTypeID.Text = _ApplicationType.ApplicationTypeID.ToString();
            txtApplicationTypeTitle.Text = _ApplicationType.ApplicationTypeTitle.ToString();
            txtApplicationFees.Text = _ApplicationType.ApplicationFees.ToString();

        }

        private bool _CheckIsApplicationDataChanged()
        {
            return (txtApplicationTypeTitle.Text.ToString() != _ApplicationType.ApplicationTypeTitle ||
                txtApplicationFees.Text.ToString() != _ApplicationType.ApplicationFees.ToString());

            
        }

        private void _FillObjectWithApplicationTypeData()
        {
            _ApplicationType.ApplicationTypeTitle = txtApplicationTypeTitle.Text.ToString();
            _ApplicationType.ApplicationFees = Convert.ToDouble(txtApplicationFees.Text.ToString());
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            

            if(!_CheckIsApplicationDataChanged())
            {
                MessageBox.Show("No Data has changed to save!", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!this.ValidateChildren())
            {
                MessageBox.Show("There is some fields not handled right!", "Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult msgResult =  MessageBox.Show("Are you sure you want to update this application type?","Confirm",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (msgResult == DialogResult.OK) {
                _FillObjectWithApplicationTypeData();

                if (_ApplicationType.Save())
                {
                    MessageBox.Show("Application Type updated successfully.", "Done",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _IsUpdated = true;
                }
                else
                {
                    MessageBox.Show("Failed to updated!", "Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
              
            };
        }

        private void frmUpdateApplicationType_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataBack?.Invoke(_IsUpdated);
        }

        private void txtApplicationTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtApplicationTypeTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeTitle, "Application Title should not be empty!");
                return;
            }

            errorProvider1.SetError(txtApplicationTypeTitle, null);
        }

        private void txtApplicationFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtApplicationFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationFees, "Application Title should not be empty!");
                return;
            }

            errorProvider1.SetError(txtApplicationFees, null);
        }

        private void txtApplicationFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !clsValidation.IsValidInteger(sender, e);
        }
    }
}
