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

namespace DVLD.Tests
{
    public partial class frmUpdateTestType : Form
    {
        clsTestType.enTestType _TestTypeID = clsTestType.enTestType.Vision;
        clsTestType _TestType;
        bool _IsUpdated = false;

        public delegate void DataBackEventHandler(bool IsUpdated);

        // 2. Declare event based on delegate
        public event DataBackEventHandler DataBack;
        public frmUpdateTestType(int TestTypeID)
        {
            _TestTypeID = (clsTestType.enTestType)TestTypeID;
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DataBack?.Invoke(_IsUpdated);
            this.Close();
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            _TestType = clsTestType.Find(_TestTypeID);
            lblTestTypeID.Text = _TestType.TestTypeID.ToString();
            txtTestTypeTitle.Text = _TestType.TestTypeTitle.ToString();
            txtTestTypeDescription.Text = _TestType.TestTypeDescription.ToString();
            txtTestTypeFees.Text = _TestType.TestTypeFees.ToString();

        }

        private bool _CheckIsTestDataChanged()
        {
            return (txtTestTypeTitle.Text.ToString() != _TestType.TestTypeTitle ||
                txtTestTypeFees.Text.ToString() != _TestType.TestTypeFees.ToString() ||
                txtTestTypeDescription.Text.ToString() != _TestType.TestTypeDescription);


        }

        private void _FillObjectWithTestTypeData()
        {
            _TestType.TestTypeTitle = txtTestTypeTitle.Text.ToString();
            _TestType.TestTypeDescription = txtTestTypeDescription.Text.ToString();
            _TestType.TestTypeFees = Convert.ToDouble(txtTestTypeFees.Text.ToString());
        }
        private void btnSave_Click(object sender, EventArgs e)
        {


            if (!_CheckIsTestDataChanged())
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

            DialogResult msgResult = MessageBox.Show("Are you sure you want to update this Test type?", "Confirm",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (msgResult == DialogResult.OK)
            {
                _FillObjectWithTestTypeData();

                if (_TestType.Save())
                {
                    MessageBox.Show("Test Type updated successfully.", "Done",
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

        private void frmUpdateTestType_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataBack?.Invoke(_IsUpdated);
        }

        private void txtTestTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestTypeTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTypeTitle, "Test Title should not be empty!");
                return;
            }

            errorProvider1.SetError(txtTestTypeTitle, null);
        }

        private void txtTestTypeFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestTypeFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTypeFees, "Test Title should not be empty!");
                return;
            }

            errorProvider1.SetError(txtTestTypeFees, null);
        }

        private void txtTestTypeFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !clsValidation.IsValidInteger(sender, e);
        }

        private void txtTestTypeDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestTypeDescription.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTypeDescription, "Test Description should not be empty!");
                return;
            }

            errorProvider1.SetError(txtTestTypeDescription, null);
        }
    }
}
