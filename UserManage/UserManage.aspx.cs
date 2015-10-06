using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;

public partial class UserManage_UserManage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //Manually register the event-handling methods.
        ChangePassword1.ChangingPassword += new LoginCancelEventHandler(this.ChangingPassword);
        if (Context.User.Identity.IsAuthenticated)
        {
            ChangePassword1.DisplayUserName = false;
            if (User.IsInRole("admin"))
            {
                AdminPg.Visible = true;
                
            }
            else
            {
                AdminPg.Visible = false;
                
            }
        }

    }
    protected void ChangingPassword(Object sender, LoginCancelEventArgs e)
    {
        if (ChangePassword1.CurrentPassword.ToString() == ChangePassword1.NewPassword.ToString())
        {
            lblError.Visible = true;
            lblError.Text = "Old password and new password must be different.  Please try again.";
            e.Cancel = true;
        }
        else
        {
            //This line prevents the error showing up after a first failed attempt.
            lblError.Visible = false;
            lblError.Text = ChangePassword1.CurrentPassword.ToString();
        }
    }

    protected void loginButton_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(2000);//For testing.
        programmaticModalPopup.Hide();
    }
}