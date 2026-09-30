using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsTest
    {
        enum enMode
        {
            AddNew ,Updated
        }
        enMode _Mode;

        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes {  get; set; }
        public int CreatedByUserID {  get; set; }

        public clsTest() {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = string.Empty;
            CreatedByUserID = -1;
            _Mode = enMode.AddNew;  
        }

        clsTest(int testID,int testAppointmentID, bool testResult, string notes, int createdByUser)
        {
            TestID = testID;
            TestAppointmentID = testAppointmentID;
            TestResult = testResult;
            Notes = notes;
            CreatedByUserID = createdByUser;

            _Mode= enMode.Updated;
        }


        public static clsTest Find(int testID)
        {
            int testAppointmentID = -1, createdByUser = -1;
            bool testResult = false;
            string notes = "";

            if (clsTestData.GetTestInfoByID(testID, ref testAppointmentID, ref testResult, ref notes, ref createdByUser)) {
                return new clsTest(testID, testAppointmentID, testResult, notes, createdByUser);
            } else
                return null;

        }

        private bool _AddNew()
        {
            this.TestID = clsTestData.AddNewTest(TestAppointmentID,TestResult,Notes,CreatedByUserID);
            return TestID != -1;
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        _Mode = enMode.Updated;
                        return true;
                    }
                    return false;
                case enMode.Updated:
                    return true; ///update method here...
                default:
                    return false;
            }
        }




    }
}
