using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ImagePreviewSQL
{
    public class ImageDatabase
    {
        private SqlConnection connection;
        private int rows;

        private List<int> idArray = new List<int>();
        private List<string> nameArray = new List<string>();
        private List<byte[]> binaryList = new List<byte[]>();

        public int Rows
        {
            get { return rows; }
        }
        public List<int> IdArray
        {
            get { return idArray; }
        }
        public List<string> NameArray
        {
            get { return nameArray; }
        }
        public List<byte[]> BinaryList
        {
            get { return binaryList; }
        }

        public ImageDatabase(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }
    
        /// <summary>
        /// Used to add a image to the database
        /// </summary>
        public void AddImage(string name, byte[] binary)
        {
            // Creation of SqlCommand and sets it as a Stored Procedure
            SqlCommand command = new SqlCommand("InsertPicture", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Adding the first Parameter, which is the name of the image
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@name";
            param.Value = name;
            command.Parameters.Add(param);

            // Adding the second Parameter, which is the hexadecimal for the image 
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@pbinary";
            param1.Value = binary;
            command.Parameters.Add(param1);

            ExecuteNonQueryCommand(command);
        }

        /// <summary>
        /// Used to delelete a image from the database
        /// </summary>
        public void DeleteImage(int id)
        {
            // Creation of SqlCommand and sets it as a Stored Procedure
            SqlCommand command = new SqlCommand("DeletePicture", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Adding the first Parameter, which is the id of the image
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@id";
            param.Value = id;
            command.Parameters.Add(param);

            ExecuteNonQueryCommand(command);
        }

        /// <summary>
        /// Used to read all data from database and store it to lists
        /// </summary>
        public void ReadAll()
        {
            ReadData(new SqlCommand("EXEC SelectPicture", connection));
        }

        // Used to read data from database
        private void ReadData(SqlCommand command)
        {
            // Used to clear old content from lists
            ClearLists();

            // Open connection to database
            connection.Open();
            SqlDataReader sqlDataReader = command.ExecuteReader();

            // Loops through database
            rows = 0;
            while(sqlDataReader.Read())
            {
                idArray.Add((int)sqlDataReader[0]);
                nameArray.Add((string)sqlDataReader[1]);
                binaryList.Add((byte[])sqlDataReader[2]);
                rows++;
            }

            // Close connection to database
            connection.Close();
        }

        // Used to clear lists
        private void ClearLists()
        {
            idArray.Clear();
            nameArray.Clear();
            binaryList.Clear();
        }

        // Used to execute not query
        private void ExecuteNonQueryCommand(SqlCommand command)
        {
            // Open connection to database - execute command - close connection
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
