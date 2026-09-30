using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsLicense
    {
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClassID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public Double PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public int CreatedByUserID { get; set; }
        public int PersonID
        {
            get
            {
                return clsDriver.FindByDriverID(DriverID).PersonID;
            }
        }
        public clsLicenseClass LicenseClassInfo
        {
            get
            {
                return clsLicenseClass.Find(LicenseClassID);
            }
        }


        public enum enIssueReason
        {
            FirstTime = 1, Renew, ReplacementForDamaged, ReplacementForLost
        }


        enum enMode
        {
            AddNew, Update
        }
        enMode _Mode;
        public clsLicense()
        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClassID = -1;
            IssueDate = DateTime.MinValue;
            ExpirationDate = DateTime.MinValue;
            Notes = "";
            PaidFees = 0.0;
            IsActive = false;
            IssueReason = enIssueReason.FirstTime; 
            CreatedByUserID = -1;

            _Mode = enMode.AddNew;
        }

        clsLicense(int licenseID, int applicationID, int driverID, int licenseCLass,DateTime issueDate, DateTime expirationDate,
            string notes, double paidFees, bool isActive, enIssueReason issueReason, int createdByUserID)
        {
            this.LicenseID = licenseID;
            this.ApplicationID = applicationID;
            this.DriverID = driverID;
            this.LicenseClassID = licenseCLass;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.Notes = notes;
            this.PaidFees = paidFees;
            this.IsActive = isActive;
            this.IssueReason = issueReason;
            this.CreatedByUserID = createdByUserID;


            _Mode = enMode.Update;
        }
        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1;
            int LicenseClassID = -1;
            DateTime IssueDate = DateTime.MinValue;
            DateTime ExpirationDate = DateTime.MinValue;
            string Notes = "";
            double PaidFees = 0.0;
            bool IsActive = false;
            byte IssueReason = 1;
            int CreatedByUserID = -1;


            if (clsLicenseData.GetLicenseInfoByID(LicenseID,ref ApplicationID,ref DriverID,ref LicenseClassID,ref IssueDate,
                ref ExpirationDate,ref Notes,ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID,IssueDate, ExpirationDate, Notes,PaidFees,
                    IsActive,(enIssueReason) IssueReason, CreatedByUserID);
            }
            return null;
        }
        public static DataTable ListAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }
        private bool _AddNew()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(ApplicationID,DriverID,LicenseClassID,IssueDate,ExpirationDate,
                Notes,PaidFees,IsActive,(byte)IssueReason,CreatedByUserID);

            return LicenseID > -1;
        }
        private bool _Update()
        {

            return false;
        }
        public static bool Delete(int ID)
        {
            return false;
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
                default:
                    return false;
            }
        }

        public string GetIssueReasonText()
        {
            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.ReplacementForLost:
                    return "Replacement For Lost";
                case enIssueReason.ReplacementForDamaged:
                    return "Replacement For Damaged";
                default: return "";
            }
        }

        public static bool DoesHaveLicenseOfLicenseClass(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.DoesHaveLicenseOfLicenseCLass(PersonID, LicenseClassID);
        }

        public bool Deactivate()
        {
            return clsLicenseData.DeactivateLicense(LicenseID);
        }

        
    }
}
