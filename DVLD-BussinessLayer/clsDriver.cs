using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsDriver
    {
        public int DriverID {  get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID {  get; set; }
        public DateTime CreatedDate { get; set; }  
        

        enum enMode
        {
            AddNew, Update
        }
        enMode _Mode;
        public clsDriver() {
            DriverID = -1;
            PersonID = -1;
            CreatedDate = DateTime.Now;
            CreatedByUserID = -1;
            _Mode = enMode.AddNew;
        }

        clsDriver(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            DriverID = driverID;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            CreatedDate = createdDate;
            _Mode = enMode.Update;
        }
        private bool _AddNew()
        {
            this.DriverID = clsDriverData.AddNewDriver(PersonID,CreatedByUserID, CreatedDate);
            return DriverID > -1;
        }
        private bool _Update() {
            int personID = -1;
            int createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            return true;
        }
        public static clsDriver FindByDriverID(int driverID)
        {
            int personID = -1;
            int createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if(clsDriverData.GetDriverInfoByDriverID(driverID,ref personID, ref createdByUserID, ref createdDate))
                return new clsDriver(driverID,personID,createdByUserID, createdDate);
            return null;
        }

        public static clsDriver FindByPersonID(int personID)
        {
            int driverID = -1;
            int createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByPersonID(personID, ref driverID,  ref createdByUserID, ref createdDate))
                return new clsDriver(driverID, personID, createdByUserID, createdDate);
            return null;
        }

        public static bool IsExistByPersonID(int PersonID)
        {
            return clsDriverData.IsDriverExistByPersonID(PersonID);
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


        public static DataTable ListAllLocalLicenses(int DriverID)
        {
            return clsDriverData.GetAllLocalLicensesByDriverID(DriverID);
        }

        public static DataTable ListAllInternationalLicenses(int DriverID)
        {
            return clsDriverData.GetAllInternationalLicensesByDriverID(DriverID);
        }

        public static DataTable ListAllDrivers()
        {
            return clsDriverData.GetAllDrivers();
        } 
    }
}
