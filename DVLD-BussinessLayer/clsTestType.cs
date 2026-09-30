using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsTestType
    {
        public enTestType TestTypeID { get; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public double TestTypeFees { get; set; }

        public enum enTestType
        {
            Vision = 1, Written, Practical 
        }
        public clsTestType()
        {
            TestTypeID = enTestType.Vision;
            TestTypeTitle = string.Empty;
            TestTypeDescription = string.Empty;
            TestTypeFees = 0.0;
        }

        clsTestType(enTestType testTypeID, string testTypeTitle, string testTypeDescription, double testTypeFees)
        {
            TestTypeID = testTypeID;
            TestTypeTitle = testTypeTitle;
            TestTypeDescription= testTypeDescription;
            TestTypeFees = testTypeFees;
        }

        public static clsTestType Find(enTestType TestTypeID)
        {
            string TestTypeTitle = "";
            string TestTypeDescription = "";
            double TestTypeFees = 0.0;

            if (clsTestTypeData.GetTestTypeInfoByID((int)TestTypeID, ref TestTypeTitle, ref TestTypeDescription, ref TestTypeFees))
            {
                return new clsTestType(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            }
            return null;


        }

        public static DataTable ListAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }

        private bool _UpdateTestType()
        {
            return (clsTestTypeData.UpdateTestType((int)TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees));

        }

        public bool Save()
        {
            return (_UpdateTestType());

        }

        public static double GetTestFees(enTestType TestTypeID)
        {
            return clsTestTypeData.GetTestFeesByTestTypeID((int)TestTypeID);
        }
    }
}
