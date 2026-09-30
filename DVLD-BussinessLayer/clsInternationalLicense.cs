using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsInternationalLicense : clsApplication
    {

        enum enMode
        {
            AddNew, Update
        }
        enMode _Mode = enMode.AddNew;

        public int InternationalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public bool IsActive { get; set; }
        public int DriverID { get; set; }

        public clsInternationalLicense()
        {
            //here we set the applicaiton type to New International License.
            this.ApplicationTypeID = clsApplication.enApplicationType.NewInternationalLicense;

            this.InternationalLicenseID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;

            this.IsActive = true;


            _Mode = enMode.AddNew;
            base._Mode = clsApplication.enMode.AddNew;

        }
        public clsInternationalLicense(int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             double PaidFees, int CreatedByUserID,
             int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive)

        {
            //this is for the base clase
            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            base.ApplicationTypeID = clsApplication.enApplicationType.NewInternationalLicense;
            base.ApplicationStatus = ApplicationStatus;
            base.LastStatusDate = LastStatusDate;
            base.PaidFees = PaidFees;
            base.CreatedByUserID = CreatedByUserID;

            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;

            

            _Mode = enMode.Update;
        }

        public bool _AddNew()
        {
            if (!base.Save())
                return false;

            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(base.ApplicationID, IssueDate,
                ExpirationDate, IssuedUsingLocalLicenseID, DriverID, IsActive, base.CreatedByUserID);

            return InternationalLicenseID > -1;
        }

        public bool _Update()
        {
            if (!base.Save()) return false;

            return true;
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


        public static clsInternationalLicense Find(int LicenseID)
        {
            int applicationID = -1;
            DateTime issueDate = DateTime.MinValue;
            DateTime expirationDate = DateTime.MinValue;
            int driverID = -1;
            bool isActive = false;
            int issuedUsingLocalDrivingLicenseID = -1;
            int createdByUserID = -1;


            if (clsInternationalLicenseData.GetLicenseInfoByID(LicenseID, ref applicationID, ref issueDate,
                ref expirationDate, ref issuedUsingLocalDrivingLicenseID, ref driverID, ref isActive, ref createdByUserID))
            {

                
                clsApplication app = clsApplication.Find(applicationID);
                if (app == null)
                    return null;

                return new clsInternationalLicense(app.ApplicationID, app.ApplicantPersonID, app.ApplicationDate, app.ApplicationStatus,
                    app.LastStatusDate, app.PaidFees, app.CreatedByUserID, LicenseID, driverID, issuedUsingLocalDrivingLicenseID,
                    issueDate, expirationDate, isActive);

                
            }
            return null;

        }

        public static clsInternationalLicense GetActiveInternationalLicenseByDriverID(int DriverID)
        {

            int applicationID = -1;
            int localLiceseID = -1;
            int internationalLicenseID = -1;
            DateTime issueDate = DateTime.MinValue;
            DateTime expirationDate = DateTime.MinValue;
            bool isActive = false;
            int createdByUserID = -1;


            if (clsInternationalLicenseData.GetActiveInternationalLicenseByLocalLicenseID(DriverID, ref localLiceseID, ref internationalLicenseID, ref applicationID, ref issueDate,
                ref expirationDate, ref isActive, ref createdByUserID))
            {


                clsApplication app = clsApplication.Find(applicationID);
                if (app == null)
                    return null;

                return new clsInternationalLicense(app.ApplicationID, app.ApplicantPersonID, app.ApplicationDate, app.ApplicationStatus,
                    app.LastStatusDate, app.PaidFees, app.CreatedByUserID, internationalLicenseID, DriverID, localLiceseID,
                    issueDate, expirationDate, isActive);

            }
            return null;

        }
        public static DataTable ListAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

    }
}
