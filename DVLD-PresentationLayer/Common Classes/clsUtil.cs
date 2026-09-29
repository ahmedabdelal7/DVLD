using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DVLD.Common_Classes
{
    public static class clsUtil
    {
        public static string GetPathExtension(string path)
        {
            string Ext = "";

            int extIndex = -1;
            try
            {
                if (path.Contains('.'))
                    extIndex = path.LastIndexOf(".");
                else return "";

            }
            catch (Exception) {
                return "";
            }

            Ext = path.Remove(0,extIndex);

            return Ext;
        }

        public static string CustomShortDate(DateTime date)
        {
            return date.ToString("dd")+"/"+date.ToString("MMM")+"/"+date.ToString("yyyy");
        }

        //private static string ImagesFolderPath = @"C:\DVLD\People-Images\";
        public static bool CreateFolderIfDoesNotExist(string FolderPath)
        {

            // Check if the folder exists
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    // If it doesn't exist, create the folder
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message);
                    return false;
                }
            }

            return true;

        }

        private static string ReplaceFileNameWithGUID(string sourceFile)
        {
            string GUID = Guid.NewGuid().ToString();

            FileInfo fileInfo = new FileInfo(sourceFile);
            string ext =  fileInfo.Extension;

            return GUID + ext;

        }
        public static bool CopyImageToProjectImagesFolder(ref string sourceFile)
        {
            // this funciton will copy the image to the
            // project images foldr after renaming it
            // with GUID with the same extention, then it will update the sourceFileName with the new name.

            string DestinationFolder = @"C:\DVLD\People-Images\";
            if (!CreateFolderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }

            string destinationFile = DestinationFolder + ReplaceFileNameWithGUID(sourceFile);
            try
            {
                File.Copy(sourceFile, destinationFile, true);

            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            sourceFile = destinationFile;
            return true;
        }


        /// <summary>
        /// //////////////////
        /// </summary>

        private static string RegistryKeyPath = @"HKEY_CURRENT_USER\";
        private static string UserLoginInfoNode = @"Software\DVLD\UserLoginInfo";

       
        private static string UserNameValueName = "UserName";
        private static string PasswordValueName = "Password";

        // Save login information to registry

        /// <summary>
        /// This function take UserName and Password and store them at Curren_User Registry
        /// </summary>
        /// <param name="userName">Logged in UserName</param>
        /// <param name="password">Logged in Password</param>
        /// <returns>Boolean true: if saved successful or false: if failed to saved</returns>
                
        public static bool SaveLoginInformationToRegistry(string userName, string password)
        {
            string LoginInfoKeyName = RegistryKeyPath + UserLoginInfoNode;            

            try
            {                
                Registry.SetValue(LoginInfoKeyName, UserNameValueName, userName, RegistryValueKind.String);
                Registry.SetValue(LoginInfoKeyName, PasswordValueName, password, RegistryValueKind.String);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return false;
        }

        // Load login information from registry
        public static bool LoadLoginInformationFromRegistry(ref string userName,ref string password)
        {
            string LoginInfoKeyName = RegistryKeyPath + UserLoginInfoNode;

            try
            {
                userName = (string)Registry.GetValue(LoginInfoKeyName,UserNameValueName,null);
                password = (string)Registry.GetValue(LoginInfoKeyName,PasswordValueName,null);
                if (userName != null && password != null)
                    return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return false;

        }

        // Delete login information frm registry
        public static bool DeleteLoginInfoFromRegistry()
        {
            try
            {
                // Open the registry key in read/write mode with explicit registry view
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    using (RegistryKey key = baseKey.OpenSubKey(UserLoginInfoNode, true))
                    {
                        if (key != null)
                        {
                            // Delete the specified value
                            key.DeleteValue(UserNameValueName);
                            key.DeleteValue(PasswordValueName);

                            return true;
                            
                        }
                       
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");

            }
            return false;
        }

        public static string ComputeHash(string input)
        {
            //SHA is Secutred Hash Algorithm.
            // Create an instance of the SHA-256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));


                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

    }
}
