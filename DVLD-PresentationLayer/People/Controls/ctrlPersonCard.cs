using DVLD.Properties;
using DVLD_BusinessLayer;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DVLD.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        public ctrlPersonCard()
        {
            InitializeComponent();
        }


        int _PersonID = -1;

        clsPerson _Person;        
        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }

        public int PersonID
        {
            get { return _PersonID; }
        }
        
        private void _SetPersonImage()
        {

            if (_Person.ImagePath != "")
                if (File.Exists(_Person.ImagePath))
                    ppPersonImage.ImageLocation = _Person.ImagePath;
                else MessageBox.Show("Could`t Find this Image [" + _Person.ImagePath + "] .", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ppPersonImage.Image = (_Person.Gender == clsPerson.enGender.Male ? Resources.man : Resources.woman);

        }

        public void _ResetDefaultValues()
        {
            lblPersonID.Text = "N/A";
            lblNationalNo.Text = "[???]";
            lblName.Text = "[???]";
            lblGender.Text = "[???]";
            lblCountry.Text = "[???]";
            lblEmail.Text = "[???]";
            lblPhone.Text = "[???]";
            lblAddress.Text = "[???]";
            lblDateOfBirth.Text = "[???]";
            ppPersonImage.Image = Resources.man;
            _Person = null;

        }
    

        private void _FillPersonInfo()
        {

            _PersonID = _Person.PersonID; 

            lblPersonID.Text = _Person.PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblName.Text = _Person.FullName;
            lblPhone.Text = _Person.Phone;
            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblGender.Text = (_Person.Gender == clsPerson.enGender.Male ? "Male" : "Female");
            lblCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;
            _SetPersonImage();
        }
        public void LoadPersonInfo(int personID)
        {
            _PersonID = -1;
            _Person = clsPerson.Find(personID);
            if( _Person == null)
            {
                _ResetDefaultValues();
                MessageBox.Show($"This person with ID {_Person.PersonID.ToString()} is not found", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }

        public void LoadPersonInfo( string NationalNo)
        {
            _PersonID = -1;

            _Person = clsPerson.Find(NationalNo);
            if (_Person == null)
            {
                _ResetDefaultValues();
                MessageBox.Show($"This person is not found", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }

        private void _RefreshPersonInfo(bool IsChanged = true)
        {
            if (IsChanged)
            {
                LoadPersonInfo(_PersonID);
            }
        }
        private void llEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(_Person == null)
            {
                return; 
            }
            frmAddEditPerson frm = new frmAddEditPerson(_Person.PersonID);
            frm.IsSaved += _RefreshPersonInfo;
            frm.ShowDialog();
        }

    }
}
