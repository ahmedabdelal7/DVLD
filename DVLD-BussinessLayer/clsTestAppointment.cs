using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsTestAppointment
    {
        public int TestAppointmentID { get; set; }
        public clsTestType.enTestType TestTypeID { get; set; }
        public int LDLAppID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public double PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int RetakeTestApplicationID { get; set; }

        

        enum enMode
        {
            AddNew, Update
        }
        enMode _Mode;
        public clsTestAppointment()
        {
            TestAppointmentID = -1;
            TestTypeID = clsTestType.enTestType.Vision;
            LDLAppID = -1;
            AppointmentDate = DateTime.MinValue;
            PaidFees = 0.0;
            CreatedByUserID = -1;
            IsLocked = false;
            RetakeTestApplicationID = -1;

            _Mode = enMode.AddNew;
        }

        clsTestAppointment(int testAppointmentID, clsTestType.enTestType testTypeID, int lDLAppID, DateTime appointmentDate,
            double paidFees, int createdByUserID, bool isLocked,int retakeTestAppointmentID)
        {
            this.TestAppointmentID = testAppointmentID;
            this.TestTypeID = testTypeID;
            this.LDLAppID = lDLAppID;
            this.AppointmentDate = appointmentDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;
            this.IsLocked = isLocked;
            this.RetakeTestApplicationID = retakeTestAppointmentID;

            _Mode= enMode.Update;
        }
        public static clsTestAppointment Find(int TestAppointmentID)
        {
            int testTypeID = -1;
            int lDLAppID = -1;
            DateTime appointmentDate = DateTime.MinValue;
            double paidFees = 0;
            int createdByUserID = -1;
            bool isLocked = false;
            int retakeTestAppointmentID = -1;


            if(clsTestAppointmentData.GetTestAppointmentInfoByID(TestAppointmentID,ref testTypeID,ref lDLAppID,ref appointmentDate,ref paidFees,
                ref createdByUserID,ref isLocked,ref retakeTestAppointmentID))
            {
                return new clsTestAppointment(TestAppointmentID, (clsTestType.enTestType)testTypeID, lDLAppID,
                    appointmentDate, paidFees, createdByUserID, isLocked, retakeTestAppointmentID);
            }
            return null;
        }
        public static DataTable ListAllTestAppointments(int LDLAppID, clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetAllTestAppointmentsByAppIDAndTestType(LDLAppID, (int)TestTypeID);
        }
        private bool _AddNew()
        {
            this.TestAppointmentID = clsTestAppointmentData.AddNewTestAppointment((int)TestTypeID, LDLAppID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);

            return TestAppointmentID > -1;
        }
        private bool _Update() {

            return clsTestAppointmentData.UpdateTestAppointment(TestAppointmentID, AppointmentDate, IsLocked, RetakeTestApplicationID);
        }
        public static bool Delete(int ID)
        {
            return clsTestAppointmentData.DeleteTestAppointment(ID);
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
        

       

    }
}
