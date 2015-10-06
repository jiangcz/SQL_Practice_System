using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class DatabaseManagement_DatabaseManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
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
            FormsAuthentication.RedirectToLoginPage("~/login.aspx");
        lblError.Text = string.Empty;
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Make sure the current GridViewRow is a data row.
        if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex != e.Row.RowIndex)
        {
            // Add client-side confirmation when deleting.
            (e.Row.Cells[3].Controls[2] as Button).Attributes["onclick"] = "if(!confirm('Do you want to delete this row?')) return false;";
        }
    }
    protected void Insert(object sender, EventArgs e)
    {
        try
        {
            SqlDataSource1.Insert();
            lblError.Text = "The database has been added";
            //empty insert row
            txtDBName.Text = string.Empty;
        }
        catch (Exception error)
        {
            Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Insert Failed!\\nCannot insert duplicate key in object Database or column does not allow nulls');</script>"));
            lblError.Text = "Error: " + error.Message;
        }
    }

    protected void OnRowDataBound2(object sender, GridViewRowEventArgs e)
    {
        // Make sure the current GridViewRow is a data row.
        if (e.Row.RowType == DataControlRowType.DataRow && GridView2.EditIndex != e.Row.RowIndex)
        {
            // Add client-side confirmation when deleting.
            (e.Row.Cells[3].Controls[0] as Button).Attributes["onclick"] = "if(!confirm('Do you want to delete this row?')) return false;";
        }
    }

    protected void DetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        if (e.Exception == null && e.AffectedRows == 1)
        {
            // Use the Values property to get the value entered by 
            // the user for the CompanyName field.
            String name = e.Values["DBName"].ToString();

            // Display a confirmation message.
            lblError.Text = name + " added successfully. ";
            GridView2.DataBind();

        }
        else
        {
            // Insert the code to handle the exception.
            lblError.Text = e.Exception.Message;
            Response.Write("<script language=Javascript>alert('Cannot insert duplicate key row!');</script>");
            // Use the ExceptionHandled property to indicate that the 
            // exception is already handled.
            e.ExceptionHandled = true;

            // When an exception occurs, keep the DetailsView
            // control in insert mode.
            e.KeepInInsertMode = true;
        }
    }
    protected void DetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        if (e.Exception == null && e.AffectedRows == 1)
        {
            GridView2.DataBind();
        }
        else
        {
            // Insert the code to handle the exception.
            lblError.Text = e.Exception.Message;
            Response.Write("<script language=Javascript>alert('Cannot insert duplicate key row!');</script>");
            // Use the ExceptionHandled property to indicate that the 
            // exception is already handled.
            e.ExceptionHandled = true;
            // When an exception occurs, keep the DetailsView
            // control in insert mode.
            e.KeepInEditMode = true;
        }
    }
    
}