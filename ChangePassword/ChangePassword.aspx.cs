using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;


public partial class ChangePassword_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            Label4.Visible = false;
            txt_user.Visible = false;
            txt_user.Text = User.Identity.Name;
            if (User.IsInRole("admin"))
            {
                AdminPg.Visible = true;
                DBPg.Visible = true;
            }
            else
            {
                AdminPg.Visible = false;
                DBPg.Visible = false;
            }
        }
    }


    protected void ChangedPassword(object sender, EventArgs e)
    {
        string update = "update users set Password = @Pass Where UserID = @ID And Password = @OldPass COLLATE Latin1_General_CS_AS";
        int i = 0;
        if (Page.IsValid)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagementConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(update, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", txt_user.Text);
                    cmd.Parameters.AddWithValue("@Pass", txt_npassword.Text);
                    cmd.Parameters.AddWithValue("@OldPass", txt_opassword.Text);
                    conn.Open();
                    i = cmd.ExecuteNonQuery();
                }
                if (i > 0)
                {
                    lblError.Text = "Password changed Successfully";
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                    //Response.Redirect("~/Default.aspx", true);
                    Response.Write("<script language=Javascript>alert('Your Password Has Been Changed\\nPlease Login With New Password');document.location='/Login.aspx';</script>");
                }
                else
                {
                    Response.Write("<script language=Javascript>alert('Invalid username or password. Please try again.');</script>");
                    Page.Controls.Add(new LiteralControl("<p>If you have any question or encounter any problem logging in,<br />please contact a site administrator.</p>"));
                }
            }
        }
    }
    protected void ComparePwd(object source, ServerValidateEventArgs args)
    {
        if (txt_opassword.Text.ToString() == txt_npassword.Text.ToString())
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
    }

    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        txt_user.Text = string.Empty;
        foreach (Control c in Controls)
        {
            if (c is TextBox)
            {
                ((TextBox) c).Text = string.Empty;
            }
        }
    }
}