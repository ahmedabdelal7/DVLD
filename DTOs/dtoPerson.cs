using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class dtoPerson
    {
        public int PersonID;
        public string NationalNo;
        public string FirstName;
        public string SecondName;
        public string ThirdName;
        public string LastName;
        public string Email;
        public string Phone;
        public DateTime DateOfBirth;
        public byte Gender;
        public string Address;
        public int NationalityCountryID;
        public string ImagePath;

        public dtoPerson
            (
                int personID, string nationalNo, string firstName, string secondName, 
                string thirdName, string lastName, string email, string phone,
                DateTime dateOfBirth, byte gender, string address,
                int nationalityCountryID, string imagePath
            )

        {
            PersonID = personID;
            NationalNo = nationalNo;
            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            NationalityCountryID = nationalityCountryID;
            ImagePath = imagePath;
        }
    }
}
