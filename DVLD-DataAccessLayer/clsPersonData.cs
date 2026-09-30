using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using DTOs;

namespace DVLD_DataAccessLayer
{
    public static class clsPersonData
    {
        public static int AddNewPerson(dtoPerson dto)
        {
            int NewPersonID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_AddNewPerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NationalNo", dto.NationalNo);
                    command.Parameters.AddWithValue("@FirstName", dto.FirstName);
                    command.Parameters.AddWithValue("@SecondName", dto.SecondName);
                    command.Parameters.AddWithValue("@ThirdName",
                        (object)dto.ThirdName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", dto.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", dto.DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", dto.Gender);
                    command.Parameters.AddWithValue("@Address", dto.Address);
                    command.Parameters.AddWithValue("@Phone", dto.Phone);
                    command.Parameters.AddWithValue("@Email",
                        (object)dto.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NationalityCountryID",
                        dto.NationalityCountryID);
                    command.Parameters.AddWithValue("@ImagePath",
                        (object)dto.ImagePath ?? DBNull.Value);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                            NewPersonID = Convert.ToInt32(result);
                    }
                    catch (Exception)
                    {
                        // Logging here
                        throw;
                    }
                }
            }

            return NewPersonID;
        }

        public static bool UpdatePerson(dtoPerson dtoPerson)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdatePerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", dtoPerson.PersonID);
                    command.Parameters.AddWithValue("@NationalNo", dtoPerson.NationalNo);
                    command.Parameters.AddWithValue("@FirstName", dtoPerson.FirstName);
                    command.Parameters.AddWithValue("@SecondName", dtoPerson.SecondName);

                    if (string.IsNullOrEmpty(dtoPerson.ThirdName))
                        command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@ThirdName", dtoPerson.ThirdName);

                    command.Parameters.AddWithValue("@LastName", dtoPerson.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", dtoPerson.DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", dtoPerson.Gender);
                    command.Parameters.AddWithValue("@Address", dtoPerson.Address);
                    command.Parameters.AddWithValue("@Phone", dtoPerson.Phone);

                    if (string.IsNullOrEmpty(dtoPerson.Email))
                        command.Parameters.AddWithValue("@Email", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Email", dtoPerson.Email);

                    command.Parameters.AddWithValue(
                        "@NationalityCountryID",
                        dtoPerson.NationalityCountryID);

                    if (string.IsNullOrEmpty(dtoPerson.ImagePath))
                        command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@ImagePath", dtoPerson.ImagePath);

                    try
                    {
                        connection.Open();

                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        // Logging here
                        throw;
                    }
                }
            }

            return rowsAffected > 0;
        }

        public static bool DeletePerson(int PersonID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DeletePerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        // Logging here
                        throw;
                    }
                }
            }

            return rowsAffected > 0;
        }

        public static dtoPerson GetPersonByID(int PersonID)
        {
            dtoPerson dtoPerson = null;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPersonByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dtoPerson = new dtoPerson(
                                    (int)reader["PersonID"],
                                    reader["NationalNo"].ToString(),
                                    reader["FirstName"].ToString(),
                                    reader["SecondName"].ToString(),
                                    reader["ThirdName"] == DBNull.Value
                                        ? ""
                                        : reader["ThirdName"].ToString(),
                                    reader["LastName"].ToString(),
                                    reader["Email"] == DBNull.Value
                                        ? ""
                                        : reader["Email"].ToString(),
                                    reader["Phone"].ToString(),
                                    (DateTime)reader["DateOfBirth"],
                                    Convert.ToByte(reader["Gender"]),
                                    reader["Address"].ToString(),
                                    (int)reader["NationalityCountryID"],
                                    reader["ImagePath"] == DBNull.Value
                                        ? ""
                                        : reader["ImagePath"].ToString()
                                );
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Logging here
                        throw;
                    }
                }
            }

            return dtoPerson;
        }

        public static dtoPerson GetPersonByNationalNo(string NationalNo)
        {
            dtoPerson dtoPerson = null;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPersonByNationalNo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dtoPerson = new dtoPerson(
                                    (int)reader["PersonID"],
                                    reader["NationalNo"].ToString(),
                                    reader["FirstName"].ToString(),
                                    reader["SecondName"].ToString(),
                                    reader["ThirdName"] == DBNull.Value
                                        ? ""
                                        : reader["ThirdName"].ToString(),
                                    reader["LastName"].ToString(),
                                    reader["Email"] == DBNull.Value
                                        ? ""
                                        : reader["Email"].ToString(),
                                    reader["Phone"].ToString(),
                                    (DateTime)reader["DateOfBirth"],
                                    Convert.ToByte(reader["Gender"]),
                                    reader["Address"].ToString(),
                                    (int)reader["NationalityCountryID"],
                                    reader["ImagePath"] == DBNull.Value
                                        ? ""
                                        : reader["ImagePath"].ToString()
                                );
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Logging here
                        throw;
                    }
                }
            }

            return dtoPerson;
        }

        public static bool IsPersonExistByID(int PersonID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_IsPersonExistByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        return command.ExecuteScalar() != null;
                    }
                    catch (Exception)
                    {
                        // Logging here
                        throw;
                    }
                }
            }
        }

        public static bool IsPersonExistByNationalNo(string NationalNo)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_IsPersonExistByNationalNo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    try
                    {
                        connection.Open();

                        return command.ExecuteScalar() != null;
                    }
                    catch (Exception)
                    {
                        // Logging here
                        throw;
                    }
                }
            }
        }

        public static DataTable GetAllPeople()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_ReadAllPeople", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                    catch (Exception)
                    {
                        // Logging here
                        throw;
                    }
                }
            }

            return dataTable;
        }


        public static async Task<DataTable> GetPeopleAsync(int PageNumber, int RowPerPage)
        {
            DataTable dataTable = new DataTable();

            
            using ( SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_FetchPeople", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PageNumber", PageNumber);
                    command.Parameters.AddWithValue("@RowsPerPage", RowPerPage);

                    try
                    {
                        await connection.OpenAsync();
                        //connection.Open();

                        //Should all function in call stack be async
                        //using (SqlDataReader reader = command.ExecuteReaderAsync())
                        //{
                        //    dataTable.Load(reader);
                        //}


                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader);
                        }
                    }
                    catch (Exception)
                    {
                        // Logging here
                        throw;
                    }
                }
            }

            return  dataTable;
        }



    }
}