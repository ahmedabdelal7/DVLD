using DVLD.Common_Classes;
using DVLD.Properties;
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

namespace DVLD.Licenses
{
    public partial class frmDriverLicenseInfo : Form
    {
        int _LicenseID = -1;
        public frmDriverLicenseInfo(int LicenseID)
        {
            _LicenseID = LicenseID;
            InitializeComponent();
        }

        private void frmDriverLicenseCard_Load(object sender, EventArgs e)
        {
            ctrlLicenseDetails1.LoadLicenseInfo(_LicenseID);
               

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
