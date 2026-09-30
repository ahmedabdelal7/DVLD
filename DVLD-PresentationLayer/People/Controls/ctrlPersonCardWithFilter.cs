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


namespace DVLD.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        //Event on person selected.
        public event Action<int> OnPersonSelected;

        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get => _ShowAddPerson;
            set 
            {
                _ShowAddPerson=value;
                btnAdd.Visible = _ShowAddPerson;
            } 
        }

        private bool _FilterEnabled = false;
        public bool FilterEnabled
        {
            get => _FilterEnabled;
            set
            {
                _FilterEnabled=value;
                gbFilter.Enabled = _FilterEnabled;
            }
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }


        //int _PersonID = -1;
        public int PersonID => ctrlPersonCard1.PersonID;

        public clsPerson SelectedPerson =>
            ctrlPersonCard1.SelectedPersonInfo;

        //----------------------Methods---------------------------//

        private void FindNow()
        {
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text));

                    break;

                case "National No.":
                    ctrlPersonCard1.LoadPersonInfo(txtFilterValue.Text);
                    break;

                default:
                    break;
            }

            if (OnPersonSelected != null && FilterEnabled)
                // Raise the event with a parameter
                OnPersonSelected.Invoke(PersonID);
                //OnPersonSelected(ctrlPersonCard1.PersonID);
        }

        public void LoadPersonInfo(int PersonID)
        {
            //change filter to PersonID and change the txtFilterValue = PersonID.
            cbFilterBy.SelectedIndex = 1;//personID
            txtFilterValue.Text = PersonID.ToString();
            FindNow();
        }

        private void txtSearch_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is Enter (character code 13)
            if (e.KeyChar == (char)13)
            {

                btnFind.PerformClick();
            }

            //this will allow only digits if person id is selected
            if (cbFilterBy.Text == "Person ID")
                e.Handled = !clsValidation.IsValidInteger(sender, e);
                //e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            //if handled = true => it prevent you from write this character.
         }

        private void txtSearch_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtFilterValue.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterValue, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtFilterValue, null);
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            txtFilterValue.Focus(); 
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fields are not validated!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FindNow();

        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Focus(); 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frmAddNewPerson = new frmAddEditPerson();
            frmAddNewPerson.DataBack += DataBack;
            frmAddNewPerson.ShowDialog();
     
        }

        private void DataBack(int personID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = personID.ToString();
            //ctrlPersonCard1.LoadPersonInfo(personID);
            btnFind.PerformClick();
        }

        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }
    }
}
