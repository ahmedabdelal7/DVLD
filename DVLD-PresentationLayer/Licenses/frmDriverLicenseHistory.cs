using DVLD_BusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD.Licenses
{
    public partial class frmDriverLicenseHistory : Form
    {
        int _PersonID = -1;
        public frmDriverLicenseHistory(int PersonID)
        {
            _PersonID = PersonID;
            InitializeComponent();
        }

        private void frmDriverLicenseHistory_Load(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            clsDriver driver = clsDriver.FindByPersonID(_PersonID);

            ctrlDriverLicenses1._LoadDriverLicenses(driver.DriverID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
