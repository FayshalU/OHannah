using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace OHannah
{
    class DataAccess
    {
        SqlConnection con;

        public DataAccess()
        {
            con = new SqlConnection(@"Data Source=Fayshal;Initial Catalog=OHannah;Integrated Security=True");
            con.Open();
        }

        public bool CheckUser(string id, string pass)
        {
            string s = "Select  Email , Phone , Name  FROM [OHannah].[dbo].[User] where UserId = '" + id + "' AND Password = '" + pass + "'";// "Select  Email , Phone , Name FROM User where UserId = '" + id + "' AND Password = '" + pass + "')";

            SqlCommand str = new SqlCommand(s, con);

            
            SqlDataReader reader = str.ExecuteReader();
            while (reader.Read())
            {
                LogIn.email = reader.GetString(0);
                LogIn.Number = reader.GetInt32(1).ToString();
                LogIn.name = reader.GetString(2);

            }

            reader.Close();

            if (LogIn.email != null)
            {
                //MessageBox.Show(email);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InsertUser(string userId, string password, string email, string phone, string name)
        {
            string s = "insert into [OHannah].[dbo].[User] values('" + userId + "','" + password + "','" + email + "','" + phone + "','" + name + "')";
            SqlCommand str = new SqlCommand(s, con);
            str.ExecuteNonQuery();
            //MessageBox.Show("User added");
        }

        public bool CheckID(string id)
        {
            string s = "Select  Email , Phone , Name  FROM [OHannah].[dbo].[User] where UserId = '" + id + "'";// "Select  Email , Phone , Name FROM User where UserId = '" + id + "' AND Password = '" + pass + "')";

            SqlCommand str = new SqlCommand(s, con);

            SqlDataReader reader = str.ExecuteReader();

            string temp = null;
            while (reader.Read())
            {
                temp = reader.GetString(0);
            }

            reader.Close();

            if (temp != null)
            {
                //MessageBox.Show(email);
                return false;
            }
            else
            {
                return true;
            }
        }

        public DateTime GetAlarm(string id)
        {

            string s = "SELECT ISNULL( (SELECT Min(Time) FROM [OHannah].[dbo].[Alarm] WHERE UserId = '" + id + "') , 0)";

            SqlCommand str = new SqlCommand(s, con);


            SqlDataReader reader = str.ExecuteReader();

            DateTime date = new DateTime(0001, 1, 1);

            while (reader.Read())
            {
                //MessageBox.Show("Out");
                date =  reader.GetDateTime(0);//.ToShortDateString();

            }

            reader.Close();

            return date;
        }

        public void InsertAlarm(DateTime d,string id)
        {

            string s = "insert into Alarm values('" + id + "','" + d + "')";
            SqlCommand str = new SqlCommand(s, con);
            str.ExecuteNonQuery();

        }

        public void DeleteAlarm(string id)
        {

            string s = "Delete FROM Alarm where UserId = '" + id + "' AND Time = ( Select Min(Time) FROM Alarm where UserId = '" + id + "')";// + userId;
            SqlCommand str = new SqlCommand(s, con);
            str.ExecuteNonQuery();
            //aDate = null;
        }

        public void GetDate(string id)
        {

            string s = "Select  Date , Message FROM [OHannah].[dbo].[Reminder] where UserId = '" + id + "' AND Date = ( Select Min(Date) FROM Reminder where UserId = '" + id + "')";// + userId;
            SqlCommand str = new SqlCommand(s, con);

            SqlDataReader reader = str.ExecuteReader();
            while (reader.Read())
            {
                Reminder.rMessage = reader.GetString(1);
                Reminder.rDate = reader.GetDateTime(0).ToShortDateString();

            }
            reader.Close();
            
        }

        public void InsertDate(DateTime d, string id, string Message)
        {

            string s = "insert into Reminder values('" + id + "','" + d + "','" + Message + "')";
            SqlCommand str = new SqlCommand(s, con);
            str.ExecuteNonQuery();

        }

        public void DeleteDate(string id)
        {

            string s = "Delete FROM Reminder where UserId = '" + id + "' AND Date = ( Select Min(Date) FROM Reminder where UserId = '" + id + "')";// + userId;
            SqlCommand str = new SqlCommand(s, con);
            str.ExecuteNonQuery();
            Reminder.rDate = null;
            Reminder.rMessage = null;
        }

    }
}
