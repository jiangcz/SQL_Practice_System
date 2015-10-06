using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Web.Security;
using System.Configuration;
using System.Data.SqlClient;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        FormsAuthentication.Initialize();
        ProgressBarModalPopupExtender.Show();
        //string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:/SQL Practice Website/TestSite/Database.accdb";
        //string query = "Select Count(*) From Login Where UserName = ? And Pass = ?";
        //int result = 0;
        //using (OleDbConnection conn = new OleDbConnection(connect))
        //{
        //    using (OleDbCommand cmd = new OleDbCommand(query, conn))
        //    {
        //        cmd.Parameters.AddWithValue("", Login1.UserName.ToString());
        //        cmd.Parameters.AddWithValue("", Login1.Password.ToString());
        //        conn.Open();
        //        result = (int)cmd.ExecuteScalar();
        //    }
        //}
        //string connect = "Data Source=localhost;Initial Catalog=Management; User ID=sa; Password=SQL2012@Corley;Pooling=false;";
        //string connect = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\SQL Practice Website\\TestSite\\App_Data\\Management.mdf;Integrated Security=True;";
        //string connect = ConfigurationManager.ConnectionStrings["ManagementConnectionString1"].ConnectionString;
        string query = "Select Count(*) From Users Where UserID = @ID And Password = @Pass COLLATE Latin1_General_CS_AS";
        int result = 0;
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagementConnectionString"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", Login1.UserName);
                cmd.Parameters.AddWithValue("@Pass", Login1.Password);
                conn.Open();
                result = (int)cmd.ExecuteScalar();
            }
        }
        if (result > 0)
        {
            //char[] chs = { '/' };
            //string[] strs = Request["ReturnUrl"].Split(chs);
            //string s = "Data Source=localhost;Initial Catalog=Management; User ID=sa; Password=SQL2012@Corley;Pooling=false;";
            string checkrole = "update users set LastLoginDate=GETDATE() where userid='"
                                + Login1.UserName.ToString()+ "';"
                                + "select Role from users where userid = '" + Login1.UserName.ToString() + "';";
            string usertype = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagementConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(checkrole, conn))
                {
                    conn.Open();
                    usertype = (string)cmd.ExecuteScalar();
                    //cmd.ExecuteReader();
                }
            }
            string roles = "admin";
            if (usertype == "admin")
            {
                // Create a new ticket used for authentication
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                   1, // Ticket version
                   Login1.UserName, // Username associated with ticket
                   DateTime.Now, // Date/time issued
                   DateTime.Now.AddMinutes(30), // Date/time to expire
                   true, // "true" for a persistent user cookie
                   roles, // User-data, in this case the roles
                   FormsAuthentication.FormsCookiePath);// Path cookie valid for

                // Encrypt the cookie using the machine key for secure transport
                string hash = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(
                   FormsAuthentication.FormsCookieName, // Name of auth cookie
                   hash); // Hashed ticket

                // Set the cookie's expiration time to the tickets expiration time
                if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

                // Add the cookie to the list for outgoing response
                Response.Cookies.Add(cookie);

                //Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Loged as " + Login1.UserName + "');</script>"));
                Response.Redirect(@"~\admin\admin.aspx");
                //FormsAuthentication.RedirectFromLoginPage(Login1.UserName, Login1.RememberMeSet);
            }
            else
            {
                FormsAuthentication.RedirectFromLoginPage(Login1.UserName, Login1.RememberMeSet);
            }
        }
        else
        {
            //Login1.FailureText.ToString() = "Invalid credentials";
            ProgressBarModalPopupExtender.Hide();
        }
    }
}