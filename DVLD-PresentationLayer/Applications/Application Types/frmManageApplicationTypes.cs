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
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private int _GetSelectedApplicationType()
        {
            ///MessageBox.Show(dgvApplicationTypes.SelectedCells[0].ToString());
            int SelectedID;
            try
            {
                SelectedID = int.Parse(dgvApplicationTypes.SelectedCells[0].Value.ToString());
                return SelectedID;

            }catch { return -1; }

        }
        private void _FillGridWithApplicationTypes()
        {
            DataTable _dtApplicationTypes = clsApplicationType.ListAllApplicationTypes();
            dgvApplicationTypes.Rows.Clear();

            foreach (DataRow row in _dtApplicationTypes.Rows)
            {
                dgvApplicationTypes.Rows.Add(row["ApplicationTypeID"], row["ApplicationTypeTitle"], row["ApplicationFees"]);
            }
            //dgvApplicationTypes.DataSource = _dtApplicationTypes;

            lblRecordsCount.Text = dgvApplicationTypes.RowCount.ToString();

            //dgvApplicationTypes.ClearSelection();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _FillGridWithApplicationTypes();
        }

        private void dgvApplicationTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _RefreshApplicationTypes(bool IsUpdated)
        {
            if (IsUpdated)
            {
                _FillGridWithApplicationTypes();
            }return;
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplicationTypeID = _GetSelectedApplicationType();

            if (ApplicationTypeID == -1) { 
                MessageBox.Show("Please select Application Type first","Invalid",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }


            frmUpdateApplicationType frmUpdateApplicationType = new frmUpdateApplicationType(ApplicationTypeID);
            //Delegate subscribe code.......
            //..
            frmUpdateApplicationType.DataBack += _RefreshApplicationTypes;

            frmUpdateApplicationType.ShowDialog();
        }
    }
}
