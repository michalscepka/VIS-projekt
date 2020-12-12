using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    /// <summary>
    /// Represents a MS SQL Database
    /// </summary>
    public class Database
    {
        //connection string do skoly
        // "server=dbsys.cs.vsb.cz\\STUDENT;database=userID;user=userID;password=heslo;"
        //connection string do lokalniho souboru
        // "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=Q:\VSB_Vyuka\VIS_2020\Projekt_cv2\Data\Knihovna.mdf;Integrated Security=True;Connect Timeout=30"

        private SqlConnection Connection { get; set; }
        private SqlTransaction SqlTransaction { get; set; }
        private static readonly object m_LockObj = new object();
        private static Database m_Instance = null;

        public static Database Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ??= new Database();
                }
            }
        }

        public Database()
        {
            Connection = new SqlConnection();
        }

        /// <summary>
        /// Connect
        /// </summary>
        public bool Connect(string conString)
        {
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                Connection.ConnectionString = conString;
                Connection.Open();
            }
            return true;

        }

        /// <summary>
        /// Connect
        /// </summary>
        public bool Connect()
        {
            bool ret = true;
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                //Connection string je v konfiguračním souboru xxxx.dll.config
                Connection.ConnectionString = Properties.DBLayer.Default.ConnString;
                Connection.Open();
            }
            return ret;
        }

        /// <summary>
        /// Close
        /// </summary>
        public void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// Begin a transaction.
        /// </summary>
        public void BeginTransaction()
        {
            SqlTransaction = Connection.BeginTransaction(IsolationLevel.Serializable);
        }

        /// <summary>
        /// End a transaction.
        /// </summary>
        public void EndTransaction()
        {
            SqlTransaction.Commit();
            Close();
        }

        /// <summary>
        /// If a transaction is failed call it.
        /// </summary>
        public void Rollback()
        {
            SqlTransaction.Rollback();
        }

        /// <summary>
        /// Insert a record encapulated in the command.
        /// </summary>
        public int ExecuteNonQuery(SqlCommand command)
        {
            int rowNumber = 0;
            try
            {
                rowNumber = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            return rowNumber;
        }

        /// <summary>
        /// Call SQL and return single value
        /// </summary>
        public int ExecuteScalar(SqlCommand command)
        {
            int answer = -1;
            try
            {
                answer = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception e)
            {
                throw e;
            }
            return answer;
        }

        /// <summary>
        /// Create command
        /// </summary>
        public SqlCommand CreateCommand(string strCommand)
        {
            SqlCommand command = new SqlCommand(strCommand, Connection);

            if (SqlTransaction != null)
            {
                command.Transaction = SqlTransaction;
            }
            return command;
        }

        /// <summary>
        /// Select encapulated in the command.
        /// </summary>
        public SqlDataReader Select(SqlCommand command)
        {
            SqlDataReader sqlReader = command.ExecuteReader();
            return sqlReader;
        }

    }
}
