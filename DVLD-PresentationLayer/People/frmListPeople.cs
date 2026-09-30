using DVLD.Common_Classes;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People
{
    public partial class frmListPeople : Form
    {
        public frmListPeople()
        {
            InitializeComponent();
            
        }
        enum enFilterBY
        {
            None = 0, PersonID, NationalNo, FirstName, SecondName,
            ThirdName, LastName, Nationality, Gender, Phone, Email
        }

        DataTable _dtPeople;
        private int _PageNumber = 1; //default = 1
        private int _RowsPerPage = 12;

       
        async private void frmManagePeople_Load(object sender, EventArgs e)
        {

            cbFilter.SelectedIndex = 0;
            txtFilterText.Visible = false;

            DataTable tempTable = await  clsPerson.GetPeopleAsync(_PageNumber, _RowsPerPage);

            _dtPeople = tempTable.DefaultView
                        .ToTable(false, "PersonID", "NationalNo", "FirstName", "SecondName",
                                    "ThirdName", "LastName", "Gender", "DateOfBirth",
                                    "Phone", "Email", "CountryName");


            dgvPeople.DataSource = _dtPeople;
            lblRecords.Text = dgvPeople.RowCount.ToString();
            //_FetchNextPeople(1);

            if (_dtPeople.Rows.Count == 0)
            {
                return;
            }

            dgvPeople.Columns[0].HeaderText = "Person ID";
            dgvPeople.Columns[0].Width = 100;

            dgvPeople.Columns[1].HeaderText = "National No.";
            dgvPeople.Columns[1].Width = 100;

            dgvPeople.Columns[2].HeaderText = "First Name";
            dgvPeople.Columns[2].Width = 120;

            dgvPeople.Columns[3].HeaderText = "Second Name";
            dgvPeople.Columns[3].Width = 135;

            dgvPeople.Columns[4].HeaderText = "Third Name";
            dgvPeople.Columns[4].Width = 135;

            dgvPeople.Columns[5].HeaderText = "Last Name";
            dgvPeople.Columns[5].Width = 140;
            dgvPeople.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvPeople.Columns[6].HeaderText = "Gender";
            dgvPeople.Columns[6].Width = 95;

            dgvPeople.Columns[7].HeaderText = "Date Of Birth";
            dgvPeople.Columns[7].Width = 180;

            dgvPeople.Columns[8].HeaderText = "Phone";
            dgvPeople.Columns[8].Width = 140;

            dgvPeople.Columns[9].HeaderText = "Email";
            dgvPeople.Columns[9].Width = 120;

            dgvPeople.Columns[10].HeaderText = "Nationality";
            dgvPeople.Columns[10].Width = 100;

            //dgvPeople.ClearSelection();
        }

        private int _GetSelectedPersonID()
        {
            //return (int)dgvPeople.CurrentRow.Cells[0].Value;
            return (int)dgvPeople.SelectedCells[0].Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterText.Visible = cbFilter.SelectedIndex != (int)enFilterBY.None;
            txtFilterText.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frmAddEditPerson = new frmAddEditPerson();
            //frmAddEditPerson.IsSaved += _RefreshPeopleList;
            frmAddEditPerson.ShowDialog();
           
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeople.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select person first!");
                return;
            }
            int PersonID = _GetSelectedPersonID();

            frmAddEditPerson editPerson = new frmAddEditPerson(PersonID);
            editPerson.IsSaved += _RefreshPeople; //refresh if data changed
            editPerson.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = _GetSelectedPersonID();
            DialogResult msgResult = MessageBox.Show($"Are you sure you want to delete this person [{PersonID}]",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (msgResult != DialogResult.Yes)
                return;
            
            if (!clsPerson.IsExist(PersonID))
            {
                MessageBox.Show("Failed, this person does not exist!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsPerson.Delete(PersonID))
            {

               // _RefreshPeopleList();
                MessageBox.Show("Person deleted successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshPeople();
                return;
            }

            //referential integrity.
            MessageBox.Show("Failed to this person because he has a related data in the system!",
                "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            

        }
        private void _ShowPersonDetails()
        {
            int PersonID = _GetSelectedPersonID();
            if (PersonID <= 0)
                MessageBox.Show("Please select person first.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (!clsPerson.IsExist(PersonID))
            {
                MessageBox.Show("This person does not exist, choose another one.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Information);
               return ;
            }

            frmShowPersonInfo frmPersonDetails = new frmShowPersonInfo(PersonID);
            frmPersonDetails.ShowDialog();

        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowPersonDetails();
        }

        private void dgvPeople_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            _ShowPersonDetails();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPerson AddNewPerson = new frmAddEditPerson();
           // AddNewPerson.IsSaved += _RefreshPeopleList;
            AddNewPerson.ShowDialog();            
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The feature is not implemented yet.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The feature is not implemented yet.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void txtFilterText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.SelectedIndex == (short)enFilterBY.PersonID)
                e.Handled = !clsValidation.IsValidInteger(sender, e);
        }

        private void txtFilterText_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtFilterText.Text))
            {
                _dtPeople.DefaultView.RowFilter = "";
                return;
            }

            string FilterColumn = "";

            switch ((enFilterBY)cbFilter.SelectedIndex) {
                case enFilterBY.PersonID:
                    FilterColumn = "PersonID";
                    break;
                case enFilterBY.NationalNo:
                    FilterColumn = "NationalNo";
                    break;
                case enFilterBY.FirstName:
                    FilterColumn = "FirstName";
                    break;
                case enFilterBY.SecondName:
                    FilterColumn = "SecondName";
                    break;
                case enFilterBY.ThirdName:
                    FilterColumn = "ThirdName";
                    break;
                case enFilterBY.LastName:
                    FilterColumn = "LastName";
                    break;
                case enFilterBY.Gender:
                    FilterColumn = "Gender";
                    break;
                case enFilterBY.Phone:
                    FilterColumn = "Phone";
                    break;
                case enFilterBY.Email:
                    FilterColumn = "Email";
                    break;
                case enFilterBY.Nationality:
                    FilterColumn = "Nationality";
                    break;
                case enFilterBY.None:
                    FilterColumn = "None";
                    break;
            }

            if (cbFilter.SelectedIndex == (int)enFilterBY.PersonID) 

                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, int.Parse(txtFilterText.Text.Trim()));
            
            else            

                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", FilterColumn, txtFilterText.Text.Trim());
            

        }

        private void msManagePeople_Opening(object sender, CancelEventArgs e)
        {
            if (dgvPeople.SelectedRows.Count == 0)
                msManagePeople.Enabled = false;       
            else
                msManagePeople.Enabled = true;
        }

        private int _LastScrollValue = 0;
        private int _PrevRowsCount = 0;
        async private void _RefreshPeople(bool doRefresh = true)
        {
            if (!doRefresh) return;

            _PageNumber = 1;
            _LastScrollValue = 0;
            _PrevRowsCount = 0;

            DataTable tempTable = await clsPerson.GetPeopleAsync(_PageNumber, _RowsPerPage);

            _dtPeople = tempTable.DefaultView
                        .ToTable(false, "PersonID", "NationalNo", "FirstName", "SecondName",
                                    "ThirdName", "LastName", "Gender", "DateOfBirth",
                                    "Phone", "Email", "CountryName");
            dgvPeople.DataSource = _dtPeople;
            dgvPeople.Update();
            lblRecords.Text = dgvPeople.RowCount.ToString();
        }

        async private void _FetchNextPeople(int nextPage)
        {
            progressIndictior.Visible = true;
            progressIndictior.Start();

            
            DataTable dtNextPeople = await clsPerson.GetPeopleAsync(nextPage, _RowsPerPage);
            await Task.Delay(1000);// async -- wait for 1 second then continue

            progressIndictior.Visible = false;
            progressIndictior.Stop();                       
            

            //DataTable dtNextPeople = clsPerson.GetPeopleAsync(nextPage, _RowsPerPage).GetAwaiter().GetResult();
            //DataTable dtNextPeople = clsPerson.GetPeopleAsync(nextPage, _RowsPerPage).Result;
            //

            _PrevRowsCount = dgvPeople.RowCount;

            _dtPeople.Merge(dtNextPeople.DefaultView
                    .ToTable(false, "PersonID", "NationalNo", "FirstName", "SecondName",
                                "ThirdName", "LastName", "Gender", "DateOfBirth",
                                "Phone", "Email", "CountryName"));

            if(_PageNumber > 2 && _PrevRowsCount > 0)
            {
                dgvPeople.FirstDisplayedScrollingRowIndex =  (_PrevRowsCount - _RowsPerPage);
            }
            else
            {
                dgvPeople.FirstDisplayedScrollingRowIndex = 1;
            }

            lblRecords.Text = dgvPeople.RowCount.ToString();           

        }

        private void dgvPeople_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > _LastScrollValue &&
                (e.OldValue == 0 || e.NewValue % _RowsPerPage == 0))
            {
                _LastScrollValue = e.NewValue;

                //_PageNumber + 1;
                _FetchNextPeople(++_PageNumber);                
            }            
        }
    }
}
 
