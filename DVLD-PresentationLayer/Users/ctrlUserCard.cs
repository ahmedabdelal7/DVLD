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

namespace DVLD.Users.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        public ctrlUserCard()
        {
            InitializeComponent();
        }
        clsUser _User;
        int _UserID = -1;
        public int UserID => _UserID;


        public void LoadUserInfo(int userID)
        {
            _User = clsUser.Find(userID);

            _UserID = userID;

            if (_User == null)
            {
                MessageBox.Show($"User does not exist with ID: {userID}", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetDefaultValues();
                return;
            }

            _FillUserInfo();
            
        }
        private void _FillUserInfo()
        {
            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);

            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName.ToString();
            lblIsActive.Text = _User.IsActive == false ? "No" : "Yes";

        }

        public void ResetDefaultValues()
        {
            lblUserID.Text = "";
            lblUserName.Text = "";
            lblIsActive.Text = "-";
            _UserID = -1;

            ctrlPersonCard1.ResetDefaultValues();
        }




    }
}
