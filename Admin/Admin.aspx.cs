using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;


public partial class Admin_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page.User.Identity.IsAuthenticated)
        {
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
        else
        { 
            FormsAuthentication.RedirectToLoginPage("~/login.aspx"); 
        }
        lblError.Text = string.Empty;
        //if (User.IsInRole("admin"))
            //Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Loged as " + User.Identity.Name + "');</script>")); 
    }

    //refer code.msdn.microsoft.com/CSASPNETGridView-5b16ce70
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Make sure the current GridViewRow is a data row.
        if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex != e.Row.RowIndex)
        {
            // Add client-side confirmation when deleting.
            (e.Row.Cells[5].Controls[2] as Button).Attributes["onclick"] = "if(!confirm('Do you want to delete this row?')) return false;"; 
        }
    }
    protected void Insert(object sender, EventArgs e)
    {
        try
        {
            SqlDataSource1.Insert();
            lblError.Text = "The user " + txtUserID.Text + " has been added";
            txtUserID.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }
        catch (Exception error)
        {
            Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Insert Failed!\\nCannot insert duplicate key in object Users or column does not allow nulls');</script>"));
            lblError.Text = "Error: " + error.Message;
        }
    }
}