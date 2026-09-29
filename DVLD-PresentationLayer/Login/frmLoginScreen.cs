using DVLD_BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DVLD.Common_Classes;
namespace DVLD
{
    public partial class frmLoginScreen : Form
    {
        public frmLoginScreen()
        {
            InitializeComponent();
        }
        clsUser _User;
        //string _SavedLoginPath = @"C:\DVLD\SavedLogin.txt";

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //check login information right and user is active. 
            _User = clsUser.Find(txtUserName.Text.Trim(), clsUtil.ComputeHash(txtPassword.Text.Trim()));

            if (_User == null)
            {

                MessageBox.Show("Invalid UserName or Password","Invalid",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (!_User.IsActive)
            {
                MessageBox.Show("This user is not active, please contact you admin.", "Invalid",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            

            //Save login information in text file
            if (chkRememberMe.Checked)
            {

                //Save login info to file
                //File.WriteAllLines(_SavedLoginPath, new string[] { txtUserName.Text.ToString() , txtPassword.Text.ToString()});

                //Save login info to registry
                clsUtil.SaveLoginInformationToRegistry(txtUserName.Text.Trim(), txtPassword.Text.Trim());


            }
            else
            {
                try
                {
                    //File.Delete(_SavedLoginPath);
                    clsUtil.DeleteLoginInfoFromRegistry();

                }catch { }
            }

                   
            clsGlobalSettings.LoggedInUserName = _User.UserName;  
            //clsGlobalSettings.Password = txtPassword.Text;
            clsGlobalSettings.LoggedInUserID = _User.UserID;

            //open main form screen
            frmMain frm = new frmMain();
            frm.DataBack += ShowLoginScreen;
            this.Hide();
            this.ShowInTaskbar = false;
            frm.ShowDialog();
        }

        public void ShowLoginScreen(bool EndProgram)
        {
            //if frmMain closed then delegate return true then close login screen
            if (EndProgram)
            {
                this.Close();
                return;
            }

            //else -- show login screen again
            this.Show();
            this.ShowInTaskbar = true;
            if(!chkRememberMe.Checked)
            {
                txtUserName.Text = "";
                txtPassword.Text = "";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {

            //Read login info fom text file.
            //if (File.Exists(_SavedLoginPath)) {

            //    string[] lines = File.ReadAllLines(_SavedLoginPath);

            //    try {
            //        //if text file was empty for any reason, it will throw exception here.
            //        txtUserName.Text = lines[0];
            //        txtPassword.Text = lines[1];

            //        //if exception not be thrown then Check remember me again.
            //        chkRememberMe.Checked = true;   
            //    }catch { 
            //        chkRememberMe.Checked= false;
            //    }

            //}

            string userName = "";
            string password = "";

            if(clsUtil.LoadLoginInformationFromRegistry(ref userName,ref password))
            {
                chkRememberMe.Checked = true;
                txtUserName.Text = userName;
                txtPassword.Text = password;
            }else
                chkRememberMe.Checked = false;



        }
    }
}
