using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DVLD_DataAccessLayer
{
    public static class clsUserData
    {
        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive)
                                VALUES
                                (@PersonID, @UserName, @Password, @IsActive);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);

                    //...

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                            UserID = InsertedID;

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

            }

            return UserID;
        }
        public static bool UpdateUser(int UserID, string UserName, string Password, bool IsActive)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {

                string query = @"UPDATE Users
                                SET UserName = @UserName,
								    Password = @Password,
								    IsActive = @IsActive
                                WHERE UserID = @UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);


                    try
                    {
                        connection.Open();

                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return (rowsAffected > 0);
        }
        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        connection.Open();

                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }


            return (rowsAffected > 0);
        }
        public static bool GetUserByID(int UserID, ref int PersonID, ref string UserName, ref string Password,ref bool IsActive)
        {
            bool IsFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {

                string query = "SELECT * FROM Users WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;

                                PersonID = Convert.ToInt32(reader["PersonID"]);
                                UserName = reader["UserName"].ToString();
                                Password = reader["Password"].ToString();
                                IsActive = Convert.ToBoolean(reader["IsActive"]);

                            }
                        }

                    }
                    catch (Exception)
                    {
                        throw;

                    }

                }
            }

            return IsFound;
        }
        public static bool GetUserByUserNameAndPassword(string UserName, string Password, ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool IsFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {

                string query = "SELECT * FROM Users WHERE UserName = @UserName and Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;

                                PersonID = Convert.ToInt32(reader["PersonID"]);
                                UserID = Convert.ToInt32(reader["UserID"]);
                                IsActive = Convert.ToBoolean(reader["IsActive"]);

                            }
                        }
                    }
                    catch (Exception) { throw; }
                }

            }


            return IsFound;
        }
        public static bool IsUserExist(int UserID)
        {
            bool IsExist = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT Found=1 FROM Users WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        IsExist = (result != null && int.TryParse(result.ToString(), out int InsertedID));

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return IsExist;
        }  
        public static bool IsUserExist(string UserName)
        {
            bool IsExist = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT Found=1 FROM Users WHERE UserName = @UserName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        IsExist = (result != null && int.TryParse(result.ToString(), out int found));

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return IsExist;
        }
        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool IsExist = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT Found=1 FROM Users WHERE PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        IsExist = (result != null && int.TryParse(result.ToString(), out int InsertedID));
                    }
                    catch (Exception) { throw; }
                }
                
            }
            return IsExist;
        }
        public static DataTable GetAllUsers()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT 
		                                Users.UserID,
		                                Users.PersonID,
		                                FirstName+' '+
		                                SecondName+' '+
		                                ISNULL(ThirdName,'')+' '+
		                                LastName as FullName,
		                                Users.UserName, Users.IsActive
                                FROM	Users INNER JOIN
		                                People ON Users.PersonID = People.PersonID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //if (reader.HasRows)
                            dataTable.Load(reader);
                        }
                    }
                    catch (Exception) { throw; }
                }
            }

            return dataTable;
        }

    }
}
