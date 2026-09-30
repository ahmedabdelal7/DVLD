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
    public class clsApplicationType
    {
        public int ApplicationTypeID {  get; } 
        public string ApplicationTypeTitle { get; set; }
        public double ApplicationFees { get; set; }

        public clsApplicationType() {
            ApplicationTypeID = -1;
            ApplicationTypeTitle = string.Empty;
            ApplicationFees = 0.0;
        }

        clsApplicationType(int applicationTypeID, string applicationTypeTitle, double applicationFees)
        {
            ApplicationTypeID = applicationTypeID;
            ApplicationTypeTitle = applicationTypeTitle;
            ApplicationFees = applicationFees;
        }

        public static clsApplicationType Find(int applicationTypeID)
        {
            string applicationTypeTitle = "";
            double applicationFees = 0.0;

            if(clsApplicationTypesData.GetApplicationTypeInfoByID(applicationTypeID, ref applicationTypeTitle, ref applicationFees))
            {
                return new clsApplicationType(applicationTypeID, applicationTypeTitle, applicationFees);
            }
            return null;


        }

        public static DataTable ListAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationTypes();
        }

        private bool _UpdateApplicationType()
        {
            return (clsApplicationTypesData.UpdateApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationFees));
            
        }

        public bool Save()
        {
            return (_UpdateApplicationType());
            
        }


        public static string GetApplicationTypeTitle(int ApplicationTypeID) {

            return clsApplicationTypesData.GetApplicationTypeName(ApplicationTypeID);

        }

        public static double GetApplicationFees(int ApplicationTypeID) { 
            return clsApplicationTypesData.GetApplicationTypeFees(ApplicationTypeID);
        }

    }
}
