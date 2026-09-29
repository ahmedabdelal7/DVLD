using DTOs;
using DVLD_BusinessLayer;
using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD_BussinessLayer
{
    public class clsPerson
    {

        enum enMode
        {
            AddNew, Update
        }
        enMode _Mode;
        public enum enGender
        {
           Male = 0, Female
        }
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; } }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public enGender Gender { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }

        //Composition with country table.
        public clsCountry CountryInfo;
        public clsPerson()
        {
            PersonID = -1;
            NationalNo = string.Empty;
            FirstName = string.Empty;
            SecondName = string.Empty;
            ThirdName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.MinValue;
            Email = string.Empty;
            Address = string.Empty;
            Phone = string.Empty;
            Gender = enGender.Male;
            NationalityCountryID = -1;
            ImagePath = string.Empty;

            _Mode = enMode.AddNew;

        }
        //Initialize new object withe default values:
        private clsPerson(dtoPerson dto)
        {
            PersonID = dto.PersonID;
            NationalNo = dto.NationalNo;
            FirstName = dto.FirstName;
            SecondName = dto.SecondName;
            ThirdName = dto.ThirdName;
            LastName = dto.LastName;
            DateOfBirth = dto.DateOfBirth;
            Email = dto.Email;
            Address = dto.Address;
            Phone = dto.Phone;
            Gender = (enGender) dto.Gender;
            NationalityCountryID = dto.NationalityCountryID;
            ImagePath = dto.ImagePath;

            CountryInfo = clsCountry.Find(NationalityCountryID);

            _Mode = enMode.Update;
        }

        private dtoPerson _GetDTOPersonInfo()
        {
            return new dtoPerson
                (
                    this.PersonID, this.NationalNo, this.FirstName, this.SecondName, this.ThirdName,
                    this.LastName, this.Email, this.Phone, this.DateOfBirth, (byte)this.Gender,
                    this.Address, this.NationalityCountryID, this.ImagePath
                );
        }

        private bool _AddNew()
        {
            dtoPerson dto = _GetDTOPersonInfo();
            this.PersonID = clsPersonData.AddNewPerson(dto);

            return PersonID > -1;
        }
        private bool _Update()
        {
            dtoPerson dto = _GetDTOPersonInfo();
            return clsPersonData.UpdatePerson(dto);
        }

        public static bool Delete(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }
        public static clsPerson Find(int PersonID)
        {
            dtoPerson dto = clsPersonData.GetPersonByID(PersonID);
            if (dto != null)
                return new clsPerson(dto);

            return null;
        }

        public static clsPerson Find(string NationalNo)
        {
            dtoPerson dto = clsPersonData.GetPersonByNationalNo(NationalNo);

            if(dto != null)
                return new clsPerson(dto);

            return null;

        }
        public static bool IsExist(int PersonID)
        {
            return clsPersonData.IsPersonExistByID(PersonID);
        }
        public static bool IsExist(string NationalNo)
        {
            return clsPersonData.IsPersonExistByNationalNo(NationalNo);
        }

        public static DataTable ListAllPeople() { 
            
            return clsPersonData.GetAllPeople();
        }

        async public static Task< DataTable> GetPeople(int PageNumber, int RowsPerPage)
        {
             
             return await clsPersonData.GetPeoples(PageNumber, RowsPerPage);
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
    }
}
