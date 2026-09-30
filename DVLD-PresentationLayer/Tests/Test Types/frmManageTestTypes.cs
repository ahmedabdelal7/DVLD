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
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }
        private int _GetSelectedTestType()
        {
            ///MessageBox.Show(dgvTestTypes.SelectedCells[0].ToString());
            int SelectedID;
            try
            {
                SelectedID = int.Parse(dgvTestTypes.SelectedCells[0].Value.ToString());
                return SelectedID;

            }
            catch { return -1; }

        }
        private void _FillGridWithTestTypes()
        {
            DataTable _dtTestTypes = clsTestType.ListAllTestTypes();
            dgvTestTypes.Rows.Clear();

            foreach (DataRow row in _dtTestTypes.Rows)
            {
                dgvTestTypes.Rows.Add(row["TestTypeID"], row["TestTypeTitle"], row["TestTypeDescription"], row["TestTypeFees"]);
            }
            //dgvTestTypes.DataSource = _dtTestTypes;

            lblRecordsCount.Text = dgvTestTypes.RowCount.ToString();

            //dgvTestTypes.ClearSelection();
        }
        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            _FillGridWithTestTypes();
        }


        private void _RefreshTestTypes(bool IsUpdated)
        {
            if (IsUpdated)
            {
                _FillGridWithTestTypes();
            }
            return;
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestTypeID = _GetSelectedTestType();

            if (TestTypeID == -1)
            {
                MessageBox.Show("Please select Test Type first", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            frmUpdateTestType frmUpdateTestType = new frmUpdateTestType(TestTypeID);
            //Delegate subscribe code.......
            //..
            frmUpdateTestType.DataBack += _RefreshTestTypes;

            frmUpdateTestType.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
