using DVLD.Common_Classes;
using DVLD.Properties;
using DVLD_BusinessLayer;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DVLD.People
{
    public partial class frmAddEditPerson : Form
    {
        enum enMode { AddNew, Update }

        enMode _Mode = enMode.AddNew;
        int _PersonID = -1;
        clsPerson _Person;

        public delegate void DataBackEventHandler(int PersonID);

        // 2. Declare event based on delegate
        public event DataBackEventHandler DataBack;


        public delegate void IsSavedEventHandler(bool IsDataSaved);

        // 2. Declare event based on delegate
        public event IsSavedEventHandler IsSaved;
        public frmAddEditPerson()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }
        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
            _Mode = enMode.Update;
        }

        private void _FillCountriesInComboBox()
        {
            //Load all countries from database
            DataTable dt = clsCountry.ListAllCountries();
            cbCountries.DataSource = dt;

            cbCountries.DisplayMember = "CountryName";
            cbCountries.ValueMember = "CountryID";
        }
        private void _SetDateOfBirthSelectRange()
        {
            //Set Maximum date is date now - 18 year, to add user less than 18 years old  
            dateTimePicker1.MaxDate = DateTime.Today.AddYears(-18);
            //Set Minimum date date now - 100 year, to prevent adding user grater than 100 years old
            dateTimePicker1.MinDate = DateTime.Today.AddYears(-100);
        }

        private void _GenderCheckedChange(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ppPersonImage.ImageLocation))
            {
                ppPersonImage.Image = (rbMale.Checked == true ? Resources.man : Resources.woman);
            }

        }
        private void _ValidateTextBox(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrEmpty(textBox.Text.Trim()))
            {
                e.Cancel = true;
                
                errorProvider1.SetError(textBox, "This field should not be empty!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox, null);
            }
        }
        private void _ValidateEmail(object sender, CancelEventArgs e)
        {
            TextBox email = sender as TextBox;
            if (string.IsNullOrEmpty(email.Text) || clsValidation.IsValidEmail(email.Text))
            {
                e.Cancel = false;
                errorProvider1.SetError(email, null);
                return;
            }

            e.Cancel = true;
            
            errorProvider1.SetError(email, "Email address is not valid!");
            return;

        }
        private void _ValidateNationalNo(object sender, CancelEventArgs e)
        {
            
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                
                errorProvider1.SetError(txtNationalNo, "National Number should not be empty!");
                return;
            }

            if(_Mode == enMode.AddNew)
            {
                if (clsPerson.IsExist(txtNationalNo.Text.Trim()))
                {
                    e.Cancel = true;
                    
                    errorProvider1.SetError(txtNationalNo, "National Number is already exist, enter another one!");
                    return;
                }
            }
            else
            {

                if(clsPerson.IsExist(txtNationalNo.Text) && _Person.NationalNo != txtNationalNo.Text)
                {
                    e.Cancel = true;
                    

                    errorProvider1.SetError(txtNationalNo, "National Number is already exist to another user, enter another one!");
                    return;
                }
            }

            e.Cancel = false;
            errorProvider1.SetError(txtNationalNo, null);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
           //DataBack?.Invoke(_PersonID);

            this.Close();
        }

        private void _ResetDefaultValues()
        {
            _FillCountriesInComboBox();
            _SetDateOfBirthSelectRange();

            if (_Mode == enMode.AddNew) {
                _Person = new clsPerson();
                lblAddEditPerson.Text = "Add New Person";

            }
            else
            {
                lblAddEditPerson.Text = "Update Person";
            }

            lblPersonID.Text = "N/A";
            txtNationalNo.Text = "";
            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            rbMale.Checked = true;
            ppPersonImage.Image = Resources.man;
            ppPersonImage.ImageLocation = "";
            cbCountries.SelectedIndex = cbCountries.FindString("Egypt");

            llRemove.Visible = false;

        }
        private void _LoadData()
        {

            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("Person Does not exist, choose another person!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            _Person = clsPerson.Find(_PersonID);

            lblPersonID.Text = _Person.PersonID.ToString();
            txtFirstName.Text = _Person.FirstName.ToString();
            txtSecondName.Text = _Person.SecondName.ToString();
            txtThirdName.Text = _Person.ThirdName.ToString();
            txtLastName.Text = _Person.LastName.ToString();
            txtNationalNo.Text = _Person.NationalNo.ToString();
            dateTimePicker1.Value = _Person.DateOfBirth;
            txtEmail.Text = _Person.Email.ToString();
            txtPhone.Text = _Person.Phone.ToString();
            txtAddress.Text = _Person.Address.ToString();
            if(_Person.ImagePath != "")
            {
                ppPersonImage.ImageLocation = _Person.ImagePath;

            }
            cbCountries.SelectedValue = _Person.NationalityCountryID;

            if (_Person.Gender == clsPerson.enGender.Male)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            //hide / show remove link if there is no image
            llRemove.Visible = (ppPersonImage.ImageLocation == "" ? false : true);

        }
        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {

            _ResetDefaultValues();

            if(_Mode == enMode.Update)
            {
                _LoadData();
            }
            
        }

        private void _FillDataToPersonObject()
        {
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dateTimePicker1.Value;
            _Person.Gender = 
                (rbMale.Checked == true ? clsPerson.enGender.Male : clsPerson.enGender.Female);

            _Person.NationalityCountryID = (int)cbCountries.SelectedValue;
            if (ppPersonImage.ImageLocation != null)
                _Person.ImagePath = ppPersonImage.ImageLocation;
            else
                _Person.ImagePath = "";

            //_Person.NationalityCountryID = cbCountries.SelectedIndex;

            //_Person.ImagePath = ppPersonImage.ImageLocation;

        }
        //private void _HandelPersonImage()
        //{

        //    if (ppPersonImage.ImageLocation == _Person.ImagePath)
        //        return;
            
        //    string folderPath = @"C:\DVLD\People-Images\";

        //    if (_Person.ImagePath.Contains(folderPath) )
        //    {
        //        try
        //        {
        //            File.Delete(_Person.ImagePath.ToString());
                    
        //        }
        //        catch { }
        //    }

        //    if (ppPersonImage.ImageLocation == "")
        //    {
        //        _Person.ImagePath = "";
        //        return;
        //    }

        //    if (!Directory.Exists(folderPath))
        //        Directory.CreateDirectory(folderPath);

        //    string sourceImage = ppPersonImage.ImageLocation;
        //    string imageExtension = clsUtil.GetPathExtension(sourceImage);
        //    string destinationFileName = folderPath + Guid.NewGuid().ToString() + imageExtension;

        //    try
        //    {
        //        File.Copy(sourceImage, destinationFileName, true);
        //    }
        //    catch { }

        //    //Update person object to new to the new image path
        //    _Person.ImagePath = destinationFileName;

        //    //Update pictureBox to new image path, to be ready if an update happened without reload form.
        //    ppPersonImage.ImageLocation = destinationFileName;

        //}
        private bool _HandlePersonImage()
        {
            if(_Person.ImagePath != ppPersonImage.ImageLocation)
            {

                if (_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);

                    }catch { }


                }
                if (ppPersonImage.ImageLocation != null)
                {
                    string SourceImageLocation = ppPersonImage.ImageLocation;

                    if(!clsUtil.CopyImageToProjectImagesFolder(ref SourceImageLocation))
                        return false;

                    ppPersonImage.ImageLocation = SourceImageLocation;

                }

            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("cannot save, fill required fields or enter valid fields first!", "Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Handel Image
            //....


            if (!_HandlePersonImage())
                return;

            _FillDataToPersonObject();


            if (_Person.Save())
            {
                if(_Mode == enMode.AddNew)
                {
                    _Mode = enMode.Update;
                    _PersonID = _Person.PersonID;

                    MessageBox.Show($"Person added successfully with id [{_PersonID}]", "Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    lblAddEditPerson.Text = "Update Person";
                    lblPersonID.Text = _PersonID.ToString();

                }
                else
                {
                    MessageBox.Show($"Person Updated successfully.", "Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DataBack?.Invoke(_PersonID);
                IsSaved?.Invoke(true);

                return;

            }

            MessageBox.Show("Failed to save this person.", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);

        } 
        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.InitialDirectory = @"E:\";
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Select Person Image";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.webp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                ppPersonImage.ImageLocation  = openFileDialog1.FileName;
                llRemove.Visible = true;
            }
        }

        private void llRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ppPersonImage.ImageLocation = null;
            llRemove.Visible = false;
            ppPersonImage.Image = (rbMale.Checked == true ? Resources.man : Resources.woman);
        }

        private void frmAddEditPerson_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}
