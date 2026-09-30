using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsLicenseClass
    {
        //int LicenseClassID, ref string LicenseClassTitle, ref string ClassDescription,
            //ref short MinimumAllowedAge, short DefaultValidityLength, double ClassFees

        public int LicenseClassID          {  get; set; }
        public string LicenseClassTitle    {  get; set; }
        public string ClassDescription     {  get; set; }
        public short MinimumAllowedAge     {  get; set; }
        public short DefaultValidityLength {  get; set; }
        public double ClassFees            {  get; set; }


        clsLicenseClass(int licenseClassID, string licenseClassTitle, string classDescription,
            short minimumAllowedAge, short defaultValidityLength, double classFees  ) { 
            
            this.LicenseClassID = licenseClassID;
            this.LicenseClassTitle = licenseClassTitle;
            this.ClassDescription = classDescription;
            this.MinimumAllowedAge = minimumAllowedAge;
            this.DefaultValidityLength = defaultValidityLength;
            this.ClassFees = classFees;

        }
        public static clsLicenseClass Find(int LicenseClassID)
        {

            string LicenseClassTitle = "";
            string ClassDescription = "";
            short MinimumAllowedAge = 0;
            short DefaultValidityLength = 0;
            double ClassFees = 0.0;


            if (clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID, ref LicenseClassTitle, ref ClassDescription,
                ref  MinimumAllowedAge,ref DefaultValidityLength, ref ClassFees)) {

                return new clsLicenseClass(LicenseClassID, LicenseClassTitle, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }return null;

        }

        public static bool IsExist(int LicenseClassID) { 
            return clsLicenseClassData.IsLicenseClassExist(LicenseClassID);
        }

        public static DataTable ListAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }

        public static string GetLicenseClassName(int LicenseClassID) {

            string ClassName = "";

            clsLicenseClassData.GetLicenseClassName(LicenseClassID,ref ClassName);

            return ClassName;

        }


    }
}
