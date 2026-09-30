using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {

        public int ID { get; set; }
        public int LicenseClassID { get; set; }
        public string LicenseClassName {
            get
            {
                return clsLicenseClass.GetLicenseClassName(LicenseClassID);
            }
        }

        enum enMode { AddNew, Update }
        enMode _Mode;



        public clsLocalDrivingLicenseApplication()
        {
            ID = -1;
            LicenseClassID = -1;
            ApplicationID = -1;
            ApplicationTypeID = enApplicationType.NewLocalLicense;
            PaidFees = clsApplicationType.Find((int)ApplicationTypeID).ApplicationFees;

            _Mode = enMode.AddNew;

        }

        clsLocalDrivingLicenseApplication(
                int lDLApplicationID, int applicationID, int applicantPersonID, DateTime applicationDate,
                enApplicationType applicationTypeID, enApplicationStatus applicationStatus,
                DateTime lastStatusDate, double paidFees, int licenseClassID, int createdByUserID
            )
        {
            ID = lDLApplicationID;
            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            LicenseClassID = licenseClassID;
            CreatedByUserID = createdByUserID;

            base._Mode = clsApplication.enMode.Update;
            _Mode = enMode.Update;

        }

        public static clsLocalDrivingLicenseApplication GetPersonActiveApplicationLicenseWithClass(int personID, int licenseClass)
        {
            //We should return object later on :

            int applicationID = -1;
            int LocalLicenseAppID = -1;


            if (clsLocalDrivingLicenseApplicationData.GetPersonActiveApplicationWithLicenseClass(personID, (int)licenseClass, ref LocalLicenseAppID, ref applicationID)) {

                clsApplication application = clsApplication.Find(applicationID);

                return new clsLocalDrivingLicenseApplication(LocalLicenseAppID, applicationID, application.ApplicationID, application.ApplicationDate, application.ApplicationTypeID
                    , application.ApplicationStatus, application.LastStatusDate, application.PaidFees, licenseClass, application.CreatedByUserID);
            }
            return null;
        }
        private bool _AddNew()
        {


            if (base.Save())
            {

                ID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(ApplicationID, (int)LicenseClassID);
            }

            return ID != -1;

        }
        public bool _Update() {



            if (base.Save()) {

                return (clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication(ID, ApplicationID, (int)LicenseClassID));

            }
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
            }
            return false;

        }

        public static clsLocalDrivingLicenseApplication Find(int LocalDrivingLicenseAppID)
        {
            int applicationID = -1, applicantPersonID = -1, createdByUser;//last
            DateTime applicationDate = DateTime.Now;
            enApplicationType applicationTypeID = enApplicationType.NewLocalLicense;
            enApplicationStatus applicationStatus = enApplicationStatus.New;
            DateTime lastStatusDate = DateTime.Now;
            double paidFees = 0.0;
            int licenseClassID = -1;


            if (clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID(LocalDrivingLicenseAppID, ref applicationID, ref licenseClassID))
            {
                clsApplication Application = clsApplication.Find(applicationID);
                if (Application == null)
                    return null;

                applicantPersonID = Application.ApplicantPersonID;
                applicationDate = Application.ApplicationDate;
                applicationTypeID = Application.ApplicationTypeID;
                applicationStatus = Application.ApplicationStatus;
                lastStatusDate = Application.LastStatusDate;
                paidFees = Application.PaidFees;
                createdByUser = Application.CreatedByUserID;

                //return full object
                //

                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseAppID, applicationID, applicantPersonID, applicationDate,
                    applicationTypeID, applicationStatus, lastStatusDate, paidFees, licenseClassID, createdByUser);

            }
            return null;

        }

        public bool Delete()
        {
            //Delete local then delete base
            if (clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(ID))
            {
                if (base.Delete(ApplicationID))
                    return true;
            }
            return false;

        }

        public static DataTable ListAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }

        public static bool DoesPassTestType(int LDLAppID, clsTestType.enTestType TestType)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(LDLAppID, (int)TestType);
        }

        public int GetPassedTestsCount()
        {
            return clsLocalDrivingLicenseApplicationData.GetPassedTestsCount(ID);
        }

        public static int GetTestTrialCount(int LDLAppID, clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetTestTrialCount(LDLAppID, (int)TestTypeID);
        }

        public static bool DoesHaveActiveTestAppointment(int LDLAppID, clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.DoesHaveActiveTestAppointment(LDLAppID, (int)TestTypeID);
        }

        public static bool DoesFailPrevTest(int LDLAppID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesFailPrevTest(LDLAppID, (int)TestTypeID);
        }

        public static int GetIssuedLicenseID(int LDLAppID)
        {
            return clsLocalDrivingLicenseApplicationData.GetIssuedLicenseID(LDLAppID);
        }


    }
}
