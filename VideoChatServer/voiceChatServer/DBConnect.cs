using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace voiceChatServer
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "voice_conf";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        //MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        //MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert()
        {
        }

        //Update statement
        public void Update()
        {
        }

        //Delete statement
        public void Delete()
        {
        }

        //Select statement
        public List<string>[] getusers()
        {
            string query = "SELECT * FROM user";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["ID"] + "");
                    list[1].Add(dataReader["un"] + "");
                    list[2].Add(dataReader["role"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            
                
            
        }

        //Count statement
        //public int Count()
        //{
        //}

        //Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }
        public void listuser()
        {

        }
        //signup
        public bool signup(string un, string pw)
        {
            //check username exist or not
            string query = "SELECT * FROM username WHERE un='"+un+"'";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            
            MySqlDataReader dataReader = cmd.ExecuteReader();

            int i = 0;

            while (dataReader.Read())
            {
                string s = dataReader["id"].ToString();
                i++;
            }
            dataReader.Close();

            if (i == 0)
            {
                string query1 = "INSERT INTO username (id,un,pass) VALUES('','"+un+"', '"+pw+"')";


                //create command and assign the query and connection from the constructor
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);

                //Execute command
                cmd1.ExecuteNonQuery();
                return true;

            }
            else
            {
                return false;
            }
        }
        //login
        public bool login(string un, string pw)
        {
            //check username exist or not
            string query = "SELECT * FROM username WHERE un='" + un + "' AND pass='"+pw+"'";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            MySqlDataReader dataReader = cmd.ExecuteReader();

            int i = 0;

            while (dataReader.Read())
            {
                string s = dataReader["id"].ToString();
                i++;
            }
            dataReader.Close();

            if (i > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        //Add Friend
        public bool addfriend(string un, string fr)
        {
            //check username exist or not
            string query = "SELECT * FROM friends WHERE un='" + un + "' AND fr='"+fr+"'";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            MySqlDataReader dataReader = cmd.ExecuteReader();

            int i = 0;

            while (dataReader.Read())
            {
                string s = dataReader["id"].ToString();
                i++;
            }
            dataReader.Close();

            if (i == 0 && un!=fr)
            {
                string query1 = "INSERT INTO friends (id,un,fr) VALUES('','" + un + "', '" + fr + "')";


                //create command and assign the query and connection from the constructor
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);

                //Execute command
                cmd1.ExecuteNonQuery();
                return true;

            }
            else
            {
                return false;
            }
        }
        public string flist(string un)
        {
            string query = "SELECT * FROM friends WHERE un='" + un + "'";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            MySqlDataReader dataReader = cmd.ExecuteReader();

            int i = 0;
            string s=null;

            while (dataReader.Read())
            {
                s = s+" "+dataReader["fr"].ToString();
                i++;
            }
            dataReader.Close();
            return s;
        }
    }
}
