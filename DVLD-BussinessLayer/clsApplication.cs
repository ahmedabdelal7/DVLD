using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccessLayer;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_BusinessLayer
{
    public class clsApplication
    {
        //int ApplicationID, ref int ApplicantPersonID,ref DateTime ApplicationDate,ref int ApplicationTypeID,
        //    ref ,ref DateTime LastStatusDate,ref double PaidFees,ref int CreatedByUserID

        public enum enMode
        {
            Update, AddNew
        }
        public enMode _Mode;

        public enum enApplicationStatus { New =1, Cancelled = 2, Completed = 3}

        public enum enApplicationType
        {
            NewLocalLicense = 1, RenewLicense = 2, ReplaceLostLicense = 3,
            ReplaceDamagedLicense = 4, ReleaseDetainedLicense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };

        public string ApplicantFullName
        {
            get
            {
                return clsPerson.Find(ApplicantPersonID).FullName;
            }
        }

        public int ApplicationID {  get; set; }
        public int ApplicantPersonID {  get; set; }
        public DateTime ApplicationDate {  get; set; }
        public enApplicationType ApplicationTypeID {  get; set; }
        public enApplicationStatus ApplicationStatus {  get; set; }
        public DateTime LastStatusDate {  get; set; }
        public double PaidFees {  get; set; }
        public int CreatedByUserID {  get; set; }

        public clsApplicationType ApplicationTypeInfo { get { return clsApplicationType.Find((int)ApplicationTypeID); } }


        public clsApplication(int applicationID, int applicantPersonID, DateTime applicationDate, enApplicationType applicationTypeID,
            enApplicationStatus applicationStatus, DateTime lastStatusDate, double paidFees, int createdByUserID )
        {
            _Mode = enMode.Update;
            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
        }
        public clsApplication() {
            ApplicationID = -1;
            ApplicantPersonID = -1;
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = enApplicationType.NewLocalLicense;
            ApplicationStatus = enApplicationStatus.New;
            LastStatusDate = DateTime.Now;
            PaidFees = 0.0;
            CreatedByUserID = -1;
            _Mode = enMode.AddNew;

        }


        private bool _AddNew()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(ApplicantPersonID, ApplicationDate,(int) ApplicationTypeID, (short)ApplicationStatus,
                LastStatusDate, PaidFees, CreatedByUserID);

            return ApplicationID > -1;
        }
        private bool _Update()
        {

            return clsApplicationData.UpdateApplicationStatus(ApplicationID, (short)ApplicationStatus,LastStatusDate);
        }

        protected bool Delete(int ApplicationID)
        {

            if(ApplicationStatus == enApplicationStatus.New)
            {
                return clsApplicationData.DeleteApplication(ApplicationID);

            }
            return false;
        }
        public static clsApplication Find(int ApplicationID)
        {
             int applicantPersonID = -1; DateTime applicationDate = DateTime.MinValue; int applicationTypeID = -1;
            short applicationStatus = 0; DateTime lastStatusDate = DateTime.MinValue;  double paidFees = 0.0; int createdByUserID = -1;


            if (clsApplicationData.GetApplicationInfoByID(ApplicationID, ref applicantPersonID, ref applicationDate, ref applicationTypeID,
                ref applicationStatus ,ref lastStatusDate, ref paidFees, ref createdByUserID))
            {
                return new clsApplication(ApplicationID, applicantPersonID, applicationDate, (enApplicationType)applicationTypeID,
                        (enApplicationStatus)applicationStatus, lastStatusDate, paidFees, createdByUserID);
            }
            return null;

        }
        
        public static bool IsExist(int ApplicationID)
        {
            return clsApplicationData.IsApplicationExist(ApplicationID);
        }
       
        public static DataTable ListAllApplications()
        {

            return clsApplicationData.GetAllApplications();
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
        public bool CancelApplication()
        {

            if(ApplicationStatus == enApplicationStatus.New)
            {
                ApplicationStatus = enApplicationStatus.Cancelled;
                LastStatusDate = DateTime.Now;
                return Save();
            }
            
            return false;
        }

        public bool CompleteApplication()
        {
            if (ApplicationStatus == enApplicationStatus.New) {

                if (ApplicationTypeID == enApplicationType.NewLocalLicense) { 
                    //Check if tests passes first..later 
                }
                ApplicationStatus = enApplicationStatus.Completed;
                LastStatusDate = DateTime.Now;
                return Save();
            }
            return false;

        }

        public string GetStatusText()
        {
            switch (ApplicationStatus)
            {
                case enApplicationStatus.New:
                    return "New";
                case enApplicationStatus.Cancelled:
                    return "Cancelled";
                case enApplicationStatus.Completed:
                    return "Completed";
                default:
                    return "";
            }
        }



    }
}
