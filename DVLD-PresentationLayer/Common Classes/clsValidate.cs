using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data;
using System.ComponentModel;

namespace DVLD.Common_Classes
{
    public static class clsValidation
    {

        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
        public static bool IsValidInteger(object sender, KeyPressEventArgs e)
        {

            TextBox txtFilterText = (TextBox)sender;

            //if the text is not digit and not Key control (Enter BackSpace ESC) then ignore it.
            return char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar);
            
        }
        
    }
}
