using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD_BusinessLayer
{
    public class clsUser
    {

        enum enMode { AddNew,Update};
        enMode _Mode;
        public int UserID {  get; set; }
        public int PersonID {  get; set; }
        public clsPerson PersonInfo; //composition
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public clsUser()
        {
            UserID = -1;
            PersonID = -1;
            UserName = "";
            Password = "";
            IsActive = false;

            _Mode = enMode.AddNew;

        }

        private clsUser(int userID, int personID, string userName, string password, bool isActive)
        {
            UserID = userID;
            PersonID = personID;
            UserName = userName;
            Password = password;
            IsActive = isActive;
            PersonInfo = clsPerson.Find(personID);//composition

            _Mode = enMode.Update;
        }

        private bool _AddNew()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID,this.UserName,this.Password,this.IsActive);

            return UserID > -1;
        }
        private bool _Update()
        {

            return clsUserData.UpdateUser(this.UserID, this.UserName, this.Password, this.IsActive);
        }
        public static bool Delete(int userID)
        {
            return clsUserData.DeleteUser(userID);
        }
        public static clsUser Find(int userID)
        {
            int personID = -1;
            string userName = "", password = "";
            bool isActive = false;            

            if (clsUserData.GetUserByID(userID, ref personID,ref userName,ref password,ref isActive))
            {
                return new clsUser(userID, personID, userName, password, isActive);
            }
            return null;

        }
        public static clsUser Find(string userName, string password)
        {
            int personID = -1;
            int userID = -1;
            //string userName = "", password = "";
            bool isActive = false;


            if (clsUserData.GetUserByUserNameAndPassword(userName,password,ref userID, ref personID, ref isActive))
            {
                return new clsUser(userID, personID, userName, password, isActive);
            }
            return null;

        }
        public static bool IsExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }
        public static bool IsExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }
        public static bool IsExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID(PersonID);
        }
        public static DataTable ListAllUsers()
        {
            return clsUserData.GetAllUsers();
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _Update();
            }
            return false;

        }


    }
}